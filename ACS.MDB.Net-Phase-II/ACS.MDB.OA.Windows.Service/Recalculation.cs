using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;


namespace ACS.MDB.OA.Windows.Service
{
    public class Recalculation
    {

        InflationIndexDAL inflationIndexDAL = null;

        List<string> errorList = new List<string>();

        private decimal upliftReturn;
        private double upliftCountDivisor, upliftCount;
        private DateTime upliftDateFrom, upliftDateTo;
        private decimal uplift, upliftFixed, upliftFinal, previousUplift;
        private decimal iPreIndex, iCurrentIndex, dPercentage;
        private int upliftId, iLoop;
        private bool isUsedIndex; // this variable is use to check if the value is updated or not

        /// <summary>
        /// Recalculate milestone 
        /// </summary>
        private List<ContractMaintenanceVO> GetContractMaintenanceForRecalculate(List<int> Ids)
        {
            ContractMaintenanceDAL ContractMaintenanceDAL = new ContractMaintenanceDAL();
            List<ContractMaintenanceVO> contractMaintenanceVoList = ContractMaintenanceDAL.GetContractMaintenanceForRecalculate(Ids);

            return contractMaintenanceVoList;
        }

        /// <summary>
        /// Validate recalculation for ad-hoc & credit
        /// </summary>
        /// <param name="Ids"></param>
        private void ValidateRecalculate(List<int> Ids)
        {
            List<ContractMaintenance> contractMaintenances = inflationIndexDAL.ValidateAdHocAndCredit(Ids);
            if (contractMaintenances.Count > 0)
            {
                throw new ApplicationException(Constants.VALIDATION_RECALCULATION);
            }
        }

        /// <summary>
        /// Get Inflation index details by Id
        /// </summary>
        /// <param name="indexId">index Id</param>
        /// <returns>IndexId Details</returns>
        public InflationIndexVO GetInflationIndexById(int indexId = 0)
        {
            return inflationIndexDAL.GetInflationIndexById(indexId);
        }

        /// <summary>
        /// Recalculate the Milestones
        /// </summary>
        /// <param name="recalculationVO"></param>
        /// <param name="userId"></param>
        public void RecalculateMilestone(RecalculationVO recalculationVO, int? userId)
        {

            inflationIndexDAL = new InflationIndexDAL();

            List<int> indexIds = new List<int>();
            errorList.Clear();

            //Set Recalculation Status to In progress
            inflationIndexDAL.SetRecalculationStatus(recalculationVO, System.Convert.ToInt32(Constants.RecalculationStatus.IN_PROGRESS));

            if (recalculationVO != null)
            {
                if (recalculationVO.IndexIds.Contains(";"))
                {
                    string[] ids = recalculationVO.IndexIds.Split(';');
                    indexIds.AddRange(ids.Select(id => Convert.ToInt32(id)));
                }
                else
                {
                    indexIds.Add(Convert.ToInt32(recalculationVO.IndexIds));
                }
            }

            ValidateRecalculate(indexIds);

            List<MilestoneVO> milestoneToSave = new List<MilestoneVO>();
            List<ContractMaintenanceVO> contractMaintenanceVoList = GetContractMaintenanceForRecalculate(indexIds);

            foreach (var contractMaintenanceVO in contractMaintenanceVoList)
            {
                bool isValid = ValidateMaintenanceBillingLines(contractMaintenanceVO, milestoneToSave, userId);
                
                    if (isValid)
                    {
                        //Reset counter Bug no# 1515
                        DateTime? currentDate = null;
                        DateTime? renewalEndDate = null;
                        DateTime? currentDate1 = null;
                        DateTime? previousDate = null;
                        DateTime? renewalEndDate1 = null;
                        DateTime? currentEndDate = null;
                        Decimal previousAmount = 0;
                        Decimal currentAmount = 0;
                        Decimal previousAmount1 = 0;
                        Decimal currentAmount1 = 0;
                        iLoop = 0; //this variable is use to recognize the first item
                        upliftCount = -1;
                        upliftCountDivisor = 0;
                        iPreIndex = 0;

                        //Get if inflation index associated with billing details, if yes get it's UseIndex value and set in isUsedIndex.
                        if (contractMaintenanceVO.InflationIndexId.HasValue && contractMaintenanceVO.InflationIndexId.Value > 0)
                        {
                            InflationIndexVO inflationIndexVO = GetInflationIndexById(contractMaintenanceVO.InflationIndexId.Value);
                            isUsedIndex = inflationIndexVO.UseIndex;
                        }

                        if (contractMaintenanceVO.FirstRenewalDate != null)
                        {
                            //Set inflation fixed additional
                            if (contractMaintenanceVO.InflationFixedAdditional.HasValue)
                            {
                                contractMaintenanceVO.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional / 100;
                            }


                            //Set start date, last date, start amount, next amount
                            SetStartDate(contractMaintenanceVO, ref currentDate, ref renewalEndDate, ref currentDate1,
                                         ref renewalEndDate1);
                            currentEndDate = SetLastDate(contractMaintenanceVO);
                            SetStartAmount(contractMaintenanceVO, ref previousAmount, ref currentAmount,
                                           ref previousAmount1, ref currentAmount1);
                            SetNextAmount(contractMaintenanceVO, isUsedIndex, ref currentDate, ref renewalEndDate,
                                          ref previousAmount, ref currentAmount);

                            //Add/Update first milestone line
                            MilestoneVO firstMilestone = AddFirstMilestone(contractMaintenanceVO, currentDate1,
                                                                           renewalEndDate1, currentAmount1,
                                                                          previousAmount, userId);


                            if (firstMilestone != null)
                            {
                                MilestoneVO mileStone = contractMaintenanceVO.Milestones.FirstOrDefault(milestone => firstMilestone.RenewalStartDate == milestone.RenewalStartDate && firstMilestone.RenewalEndDate == milestone.RenewalEndDate);

                                if (mileStone == null)
                                {
                                    //if milestone is null then add new milestone
                                    milestoneToSave.Add(firstMilestone);
                                }
                                // if milestone status is ready for calculation then update existing records
                                else if (mileStone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                                {
                                    milestoneToSave.Add(firstMilestone);
                                }
                            }

                            //Iterate till current date is less then current end date
                            while (currentDate.Value < currentEndDate.Value)
                            {
                                MilestoneVO milestone = contractMaintenanceVO.Milestones.Find(x => x.RenewalStartDate == currentDate && !x.IsDeleted);

                                if (milestone == null)
                                {
                                    //Add milestone
                                    milestoneToSave.Add(AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate,
                                                                     currentAmount, previousAmount, userId));
                                }
                                else
                                {
                                    //Edit milestone
                                    if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                                    {
                                        //Edit milestone
                                        milestoneToSave.Add(EditMilestone(milestone, contractMaintenanceVO, currentDate,
                                                                          renewalEndDate,
                                                                          currentAmount, previousAmount, userId));
                                    }
                                    else
                                    {
                                        //Get amount so, all generated milestones below this will use this amount
                                        currentAmount = milestone.Amount;
                                        currentDate = milestone.RenewalStartDate;
                                    }
                                }

                                //Set next date, next amount for nex milestone
                                SetNextDate(contractMaintenanceVO, ref currentDate, ref renewalEndDate, ref previousDate);
                                SetNextAmount(contractMaintenanceVO, isUsedIndex, ref currentDate, ref renewalEndDate,
                                              ref previousAmount, ref currentAmount);
                            }
                        }

                        //Delete milestones whose renewal start date is greater then current date after generation of all invoices
                        foreach (var item in contractMaintenanceVO.Milestones.FindAll(x => x.RenewalStartDate.Value >= currentDate
                            && x.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))) 
                        //(contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                        //&& contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))))
                        {
                            item.IsDeleted = true;
                            item.LastUpdatedByUserId = userId;
                            milestoneToSave.Add(item);
                        }

                        //Set last date. This will be used to identify that invoices are to be stooped on given final billing period start date.
                        DateTime? lastDate = GetLastMilestoneDate(previousDate, contractMaintenanceVO.Milestones, currentDate);

                        MilestoneVO lastMilestone = null;

                        if (lastDate == null || lastDate < contractMaintenanceVO.FinalRenewalStartDate)
                        {
                            //Insert End Line
                            lastMilestone = (AddORUpdateLastMilestone(contractMaintenanceVO, currentDate,
                                                    previousDate, renewalEndDate, currentAmount,
                                                    previousAmount, userId, null));
                        }
                        else
                        {
                            MilestoneVO milestoneVO = contractMaintenanceVO.Milestones.Find(x => x.RenewalStartDate == contractMaintenanceVO.FinalRenewalStartDate);
                            if (milestoneVO != null)
                            {
                                //Update End Line
                                lastMilestone = (AddORUpdateLastMilestone(contractMaintenanceVO, currentDate,
                                                previousDate, renewalEndDate, currentAmount,
                                             previousAmount, userId, milestoneVO));

                            }
                        }

                        //Add last milestone for save
                        if (lastMilestone != null)
                        {
                            //lastMilestone.Uplift = 0;
                            //lastMilestone.UpliftFixedRate = 0;
                            //lastMilestone.IndexRate = 0;

                            milestoneToSave.Add(lastMilestone);
                        }
                    }
            }

            MilestoneDAL milestoneDal = new MilestoneDAL();
            milestoneDal.SaveRecalculatedMilestone(milestoneToSave);

            //Set Recalculation status after completing Recalculation process
            if (errorList.Count > 0)
            {
                recalculationVO.LogFilePath = WriteLogForRecalculation(errorList);

                // Set recalculation status as completed with errors
                inflationIndexDAL.SetRecalculationStatus(recalculationVO, System.Convert.ToInt32(Constants.RecalculationStatus.COMPLETED_WITH_ERRORS));
            }
            else
            {
                // Set recalculation status as completed successfully
                inflationIndexDAL.SetRecalculationStatus(recalculationVO, System.Convert.ToInt32(Constants.RecalculationStatus.COMPLETED));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <returns></returns>
        private bool ValidateMaintenanceBillingLines(ContractMaintenanceVO contractMaintenanceVO, List<MilestoneVO> milestoneToSave, int? userId)
        {
            bool isValid = true;

            List<MaintenanceBillingLineVO> maintenanceBillingLineVOs = contractMaintenanceVO.MaintenanceBillingLineVos;

            // Get default printing line for coding details
            if (contractMaintenanceVO.IsGrouped == true && contractMaintenanceVO.GroupId != null && contractMaintenanceVO.IsDefaultLineInGroup == false)
            {
                maintenanceBillingLineVOs = GetDefaultLinesFromContractMaintenanceGroup(contractMaintenanceVO.ContractId, contractMaintenanceVO.PeriodFrequencyId, (int) contractMaintenanceVO.GroupId);
            }

            string billingLines = maintenanceBillingLineVOs.Aggregate(string.Empty, (current, maintenanceBillingLines) => current + maintenanceBillingLines.LineText);

            //Validate maintenance Billing line exist or not
            if (string.IsNullOrEmpty(billingLines))
            {
               
                errorList.Add(Constants.NO_INVOICE_TEXT_LINE_TO_CALCULATE + " Please review contract number - '" +
                                               contractMaintenanceVO.ContractNumber + "' with company - '" + contractMaintenanceVO.CompanyName
                                               + "', Customer - '" + contractMaintenanceVO.InvoiceCustomer + " - " + contractMaintenanceVO.CustomerCode + "'");
                isValid = false;

                //Delete milestones whose status is Ready for calculating
                foreach (var item in contractMaintenanceVO.Milestones.FindAll(x => x.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING)
                            && (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                            && contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))))
                {
                    item.IsDeleted = true;
                    item.LastUpdatedByUserId = userId;
                    milestoneToSave.Add(item);
                }
            }
            else
            { 
                    //Loop though all billing details billing lines
                foreach (MaintenanceBillingLineVO maintenanceBillingLine in maintenanceBillingLineVOs)
                {

                    if (!ValidateBillingLine(maintenanceBillingLine.LineText))
                    {
                        errorList.Add(String.Format(Constants.INVALID_BILLING_TEXT + " Please review contract number - '"
                            + contractMaintenanceVO.ContractNumber + "' with company - '" + contractMaintenanceVO.CompanyName
                            + "', Customer - '" + contractMaintenanceVO.InvoiceCustomer + " - " + contractMaintenanceVO.CustomerCode + "'",
                            maintenanceBillingLine.LineSequance));

                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Get the Maintenance billing line details from group
        /// </summary>
        /// <returns>Return maintenance lines of default contract maintenance line</returns>
        private List<MaintenanceBillingLineVO> GetDefaultLinesFromContractMaintenanceGroup(int contractId, int chargedFrequency, int groupId)
        {
            List<MaintenanceBillingLineVO> maintenanceBillingLineVOs = new List<MaintenanceBillingLineVO>();
            ContractMaintenanceGroupDAL contractMaintenanceGroupDAL = new ContractMaintenanceGroupDAL();

            List<ContractMaintenanceVO> contractMaintenanceVos =
                contractMaintenanceGroupDAL.GetContractMaintenanceGroupListByGroupId(
                    contractId, chargedFrequency, groupId);


            var singleOrDefault = contractMaintenanceVos.SingleOrDefault(cm => cm.IsDefaultLineInGroup == true);

            if (singleOrDefault != null)
                maintenanceBillingLineVOs = singleOrDefault.MaintenanceBillingLineVos;
            
            return maintenanceBillingLineVOs;
        }

        #region Milestone caluclation

        /// <summary>
        /// Add last milestone.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <param name="currentDate"></param>
        /// <param name="renewalEndDate"></param>
        /// <param name="currentAmount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private MilestoneVO AddORUpdateLastMilestone(ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
            DateTime? previousDate, DateTime? renewalEndDate, Decimal? currentAmount, decimal? previousAmount,
            int? userId, MilestoneVO milestoneVO)
        {
            decimal? endAmount = null;
            if (contractMaintenanceVO.FinalRenewalStartDate.HasValue)
            {
                if (!contractMaintenanceVO.EndAmount.HasValue)
                {
                    if (contractMaintenanceVO.FinalRenewalStartDate > previousDate)
                    {
                        DateTime newYearDate = previousDate.Value.AddYears(1);
                        int daysInYear = (newYearDate - previousDate.Value).Days;
                        endAmount = currentAmount;
                        //endAmount = previousAmount * ((contractMaintenanceVO.FinalRenewalStartDate.Value - previousDate.Value).Days / daysInYear);
                        //endAmount = endAmount + Math.Round(endAmount.Value * GetUplift(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value));
                    }
                    else
                    {
                        endAmount = currentAmount;
                    }
                }
                else
                {
                    endAmount = contractMaintenanceVO.EndAmount;
                }

                //@TODO : Need to check. if this requires
                currentDate = contractMaintenanceVO.FinalRenewalStartDate;
                renewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
            }

            previousAmount = currentAmount;
            if (endAmount != null)
            {
                currentAmount = endAmount;
            }

            MilestoneVO lastMilestone = null;
            if (milestoneVO == null)
            {
                lastMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate,
                                                         currentAmount, previousAmount, userId);
                if (contractMaintenanceVO.EndAmount != null)
                {
                    lastMilestone.Uplift = 0;
                    lastMilestone.UpliftFixedRate = 0;
                    lastMilestone.IndexRate = null;
                }
                
            }
            else
            {
                //Set defalt milestone status
                //milestoneVO.MilestoneStatusName = "RC";
                //milestoneVO.MilestoneStatusID = 9;
                //milestoneVO.IsApprovalRequired = true;
                //milestoneVO.ApprovedStatus = "U";

                //Edit milestone
                if (milestoneVO.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING)
                    || String.IsNullOrEmpty(milestoneVO.MilestoneStatusName))
                {
                    lastMilestone = EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);

                    if (contractMaintenanceVO.EndAmount != null)
                    {
                        lastMilestone.Uplift = 0;
                        lastMilestone.UpliftFixedRate = 0;
                        lastMilestone.IndexRate = null;
                    }
                }
            }
            return lastMilestone;
        }

        /// <summary>
        /// Get last date for the last milestone
        /// </summary>
        /// <param name="previousDate">The perivous date</param>
        /// <param name="milestones">The milestone list</param>
        /// <param name="CurrentDate">The current date</param>
        /// <returns>The late milestone date</returns>
        private DateTime? GetLastMilestoneDate(DateTime? previousDate, List<MilestoneVO> milestones, DateTime? currentDate)
        {
            DateTime? lastDate = previousDate;

            if (milestones.Count != 0)
            {
                MilestoneVO milestoneVO = milestones.OrderBy(x => x.RenewalStartDate).LastOrDefault(x => x.IsDeleted == false);
                if (milestoneVO != null)
                {
                    lastDate = milestoneVO.RenewalStartDate;
                }
                //else
                //{
                //    lastDate = currentDate;
                //}
            }
            return lastDate;
        }

        /// <summary>
        /// Add first milestone. In case if first period start date is not same as renewal date.
        /// </summary>
        /// <param name="contractMaintenanceVO">The contract maintenanceVO object</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal end date</param>
        /// <param name="currentAmount">The current amount</param>
        /// <param name="previousAmount">previous amount</param>
        /// <param name="userId">The user id</param>
        private MilestoneVO AddFirstMilestone(ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
             DateTime? renewalEndDate, Decimal? currentAmount, decimal previousAmount, int? userId)
        {
            MilestoneVO firstMilestone = null;

            //Add/Update first billing line.
            //This needs to be done if user has provided first period start date.
            if (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC) && contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                if (contractMaintenanceVO.FirstRenewalDate.HasValue && contractMaintenanceVO.FirstPeriodStartDate.HasValue &&
                    contractMaintenanceVO.FirstRenewalDate.Value > contractMaintenanceVO.FirstPeriodStartDate.Value)
                {
                    //Add first milestone
                    if (contractMaintenanceVO.Milestones.Count == 0)
                    {
                        firstMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                        firstMilestone = SetUpliftToZeroForFirstMilestone(firstMilestone);

                        //firstMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                    }
                    else
                    {
                        MilestoneVO milestoneVO = contractMaintenanceVO.Milestones.Find(x => x.RenewalStartDate == currentDate);
                        if (milestoneVO != null && milestoneVO.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                        {
                            //Edit first milestone
                            firstMilestone = EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                            firstMilestone = SetUpliftToZeroForFirstMilestone(firstMilestone);
                            //firstMilestone = EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                        }
                        else
                        {
                            firstMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                            firstMilestone = SetUpliftToZeroForFirstMilestone(firstMilestone);

                            //firstMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                        }
                    }
                }
            }
            return firstMilestone;
        }

        /// <summary>
        /// Add new milestone.
        /// </summary>
        /// <returns>Returns object to add</returns>
        private MilestoneVO AddMilestone(ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
            DateTime? renewalEndDate, Decimal? currentAmount, decimal? previousAmount, int? userId)
        {
            MilestoneVO milestoneVO = new MilestoneVO();

            milestoneVO.ContractID = contractMaintenanceVO.ContractId;
            milestoneVO.ContractLineID = contractMaintenanceVO.ContractLineId;

            // Period Frequency = Credit
            milestoneVO.Description = contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT) ? "Maintenance Credit" : Constants.MAINTENANCE;

            milestoneVO.MilestoneStatusName = "RC";
            milestoneVO.MilestoneStatusID = Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING);
            milestoneVO.IsApprovalRequired = true;
            //milestoneVO.ApprovedStatus = "U";
            milestoneVO.IsApproved = false;
            milestoneVO.ContractMaintenanceID = contractMaintenanceVO.ID;
            milestoneVO.InvoiceDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value);
            milestoneVO.RenewalStartDate = currentDate;
            milestoneVO.RenewalEndDate = renewalEndDate;
            milestoneVO.Amount = currentAmount.Value;
            milestoneVO.CompanyName = contractMaintenanceVO.CompanyName;
            milestoneVO.ContractNumber = contractMaintenanceVO.ContractNumber;

            //@TODO: add entries of index rate, percentage, index id
            if (contractMaintenanceVO.UpliftRequired.HasValue)
            {
                milestoneVO.PreviousValue = previousAmount;
                milestoneVO.Uplift = upliftFinal;
                milestoneVO.UpliftFixedRate = upliftFixed;
                milestoneVO.ChargingUpliftID = upliftId;
                milestoneVO.UpliftRate = uplift;
                // milestoneVO.Percentage =
            }
            else
            {
                milestoneVO.PreviousValue = previousAmount;
                milestoneVO.Uplift = null;
                milestoneVO.UpliftFixedRate = null;
                milestoneVO.ChargingUpliftID = null;
                milestoneVO.UpliftRate = null;
            }

            // add index rate to table
            if (isUsedIndex)
            {
                milestoneVO.IndexRate = Math.Round(iPreIndex, 2);
                milestoneVO.UpliftPercentage = uplift;
            }
            else
            {
                milestoneVO.IndexRate = null;
                milestoneVO.UpliftPercentage = null;
            }

            milestoneVO.ConditionType = "MS";
            milestoneVO.CreatedByUserId = userId;

            MilestoneDAL milestoneDal = new MilestoneDAL();

            List<MilestoneBillingLineVO> milestoneBillingLineVos = milestoneDal.GetMilestoneBillingLines(milestoneVO);
            milestoneVO.MilestoneBillingLineVos = milestoneBillingLineVos;

            //foreach (var milestoneBillingLineVo in milestoneBillingLineVos)
            //{
            //    //milestoneVO.MilestoneBillingLines.Add(new Models.MilestoneBillingLine(milestoneBillingLineVo));
            //    milestoneVO.MilestoneBillingLineVos.Add(new MilestoneBillingLineVO(milestoneBillingLineVo));
            //}

            CalculateBillingLineText(milestoneVO, contractMaintenanceVO);

            //@TODO: add this values if require
            //milestoneVO.PreviousLine
            //milestoneVO.PreviousValue =

            return milestoneVO;
        }

        /// <summary>
        /// Edit the milestone.
        /// </summary>
        /// <returns>Returns updated object</returns>
        private MilestoneVO EditMilestone(MilestoneVO milestoneVO, ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
            DateTime? renewalEndDate, Decimal? currentAmount, Decimal? previousAmount, int? userId)
        {
            milestoneVO.InvoiceDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value);
            milestoneVO.PreviousValue = previousAmount;
            milestoneVO.RenewalStartDate = currentDate;
            milestoneVO.RenewalEndDate = renewalEndDate;
            milestoneVO.Amount = currentAmount.Value;
            milestoneVO.IsDeleted = false;
            milestoneVO.MilestoneStatusID = Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING); ;
            milestoneVO.MilestoneStatusName = "RC";
            milestoneVO.ContractID = contractMaintenanceVO.ContractId;
            milestoneVO.ContractLineID = contractMaintenanceVO.ContractLineId;

            //Add entries of index rate, percentage, index id
            if (contractMaintenanceVO.UpliftRequired.HasValue)
            {
                milestoneVO.Uplift = upliftFinal;
                milestoneVO.UpliftFixedRate = upliftFixed;
                milestoneVO.ChargingUpliftID = upliftId;
                milestoneVO.UpliftRate = uplift;
            }
            else
            {
                milestoneVO.Uplift = null;
                milestoneVO.UpliftFixedRate = null;
                milestoneVO.ChargingUpliftID = null;
                milestoneVO.UpliftRate = null;
            }

            //chinh update - add index rate to table
            if (isUsedIndex)
            {
                milestoneVO.IndexRate = Math.Round(iPreIndex, 2);
                milestoneVO.UpliftPercentage = uplift;
            }
            else
            {
                milestoneVO.IndexRate = null;
                milestoneVO.UpliftPercentage = null;
            }

            milestoneVO.LastUpdatedByUserId = userId;

            MilestoneDAL milestoneDal = new MilestoneDAL();

            List<MilestoneBillingLineVO> milestoneBillingLineVos = milestoneDal.GetMilestoneBillingLines(milestoneVO);
            milestoneVO.MilestoneBillingLineVos = milestoneBillingLineVos;

            //foreach (var milestoneBillingLineVo in milestoneBillingLineVos)
            //{
            //    milestoneVO.MilestoneBillingLines.Add(new Models.MilestoneBillingLine(milestoneBillingLineVo));
            //}

            CalculateBillingLineText(milestoneVO, contractMaintenanceVO);

            return milestoneVO;
        }

        /// <summary>
        /// This method is used to set next milestone current date and renewal end date based on period frequency.
        /// </summary>
        /// <param name="contractMaintenanceVO">The ContractMaintenanceVO</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal date</param>
        /// <param name="previousDate">The previous date</param>
        private void SetNextDate(ContractMaintenanceVO contractMaintenanceVO, ref DateTime? currentDate, ref DateTime? renewalEndDate, ref DateTime? previousDate)
        {
            previousDate = currentDate;
            if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
            {
                currentDate = currentDate.Value.AddYears(1);
                renewalEndDate = currentDate.Value.AddYears(1).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
            {
                currentDate = currentDate.Value.AddMonths(6);
                renewalEndDate = currentDate.Value.AddMonths(6).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
            {
                currentDate = currentDate.Value.AddMonths(3);
                renewalEndDate = currentDate.Value.AddMonths(3).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
            {
                currentDate = currentDate.Value.AddMonths(2);
                renewalEndDate = currentDate.Value.AddMonths(2).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
            {
                currentDate = currentDate.Value.AddMonths(1);
                renewalEndDate = currentDate.Value.AddMonths(1).AddDays(-1);
            }
        }

        /// <summary>
        /// Set start date for the milestone invoice date.
        /// </summary>
        /// <param name="contractMaintenanceVO">The contract maintenance VO</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal end date</param>
        private void SetStartDate(ContractMaintenanceVO contractMaintenanceVO, ref DateTime? currentDate, ref DateTime? renewalEndDate, ref DateTime? currentDate1, ref DateTime? renewalEndDate1)
        {
            //If charging frequency id Ad-hoc or Credit
            if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                || contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                currentDate = contractMaintenanceVO.FinalRenewalStartDate;
                renewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
            }
            else
            {
                currentDate = contractMaintenanceVO.FirstRenewalDate;

                //For pro-rata when first period start date is less than same as first renewal date
                if (contractMaintenanceVO.FirstPeriodStartDate.HasValue &&
                    contractMaintenanceVO.FirstRenewalDate.HasValue &&
                    contractMaintenanceVO.FirstRenewalDate > contractMaintenanceVO.FirstPeriodStartDate)
                {
                    currentDate1 = contractMaintenanceVO.FirstPeriodStartDate;
                    renewalEndDate1 = contractMaintenanceVO.FirstRenewalDate.Value.AddDays(-1);
                }
            }

            if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
            {
                renewalEndDate = currentDate.Value.AddYears(1).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
            {
                renewalEndDate = currentDate.Value.AddMonths(6).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
            {
                renewalEndDate = currentDate.Value.AddMonths(3).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
            {
                renewalEndDate = currentDate.Value.AddMonths(2).AddDays(-1);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
            {
                renewalEndDate = currentDate.Value.AddMonths(1).AddDays(-1);
            }
        }

        /// <summary>
        /// Set last date. The date till invoice will generate.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        private DateTime SetLastDate(ContractMaintenanceVO contractMaintenanceVO)
        {
            DateTime tempDate;
            DateTime currentEndDate;

            if (!contractMaintenanceVO.FirstRenewalDate.HasValue)
            {
                tempDate = DateTime.Now;
            }
            else
            {
                tempDate = contractMaintenanceVO.FirstRenewalDate.Value.Day == 29 && contractMaintenanceVO.FirstRenewalDate.Value.Month == 2
                               ? new DateTime(DateTime.Now.Year, 2, 28)
                               : new DateTime(DateTime.Now.Year,
                                              contractMaintenanceVO.FirstRenewalDate.Value.Month,
                                              contractMaintenanceVO.FirstRenewalDate.Value.Day);
            }

            currentEndDate = !contractMaintenanceVO.FinalRenewalStartDate.HasValue
                                                    ? PeriodAdd(contractMaintenanceVO, tempDate, 3 * GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId))
                                                    : contractMaintenanceVO.FinalRenewalStartDate.Value;
            return currentEndDate;
        }

        /// <summary>
        /// Set start amount for the milestone invoice date.
        /// </summary>
        /// <param name="contractMaintenanceVO">The contract maintenance VO</param>
        /// <param name="previousAmount">The previous amount</param>
        /// <param name="currentAmount">The current amount</param>
        private void SetStartAmount(ContractMaintenanceVO contractMaintenanceVO, ref Decimal previousAmount, ref Decimal currentAmount, ref Decimal previousAmount1, ref Decimal currentAmount1)
        {
            previousAmount = 0;
            currentAmount = contractMaintenanceVO.BaseAnnualAmount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

            //If charging frequency is not Ad-hoc or Credit
            if (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                && contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                if (!contractMaintenanceVO.FirstPeriodStartDate.HasValue)
                {
                    currentAmount1 = (contractMaintenanceVO.BaseAnnualAmount / 365);
                }
                else
                {
                    currentAmount1 = (contractMaintenanceVO.BaseAnnualAmount / 365) *
                        (contractMaintenanceVO.FirstRenewalDate.Value - contractMaintenanceVO.FirstPeriodStartDate.Value).Days;
                }
                previousAmount1 = currentAmount1;
            }
        }

        /// <summary>
        /// Set start amount for the milestone invoice date.
        /// </summary>
        /// <param name="contractMaintenanceVO">The contract maintenance VO</param>
        /// <param name="inflationIndexVO">The inflation index VO</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal end date</param>
        /// <param name="previousAmount">The previous amount</param>
        /// <param name="currentAmount">The current amount</param>
        private void SetNextAmount(ContractMaintenanceVO contractMaintenanceVO, bool isUseIndex,
            ref DateTime? currentDate, ref DateTime? renewalEndDate, ref Decimal previousAmount, ref Decimal currentAmount)
        {
            previousAmount = currentAmount;

            //Calculate the amount after apply uplift index
            currentAmount = isUseIndex
                                ? currentAmount +
                                  Math.Round(
                                      (currentAmount *
                                       GetUpliftIndexRate(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value)),
                                      2)
                                : currentAmount +
                                  Math.Round(
                                      (currentAmount *
                                       GetUplift(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value)), 2);
        }

        /// <summary>
        /// This method is used to get final end date after 3 years based on period frequency.
        /// </summary>
        /// <param name="contractMaintenanceVO">The ContractMaintenanceVO</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal date</param>
        /// <returns>The uplift value</returns>
        private Decimal GetUpliftIndexRate(ContractMaintenanceVO contractMaintenanceVO, DateTime currentDate, DateTime renewalEndDate)
        {
            Decimal? indexRate = null;
            Decimal getUpliftIndexRate = 0;
            upliftFinal = 0;
            iLoop = iLoop + 1;

            //Do processing only if uplift require is true
            if (contractMaintenanceVO.UpliftRequired.HasValue && contractMaintenanceVO.UpliftRequired.Value == true)
            {
                //Decimal upliftFinal = 0;
                DateTime currentUpliftDate;

                //Uplift is not required or there is no uplift date, in other words do not uplift
                if (!contractMaintenanceVO.FirstAnnualUpliftDate.HasValue)
                {
                    uplift = previousUplift;
                    return getUpliftIndexRate;
                }

                previousUplift = uplift;
                uplift = 0;

                currentUpliftDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate, renewalEndDate);
                //upliftDateFrom = currentUpliftDate.AddDays(-1);
                upliftDateFrom = currentUpliftDate.AddDays(-(currentUpliftDate.Day - 1));
                upliftDateTo = upliftDateFrom.AddMonths(1).AddDays(-1);
                InflationIndexRateVO inflationIndexRateVO = null;

                //If inflation index is available
                //else use old index rate
                if (contractMaintenanceVO.InflationIndexId.HasValue)
                {
                    InflationIndexRateDAL inflationIndexRateDal = new InflationIndexRateDAL();
                    inflationIndexRateVO = inflationIndexRateDal.GetInflationIndexRateById(contractMaintenanceVO.InflationIndexId.Value,
                                            upliftDateFrom, upliftDateTo);
                    if (inflationIndexRateVO != null)
                    {
                        indexRate = inflationIndexRateVO.IndexRate.HasValue ? inflationIndexRateVO.IndexRate : null;
                    }
                }

                //If index rate is not available
                if (!indexRate.HasValue)
                {
                    upliftId = 0;
                    uplift = 0;
                    upliftCount = upliftCount + 1;
                    upliftCountDivisor = upliftCount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

                    if (currentDate >= contractMaintenanceVO.FirstAnnualUpliftDate.Value)
                    {
                        //Apply uplift once in a year only
                        if ((upliftCountDivisor - (int)upliftCountDivisor) == 0)
                        {
                            if (contractMaintenanceVO.InflationFixedAdditional == null)
                            {
                                contractMaintenanceVO.InflationFixedAdditional = 0;
                            }
                            //upliftFixed = contractMaintenanceVO.InflationFixedAdditional.Value;
                            //getUpliftIndexRate = getUpliftIndexRate + contractMaintenanceVO.InflationFixedAdditional.Value;
                            upliftFixed = contractMaintenanceVO.InflationFixedAdditional.HasValue ? contractMaintenanceVO.InflationFixedAdditional.Value : 0;
                            getUpliftIndexRate = getUpliftIndexRate + upliftFixed;
                            upliftFinal = (uplift + upliftFixed); // MDB-209
                        }
                    }
                }
                else
                {
                    if (iLoop == 1)
                    {
                        iCurrentIndex = indexRate.Value;
                        iPreIndex = iCurrentIndex;
                        dPercentage = 0;

                        upliftReturn = Math.Round(dPercentage, 3);
                        uplift = upliftReturn;

                        upliftCount = upliftCount + 1;
                        upliftCountDivisor = upliftCount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

                        if (currentDate >= contractMaintenanceVO.FirstAnnualUpliftDate.Value)
                        {
                            //Apply uplift once in a year only
                            if ((upliftCountDivisor - (int)upliftCountDivisor) == 0)
                            {
                                upliftFixed = contractMaintenanceVO.InflationFixedAdditional.HasValue ? contractMaintenanceVO.InflationFixedAdditional.Value : 0;
                                getUpliftIndexRate = upliftFinal = (uplift + upliftFixed);
                            }
                        }
                    }
                    else if (iLoop > 1)
                    {
                        if (currentDate < contractMaintenanceVO.FirstAnnualUpliftDate.Value)
                        {
                            iCurrentIndex = indexRate.Value;
                            dPercentage = 0;

                            upliftReturn = Math.Round(dPercentage, 3);
                            uplift = upliftReturn;

                            upliftCount = upliftCount + 1;
                            upliftCountDivisor = upliftCount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

                            //Apply uplift once in a year only
                            if ((upliftCountDivisor - (int)upliftCountDivisor) == 0)
                            {
                                //applyUplift = true;
                            }
                        }
                        if (currentDate >= contractMaintenanceVO.FirstAnnualUpliftDate.Value)
                        {
                            upliftCount = upliftCount + 1;
                            upliftCountDivisor = upliftCount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

                            //Apply uplift once in a year only
                            if ((upliftCountDivisor - (int)upliftCountDivisor) == 0)
                            {
                                iCurrentIndex = indexRate.Value;
                                if (iPreIndex == 0)
                                {
                                    dPercentage = inflationIndexRateVO.IndexRatePerAnnum.HasValue ? inflationIndexRateVO.IndexRatePerAnnum.Value : 0;
                                    iPreIndex = iCurrentIndex;
                                }

                                if (iCurrentIndex > iPreIndex)
                                {
                                    dPercentage = ((iCurrentIndex / iPreIndex) * 100 - 100) / 100;
                                    iPreIndex = iCurrentIndex;
                                }
                                else
                                {
                                    iCurrentIndex = iPreIndex;
                                    dPercentage = 0;
                                }

                                upliftReturn = Math.Round(dPercentage, 3);

                                //If index rate is not available
                                if (inflationIndexRateVO != null && !inflationIndexRateVO.IndexRate.HasValue)
                                {
                                    upliftId = 0;
                                    uplift = 0;
                                }
                                else
                                {
                                    upliftId = inflationIndexRateVO.InflationIndexRateId;
                                    uplift = upliftReturn;
                                }

                                upliftFixed = contractMaintenanceVO.InflationFixedAdditional.HasValue ? contractMaintenanceVO.InflationFixedAdditional.Value : 0;
                                getUpliftIndexRate = upliftFinal = (uplift + upliftFixed);
                            }
                        }
                    }
                }
            }

            return getUpliftIndexRate;
        }

        /// <summary>
        /// This method is used to get final end date after 3 years based on period frequency.
        /// </summary>
        /// <param name="contractMaintenanceVO">The ContractMaintenanceVO</param>
        /// <param name="currentDate">The current date</param>
        /// <param name="renewalEndDate">The renewal date</param>
        /// <returns>The uplift value</returns>
        private Decimal GetUplift(ContractMaintenanceVO contractMaintenanceVO, DateTime currentDate, DateTime renewalEndDate)
        {
            Decimal getUplift = 0;

            //Do processing only if uplift require is true
            if (contractMaintenanceVO.UpliftRequired.HasValue && contractMaintenanceVO.UpliftRequired.Value)
            {
                //  Decimal upliftFinal = 0;

                //Uplift is not required or there is no uplift date, in other words do not uplift
                if (!contractMaintenanceVO.FirstAnnualUpliftDate.HasValue)
                {
                    uplift = previousUplift;
                    return getUplift;
                }

                //  previousUplift = uplift;
                uplift = 0;

                if (currentDate >= contractMaintenanceVO.FirstAnnualUpliftDate.Value)
                {
                    upliftCount = upliftCount + 1;
                    upliftCountDivisor = upliftCount / GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId);

                    //Apply uplift once in a year only
                    if ((upliftCountDivisor - (int)upliftCountDivisor) == 0)
                    {
                        DateTime currentUpliftDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate, renewalEndDate);

                        //upliftDateFrom = currentUpliftDate.AddDays(-1); - 
                        upliftDateFrom = currentUpliftDate.AddDays(-(currentUpliftDate.Day - 1));
                        upliftDateTo = upliftDateFrom.AddMonths(1).AddDays(-1);

                        //If inflation index is available, get index rate per annum, else use last years
                        if (contractMaintenanceVO.InflationIndexId.HasValue)
                        {
                            InflationIndexRateDAL inflationIndexRateDal = new InflationIndexRateDAL();
                            //InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();

                            //InflationIndexRateVO inflationIndexRateVO =
                            //    inflationIndexRateService.GetInflationIndexRateById(contractMaintenanceVO.InflationIndexId.Value,
                            // upliftDateFrom, upliftDateTo);
                            InflationIndexRateVO inflationIndexRateVO = inflationIndexRateDal.GetInflationIndexRateById(
                                contractMaintenanceVO.InflationIndexId.Value,
                                upliftDateFrom, upliftDateTo);

                            if (inflationIndexRateVO != null)
                            {
                                //Khushboo
                                upliftId = inflationIndexRateVO.InflationIndexId;
                                //upliftId = inflationIndexRateVO.InflationIndexRateId;
                                getUplift = inflationIndexRateVO.IndexRatePerAnnum.HasValue ? inflationIndexRateVO.IndexRatePerAnnum.Value / 100 : 0;
                                previousUplift = getUplift;
                            }
                            else
                            {
                                //getUplift = previousUplift;
                                getUplift = 0;
                                upliftId = 0;
                            }
                        }

                        uplift = getUplift;
                        if (contractMaintenanceVO.InflationFixedAdditional.HasValue)
                        {
                            upliftFixed = contractMaintenanceVO.InflationFixedAdditional.Value;
                            getUplift = getUplift + contractMaintenanceVO.InflationFixedAdditional.Value;
                        }
                        else
                        {
                            upliftFixed = 0;
                        }
                    }
                }
            }

            return upliftFinal = getUplift;
        }

        /// <summary>
        /// Get Date after applying invoice in arrears.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <param name="currentDate"></param>
        /// <param name="renewalEndDate"></param>
        /// <returns></returns>
        private DateTime GetDateForAdvancedArrears(ContractMaintenanceVO contractMaintenanceVO, DateTime currentDate, DateTime renewalEndDate)
        {
            DateTime dateForAdvanceArrears = currentDate;
            switch (contractMaintenanceVO.InvoiceAdvancedArrears)
            {
                case 0:
                    dateForAdvanceArrears = currentDate;
                    break;

                case 1:
                    dateForAdvanceArrears = currentDate.AddMonths(-contractMaintenanceVO.InvoiceAdvancedId);
                    break;

                case 2:
                    dateForAdvanceArrears = renewalEndDate.AddMonths(contractMaintenanceVO.InvoiceAdvancedId);
                    break;
            }
            return dateForAdvanceArrears;
        }

        /// <summary>
        /// This method is used to get final end date after 3 years based on period frequency.
        /// </summary>
        /// <param name="contractMaintenanceVO">The ContractMaintenanceVO</param>
        /// <param name="dateToAdd">The current date</param>
        /// <param name="periodToAdd">The period to add in current date to get last final date after 3 years</param>
        private DateTime PeriodAdd(ContractMaintenanceVO contractMaintenanceVO, DateTime dateToAdd, int periodToAdd)
        {
            DateTime periodAddedDate = DateTime.Now;

            switch (contractMaintenanceVO.PeriodFrequencyId)
            {
                case 4:
                    periodAddedDate = dateToAdd.AddYears(periodToAdd);
                    break;

                case 3:
                    periodAddedDate = dateToAdd.AddMonths(periodToAdd * 6);
                    break;

                case 2:
                    periodAddedDate = dateToAdd.AddMonths(periodToAdd * 3);
                    break;

                case 7:
                    periodAddedDate = dateToAdd.AddMonths(periodToAdd * 2);
                    break;

                case 1:
                    periodAddedDate = dateToAdd.AddMonths(periodToAdd);
                    break;
            }
            return periodAddedDate;
        }

        /// <summary>
        /// Get months based on frequency 
        /// </summary>
        /// <param name="periodFrequency"></param>
        /// <returns></returns>
        public int GetFrequencyMultiple(int periodFrequency)
        {
            int frequencyMultiple = 1;

            if (periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.YEARLY)
                || periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                || periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                frequencyMultiple = 1;
            }
            else if (periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
            {
                frequencyMultiple = 2;
            }
            else if (periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
            {
                frequencyMultiple = 4;
            }
            else if (periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
            {
                frequencyMultiple = 6;
            }
            else if (periodFrequency == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
            {
                frequencyMultiple = 12;
            }
            return frequencyMultiple;
        }

        /// <summary>
        /// Set milestone Uplift details to none for the first milestone.
        /// </summary>
        /// <param name="milestoneVO"></param>
        /// <returns></returns>
        private MilestoneVO SetUpliftToZeroForFirstMilestone(MilestoneVO milestoneVO)
        {
            //For the first milestone it should be zero
            milestoneVO.Uplift = 0;
            milestoneVO.UpliftFixedRate = 0;
            milestoneVO.ChargingUpliftID = null;
            milestoneVO.UpliftRate = 0;
            return milestoneVO;
        }

        #endregion Milestone caluclation

        #region Billing Line Text Calculation

        /// <summary>
        /// Calculate the billing line text for the milestone based on the maintenance billing line.
        /// </summary>
        private void CalculateBillingLineText(MilestoneVO milestoneVO, ContractMaintenanceVO contractMaintenanceVO)
        {
            int linecount = 0;

            List<MaintenanceBillingLineVO> maintenanceBillingLineVos = contractMaintenanceVO.MaintenanceBillingLineVos;

            if (contractMaintenanceVO.IsGrouped == true && contractMaintenanceVO.GroupId != null &&
                contractMaintenanceVO.IsDefaultLineInGroup == false)
            {
                maintenanceBillingLineVos = GetDefaultLinesFromContractMaintenanceGroup(
                    contractMaintenanceVO.ContractId, contractMaintenanceVO.PeriodFrequencyId,
                    (int) contractMaintenanceVO.GroupId);
            }

            if (maintenanceBillingLineVos != null)
            {
                //Loop though all billing details billing lines
                foreach (MaintenanceBillingLineVO maintenanceBillingLine in maintenanceBillingLineVos)
                {
                    //Edit milestone billing line
                    if (linecount < milestoneVO.MilestoneBillingLineVos.Count)
                    {
                        // Calculate billing line for Ready For Calculating Status
                        if (milestoneVO.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                        {
                            //Models.MilestoneBillingLine milestoneBillingLine = milestoneVO.MilestoneBillingLines[linecount];
                            //milestoneBillingLine.LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText);
                            //milestoneBillingLine.LineSequance = linecount;
                            MilestoneBillingLineVO milestoneBillingLineVO = milestoneVO.MilestoneBillingLineVos[linecount];
                            milestoneBillingLineVO.LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText);
                            milestoneBillingLineVO.LineSequance = linecount;
                        }
                    }
                    else
                    {
                        //Add milestone billing line
                        if (maintenanceBillingLineVos.Count > linecount)
                        {
                            // Calculate billing line for Ready For Calculating Status
                            if (milestoneVO.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                            {

                                milestoneVO.MilestoneBillingLineVos.Add(new MilestoneBillingLineVO()
                                {
                                    LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText),
                                    LineSequance = linecount,
                                    ContractId = contractMaintenanceVO.ContractId,
                                });
                            }
                        }
                    }

                    linecount++;
                    //}
                }

                // Find unique line by line sequence number 
                List<MilestoneBillingLineVO> milestoneBillingLines =
                    milestoneVO.MilestoneBillingLineVos.GroupBy(l => l.LineSequance).Select(g => g.First()).ToList();

                // find duplicate line sequence number
                List<MilestoneBillingLineVO> deleteMilestoneLines =
                    milestoneVO.MilestoneBillingLineVos.Except(milestoneBillingLines).ToList();

                // Delete records which has duplicate line sequence number
                foreach (var milestoneBillingLine in deleteMilestoneLines)
                {

                    MilestoneBillingLineVO milestone = milestoneVO.MilestoneBillingLineVos.Find(
                        temp => temp.ID == milestoneBillingLine.ID
                        );

                    milestone.IsDeleted = true;
                }
            }
        }

        /// <summary>
        /// Creates billing line text by replacing the billing line tags
        /// </summary>
        /// <param name="maintenanceBillingLineText">The maintenance billing line text</param>
        /// <returns>The billing line text after replacing the tags</returns>
        private String CreateBillingLineText(MilestoneVO milestoneVO, string maintenanceBillingLineText)
        {
            int pos, endpos = 0, prevpos = 0;
            string tempString = string.Empty, fieldName;
            pos = 0;

            //If billing line text is empty then return
            if (String.IsNullOrEmpty(maintenanceBillingLineText))
            {
                return null;
            }


            ////Validate and create belling line
            //if (ValidateBillingLine(maintenanceBillingLineText))
            //{
            while (pos >= 0)
            {
                if (endpos > pos)
                {
                    pos = endpos + 1;
                }
                prevpos = pos;

                if (pos >= maintenanceBillingLineText.Length)
                {
                    pos = -1;
                    break;
                }
                else
                {
                    pos = maintenanceBillingLineText.Contains("[") ? maintenanceBillingLineText.IndexOf('[', pos) : -1;
                }

                if (pos != -1)
                {
                    tempString = tempString + maintenanceBillingLineText.Substring(prevpos, (pos - prevpos));
                    endpos = maintenanceBillingLineText.IndexOf(']', pos + 1);
                    fieldName = maintenanceBillingLineText.Substring(pos + 1, endpos - pos - 1);
                    tempString = tempString + GetBillingLineFieldValue(milestoneVO, fieldName);
                }
                else
                {
                    tempString = tempString +
                                 maintenanceBillingLineText.Substring(prevpos, maintenanceBillingLineText.Length - prevpos);
                }
            }
            //}
            //else
            //{
            //    tempString = maintenanceBillingLineText;
            //}

            //If text length is more then 48 characters then show till 48 characters followed by * sign
            if (tempString.Length > 48)
            {
                tempString = tempString.Substring(0, 47) + "*";
            }
            return tempString;
        }

        /// <summary>
        /// Validates the maintenance billing line text
        /// </summary>
        /// <param name="maintenanceBillingLineText">The maintenance billing line text</param>
        /// <returns>True if valid, false otherwise</returns>
        private bool ValidateBillingLine(string maintenanceBillingLineText)
        {
            bool isValid = false;
            int pos, endPosition, startPosition, openBracketCount, closeBracketCount;
            string tempString;

            pos = openBracketCount = closeBracketCount = 0;
            tempString = maintenanceBillingLineText;

            endPosition = !String.IsNullOrEmpty(tempString) ? tempString.IndexOf(']', 0) : -1;
            startPosition = !String.IsNullOrEmpty(tempString) ? tempString.IndexOf('[', 0) : -1;
            if (endPosition < startPosition)
            {
                return false;
            }

            //validate opening brackets
            while (pos >= 0)
            {
                pos = !String.IsNullOrEmpty(tempString) ? tempString.IndexOf('[', 0) : -1;
                //pos = tempString.IndexOf('[', 0);
                if (pos != -1)
                {
                    tempString = tempString.Substring(pos + 1, (tempString.Length - (pos + 1)));
                    //tempString = tempString.Substring(pos + 1, (tempString.Length - pos));
                    openBracketCount = openBracketCount + 1;
                }
            }

            pos = 0;
            tempString = maintenanceBillingLineText;

            //validate closing brackets
            while (pos >= 0)
            {
                pos = !String.IsNullOrEmpty(tempString) ? tempString.IndexOf(']', 0) : -1;
                if (pos != -1)
                {
                    tempString = tempString.Substring(pos + 1, (tempString.Length - (pos + 1)));
                    closeBracketCount = closeBracketCount + 1;
                }
            }

            //Check if count of opening and closing brackets are same or not
            if (openBracketCount == closeBracketCount)
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Gets billing line field value for the matched field name
        /// </summary>
        /// <param name="milestoneVO">The milestone Value object</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>The billing line field value</returns>
        private string GetBillingLineFieldValue(MilestoneVO milestoneVO, string fieldName)
        {
            string fieldValue = fieldName;
            switch (fieldName.ToUpper())
            {
                case "CLCRID": milestoneVO.ID.ToString(); break;
                case "TOTALUPLIFT": fieldValue = milestoneVO.Uplift.HasValue ? String.Format("{0:#.0}", milestoneVO.Uplift * 100) : string.Empty; break;
                case "EXPR1": fieldValue = milestoneVO.PreviousValue.HasValue ? milestoneVO.PreviousValue.Value.ToString() : string.Empty; break;
                case "CLPrevValue": fieldValue = milestoneVO.PreviousValue.HasValue ? String.Format("{0:#,000.00}", milestoneVO.PreviousValue) : string.Empty; break;
                case "PREVIOUSVALUE": fieldValue = milestoneVO.PreviousValue.HasValue ? String.Format("{0:#,000.00}", milestoneVO.PreviousValue) : string.Empty; break;
                case "RENEWAL_START_DATE": fieldValue = milestoneVO.RenewalStartDate.HasValue ? milestoneVO.RenewalStartDate.Value.ToString(Constants.DATE_FORMAT) : string.Empty; break;
                case "RENEWAL_END_DATE": fieldValue = milestoneVO.RenewalEndDate.HasValue ? milestoneVO.RenewalEndDate.Value.ToString(Constants.DATE_FORMAT) : string.Empty; break;
                case "MILESTONE_LN_STATUS": fieldValue = milestoneVO.MilestoneStatusName; break;
                case "ESTIMATED_COMP_DT": fieldValue = milestoneVO.InvoiceDate.ToShortDateString(); break;
                case "PERCENTAGE": fieldValue = Convert.ToString(milestoneVO.Percentage); break;
                case "CLAMOUNT": fieldValue = Convert.ToString(milestoneVO.Amount); break;
                case "CLUPLIFTRATEP1": fieldValue = milestoneVO.UpliftRate.HasValue ? Math.Round(milestoneVO.UpliftRate.Value * 100, 1).ToString() : string.Empty; break;
                case "CLUPLIFTRATEP2": fieldValue = milestoneVO.UpliftRate.HasValue ? Math.Round(milestoneVO.UpliftRate.Value * 100, 2).ToString() : string.Empty; break;
                case "CLUPLIFTRATE": fieldValue = milestoneVO.UpliftRate.HasValue ? milestoneVO.UpliftRate.Value.ToString() : string.Empty; break;
                case "CLUPLIFTFIXEDRATEP2": fieldValue = milestoneVO.UpliftFixedRate.HasValue ? Math.Round(milestoneVO.UpliftFixedRate.Value * 100, 2).ToString() : string.Empty; break;
                case "CLUPLIFTFIXEDRATE": fieldValue = milestoneVO.UpliftFixedRate.HasValue ? milestoneVO.UpliftFixedRate.Value.ToString() : string.Empty; break;
                case "CLGROSSANNUALUPLIFTP1": fieldValue = Math.Round(((milestoneVO.UpliftRate.HasValue ? milestoneVO.UpliftRate.Value : 0) +
                                                                        (milestoneVO.UpliftFixedRate.HasValue ? milestoneVO.UpliftFixedRate.Value : 0)) * 100, 1).ToString(); break;

                case "CLUPLIFTP1": fieldValue = milestoneVO.Uplift.HasValue ? Math.Round(milestoneVO.Uplift.Value * 100, 1).ToString() : string.Empty; break;
                case "CLUPLIFT": fieldValue = milestoneVO.Uplift.HasValue ? milestoneVO.Uplift.Value.ToString() : string.Empty; break;
                //case "CLQUOTEDATE": break;
            }
            return fieldValue;
        }

        #endregion Billing Line Text Calculation

        /// <summary>
        /// Gets fully qualified path of the log file
        /// </summary>
        private string GetLogPath(string logPath)
        {
            string filePath = string.Empty;

            if (!String.IsNullOrWhiteSpace(logPath))
            {
                bool IsExists = Directory.Exists((logPath));

                if (!IsExists)
                    Directory.CreateDirectory(logPath);
            }
            else
            {
                logPath = "C:\\MDB\\RecalculateMilestone";
                Directory.CreateDirectory(logPath);
            }

            filePath = logPath + "\\Recalcualtion On " + string.Format("{0:dd-MMM-yyyy hh-mm-ss}", DateTime.Now) + ".csv";

            return filePath;
        }

        /// <summary>
        /// Write contract details for recalculated milestones
        /// </summary>
        /// <param name="contractMaintenanceVoList"></param>
        private void WriteToCSV(List<ContractMaintenanceVO> contractMaintenanceVoList)
        {
            string logFile = string.Empty;  //ApplicationConfiguration.GetRecalculationLog();
            string fileName = GetLogPath(logFile);

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                StringBuilder builder = new StringBuilder();

                streamWriter.WriteLine("Company Name, Contract Number, Activity Name, Account Code, Cost Centre");

                foreach (var item in contractMaintenanceVoList)
                {
                    builder.Append(GenerateRow(item));
                    streamWriter.WriteLine(builder);
                    builder.Clear();
                }
            }
        }

        /// <summary>
        /// Generate row to write in csv file.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <returns></returns>
        private StringBuilder GenerateRow(ContractMaintenanceVO contractMaintenanceVO)
        {
            StringBuilder row = new StringBuilder();

            row.Append(CheckString(contractMaintenanceVO.CompanyName));
            row.Append(',');
            row.Append(CheckString(contractMaintenanceVO.ContractNumber));
            row.Append(',');
            row.Append(CheckString(contractMaintenanceVO.ActivityCode));
            row.Append(',');
            row.Append(CheckString(contractMaintenanceVO.Account));
            row.Append(',');
            row.Append(CheckString(contractMaintenanceVO.CostCenter));

            return row;
        }

        /// <summary>
        /// Validate string has comma value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private StringBuilder CheckString(string value)
        {
            StringBuilder row = new StringBuilder();
            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                {
                    row.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                }
                else
                {
                    row.Append(value);
                }
            }
            return row;
        }

        /// <summary>
        /// Gets the List of Recalculation requests
        /// </summary>
        /// <returns>Recalculation Request List</returns>
        public List<RecalculationVO> GetRecalculationRequestList()
        {
            return inflationIndexDAL.GetRecalculationRequestList();
        }


        /// <summary>
        /// Returns the current date and time
        /// with proper format
        /// </summary>
        /// <returns>Current date and time</returns>
        private static string GetCurrentDateTime()
        {
            return String.Format("{0:dd-MM-yyyy_HH-mm-ss}", DateTime.Now);
        }

        /// <summary>
        /// Write the log for Recalculation process
        /// </summary>
        /// <param name="error"></param>
        /// <param name="logFilePath"></param>
        private static string WriteLogForRecalculation(List<string> error)
        {
            string logFilePath = string.Empty;
            StreamWriter writer = null;
            try
            {
                if (string.IsNullOrEmpty(logFilePath))
                {
                    logFilePath = @"C:\ARBS\RecalculationLog" ;
                }

                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                //Set the file path having dd-MM-yy HH.mm.ss format in file name                
                logFilePath += "\\Recalculation_" + GetCurrentDateTime() + ".txt";

                //Write data to file
                //writer = new StreamWriter(logFilePath, false);
                writer =  new StreamWriter(logFilePath);
                for (int i = 0; i < error.Count; i++)
                {
                    //string message = GetCurrentDateTime() + " : " + error[i]; // +Environment.NewLine;   
                    writer.WriteLine(error[i]);
                    writer.WriteLine("-----------------------------------------------------------------------------------------");
                }
                return logFilePath;
            }
            finally
            {
                //Cleanup buffers and
                //close the writer
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

    }
}
