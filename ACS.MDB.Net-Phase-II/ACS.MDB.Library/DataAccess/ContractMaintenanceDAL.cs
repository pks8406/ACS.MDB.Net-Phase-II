using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ContractMaintenanceDAL : BaseDAL
    {
        /// <summary>
        /// Get contract maintenance details by Id
        /// </summary>
        /// <param name="contractMaintenanceId">contract maintenanceId id</param>
        /// <returns>Contract Maintenace Details</returns>
        public ContractMaintenanceVO GetContractMaintenanceById(int contractMaintenanceId)
        {
            ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == contractMaintenanceId);

            ContractMaintenanceVO contractMaintenanceVO = null;
            if (contractMaintenance != null)
            {
                contractMaintenanceVO = new ContractMaintenanceVO(contractMaintenance,null);
            }

            //if (contractMaintenance.Milestones.Count > 0)
            //{
            //    if (contractMaintenanceVO.FirstPeriodStartDate != null && contractMaintenanceVO.FirstRenewalDate != null)
            //    {
            //        contractMaintenanceVO.checkValue = true;
            //    }
            //}
            return contractMaintenanceVO;
        }

        /// <summary>
        /// Gets the list of contract Maintenance
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceListbyContractId(int contractId)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();

            List<ContractMaintenance> contractMaintenances =
                mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId &&
                    c.IsDeleted == false).ToList();

            List<LINQ.AuditReason> AuditReasonList = mdbDataContext.AuditReasons.ToList();

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item, AuditReasonList));
            }

            //Sort billing detail grid by item is isGrouped or not and group name 
            contractMaintenancesVOList = contractMaintenancesVOList.OrderBy(c => c.IsGrouped).ThenBy(c => c.GroupName).ToList();
            return contractMaintenancesVOList;
        }

        /// <summary>
        /// Gets the Contract maintenance list by contract line id.
        /// </summary>
        /// <param name="contractLineId">Contract Line id</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceListbyContractLineId(int contractLineId)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();

            List<ContractMaintenance> contractMaintenances =
                mdbDataContext.ContractMaintenances.Where(c => c.ContractLineID == contractLineId &&
                    c.IsDeleted == false).ToList();

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            return contractMaintenancesVOList;
        }

        /// <summary>
        /// Save contact maintenance details
        /// </summary>
        /// <param name="contractMaintenanceVO">Value Object Contract Maintenance</param>
        public void SaveContractMaintenance(ContractMaintenanceVO contractMaintenanceVO)
        {
            if (contractMaintenanceVO != null)
            {
                if (contractMaintenanceVO.ID <= 0)
                {
                    //Add new contract maintenance
                    ContractMaintenance contractMaintenance = new ContractMaintenance();
                    // contractMaintenance.ContractLine = new ContractLine();
                    //  contractMaintenance.ContractLine.ID = contractMaintenanceVO.ContractLineId;

                    contractMaintenance.ContractID = contractMaintenanceVO.ContractId;
                    contractMaintenance.ContractLineID = contractMaintenanceVO.ContractLineId;
                    contractMaintenance.ChargeFrequencyID = contractMaintenanceVO.PeriodFrequencyId;
                    //  contractMaintenance.ContractLine.ActivityCode = contractMaintenanceVO.ActivityCodeId;

                    contractMaintenance.ProductID = contractMaintenanceVO.ProductId;

                    contractMaintenance.SubProductID = contractMaintenanceVO.SubProductId;

                    if (contractMaintenanceVO.InflationIndexId > 0)
                    {
                        contractMaintenance.InflationIndexID = contractMaintenanceVO.InflationIndexId;
                    }
                    contractMaintenance.InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
                    contractMaintenance.ReasonCode = contractMaintenanceVO.ReasonId;
                    contractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;
                    contractMaintenance.BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
                    contractMaintenance.FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
                    contractMaintenance.FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate;
                    contractMaintenance.FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate;
                    contractMaintenance.FirstAnnualUpliftDate = contractMaintenanceVO.FirstAnnualUpliftDate;
                    contractMaintenance.FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate;
                    contractMaintenance.FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
                    contractMaintenance.EndAmount = contractMaintenanceVO.EndAmount;
                    //contractMaintenance.ContractLine.OAActivityCode.ActivityCode = contractMaintenanceVO.ActivityCode;

                    contractMaintenance.QTY = contractMaintenanceVO.QTY;
                    contractMaintenance.CreationDate = contractMaintenanceVO.CreationDate;
                    contractMaintenance.DeleteDate = contractMaintenanceVO.DeleteDate;
                    contractMaintenance.ReasonDate = contractMaintenanceVO.ReasonDate;
                    contractMaintenance.InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;
                    //contractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;

                    contractMaintenance.UpliftRequired = contractMaintenanceVO.UpliftRequired;
                    contractMaintenance.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional;
                    //contractMaintenance.TerminationReason = contractMaintenanceVO.TerminationReason;
                    contractMaintenance.DeleteReason = contractMaintenanceVO.DeleteReason;

                    //ARBS-137-Add ForecastBillingStartDate 
                    contractMaintenance.ForecastBillingStartDate = contractMaintenanceVO.ForecastBillingStartDate;

                    //Added fields for grouping of billing lines
                    contractMaintenance.GroupId = null;
                    contractMaintenance.GroupName = "Ungroup";
                    contractMaintenance.IsGrouped = null;
                    contractMaintenance.IsDefaultLineInGroup = null;
                    contractMaintenance.DocumentTypeID = contractMaintenanceVO.DocumentTypeId;

                    contractMaintenance.Comment = !string.IsNullOrEmpty(contractMaintenanceVO.Comment) ? contractMaintenanceVO.Comment.Trim() : contractMaintenanceVO.Comment;
                    contractMaintenance.CreatedBy = contractMaintenanceVO.CreatedByUserId;
                    contractMaintenance.CreatedDate = DateTime.Now;

                    //Save contract maintenance lines
                    contractMaintenance.MaintenanceBillingLines = new System.Data.Linq.EntitySet<MaintenanceBillingLine>();
                    foreach (var item in contractMaintenanceVO.MaintenanceBillingLineVos)
                    {
                        contractMaintenance.MaintenanceBillingLines.Add(new MaintenanceBillingLine() { LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText, LineSequance = item.LineSequance, CreatedBy = contractMaintenanceVO.CreatedByUserId, CreationDate = DateTime.Now });
                    }

                    mdbDataContext.ContractMaintenances.InsertOnSubmit(contractMaintenance);
                    mdbDataContext.SubmitChanges();

                    //Added newly created billing line Id into contractMaintenance Value object
                    contractMaintenanceVO.ID = contractMaintenance.ID;                    
                }
                else
                {
                    //Update contract maintenance details
                    ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.Where(c => c.ID == contractMaintenanceVO.ID && c.IsDeleted == false).SingleOrDefault();

                    ///This Code is Commented because now we are not allowing to change charging frequency in edit mode,
                    ///if milestones already generated for selected maintenance line.

                    ////If charging frequency is adhoc or credit
                    //if ((contractMaintenance.ChargeFrequencyID == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) ||( contractMaintenance.ChargeFrequencyID == Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
                    //{
                    //    if (contractMaintenance.FinalRenewalStartDate != contractMaintenanceVO.FinalRenewalStartDate)
                    //    {
                    //        DeleteMilestones(contractMaintenanceVO);
                    //    }
                    //}
                    //else 
                    //{
                    //    //If charging frequency is not adhoc or credit
                    //    if (contractMaintenance.FirstRenewalDate != contractMaintenanceVO.FirstRenewalDate)
                    //    {
                    //        DeleteMilestones(contractMaintenanceVO);
                    //    }
                    //}

                    contractMaintenance.ContractID = contractMaintenanceVO.ContractId;
                    contractMaintenance.ContractLineID = contractMaintenanceVO.ContractLineId;
                    contractMaintenance.ChargeFrequencyID = contractMaintenanceVO.PeriodFrequencyId;
                    //contractMaintenance.ContractLine.ActivityCodeID = contractMaintenanceVO.ActivityCodeId;

                    contractMaintenance.ProductID = contractMaintenanceVO.ProductId;

                    contractMaintenance.SubProductID = contractMaintenanceVO.SubProductId;

                    contractMaintenance.InflationIndexID = contractMaintenanceVO.InflationIndexId;
                    contractMaintenance.InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
                    contractMaintenance.ReasonCode = contractMaintenanceVO.ReasonId;

                    contractMaintenance.BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
                    contractMaintenance.FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
                    contractMaintenance.FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate;
                    contractMaintenance.FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate;
                    contractMaintenance.FirstAnnualUpliftDate = contractMaintenanceVO.FirstAnnualUpliftDate;
                    contractMaintenance.FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate;
                    contractMaintenance.FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
                    contractMaintenance.EndAmount = contractMaintenanceVO.EndAmount;
                    //contractMaintenance.ContractLine.OAActivityCode.ActivityCode = contractMaintenanceVO.ActivityCode;

                    contractMaintenance.QTY = contractMaintenanceVO.QTY;
                    contractMaintenance.CreationDate = contractMaintenanceVO.CreationDate;
                    contractMaintenance.DeleteDate = contractMaintenanceVO.DeleteDate;
                    contractMaintenance.ReasonDate = contractMaintenanceVO.ReasonDate;
                    contractMaintenance.InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;
                    contractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;

                    contractMaintenance.UpliftRequired = contractMaintenanceVO.UpliftRequired;
                    contractMaintenance.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional;
                    //contractMaintenance.TerminationReason = contractMaintenanceVO.TerminationReason;
                    contractMaintenance.DeleteReason = contractMaintenanceVO.DeleteReason;

                    //ARBS-137-Add ForecastBillingStartDate 
                    contractMaintenance.ForecastBillingStartDate = contractMaintenanceVO.ForecastBillingStartDate;
                    contractMaintenance.Comment = !string.IsNullOrEmpty(contractMaintenanceVO.Comment) ? contractMaintenanceVO.Comment.Trim() : contractMaintenanceVO.Comment;

                    //Added fields for grouping of billing lines
                    contractMaintenance.GroupId = contractMaintenanceVO.GroupId; ;
                    contractMaintenance.GroupName = contractMaintenanceVO.GroupName;
                    contractMaintenance.IsGrouped = contractMaintenanceVO.IsGrouped;
                    contractMaintenance.IsDefaultLineInGroup = contractMaintenanceVO.IsDefaultLineInGroup;

                    contractMaintenance.DocumentTypeID = contractMaintenanceVO.DocumentTypeId;

                    contractMaintenance.LastUpdatedBy = contractMaintenanceVO.LastUpdatedByUserId;
                    contractMaintenance.LastUpdatedDate = DateTime.Now;

                    //Save contract maintenance lines
                    foreach (var item in contractMaintenanceVO.MaintenanceBillingLineVos)
                    {
                        MaintenanceBillingLine maintenanceBillingLine = contractMaintenance.MaintenanceBillingLines.ToList().Find(x => x.ID == item.BillingLineID && x.ID != 0);
                        if (maintenanceBillingLine != null)
                        {
                            maintenanceBillingLine.LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText;
                            maintenanceBillingLine.LineSequance = item.LineSequance;
                            maintenanceBillingLine.Description = item.Description;
                            maintenanceBillingLine.MaintenanceID = contractMaintenanceVO.ID;
                            maintenanceBillingLine.LastUpdatedBy = contractMaintenanceVO.LastUpdatedByUserId;
                            maintenanceBillingLine.LastUpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            contractMaintenance.MaintenanceBillingLines.Add(new MaintenanceBillingLine()
                            {
                                LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText,
                                LineSequance = item.LineSequance,
                                Description = item.Description,
                                MaintenanceID = contractMaintenanceVO.ID,
                                CreatedBy = contractMaintenanceVO.CreatedByUserId,
                                CreationDate = DateTime.Now
                            });
                        }
                    }

                    //Delete Milestones if Record is on Backlog
                    if (contractMaintenanceVO.IncludeInForecast != 0)
                    {
                        DeleteMilestones(contractMaintenanceVO);
                    }

                    mdbDataContext.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Delete contract maintenance and associated details
        /// </summary>
        /// <param name="Ids">Ids of contact to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteContractMaintenance(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //Delete maintenance lines
                    ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == id);
                    List<Milestone> milestones = mdbDataContext.Milestones.Where(c => c.MaintenanceID == id && c.IsDeleted == false).ToList();

                    if (milestones.Count == 0 && contractMaintenance.IsGrouped != true)
                    {
                        contractMaintenance.IsDeleted = true;
                        contractMaintenance.LastUpdatedDate = DateTime.Now;
                        contractMaintenance.LastUpdatedBy = userId;

                        ////Delete billing line tags
                        List<MaintenanceBillingLine> maintenanceBillingLines = mdbDataContext.MaintenanceBillingLines.Where(c => c.MaintenanceID == id).ToList();
                        foreach (var maintenanceBillingLine in maintenanceBillingLines)
                        {
                            maintenanceBillingLine.IsDeleted = true;
                            maintenanceBillingLine.LastUpdatedDate = DateTime.Now;
                            maintenanceBillingLine.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Add/Update/Delete milestones
        /// </summary>
        /// <param name="maintenanceId">The maintenance id</param>
        /// <param name="milestonesList">The milestone list</param>
        public void AddUpdateDeleteMilestones(int maintenanceId, List<MilestoneVO> milestonesList)
        {
            //MilestoneDAL milestoneDAL = new MilestoneDAL();
            //List<MilestoneVO> origionalMilestoneList = milestoneDAL.GetMilestoneList(maintenanceId);
            List<MilestoneVO> milestonesToAdd = milestonesList.FindAll(x => x.ID == 0);
            List<MilestoneVO> milestonesToEdit = milestonesList.FindAll(x => x.ID > 0);
            List<MilestoneVO> milestonesToDelete = milestonesList.FindAll(x => x.IsDeleted == true);

            ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == maintenanceId);
            List<Milestone> milestonesListToUpdate = contractMaintenance.Milestones.ToList().FindAll(x => x.IsDeleted == false);

            foreach (var item in milestonesToEdit)
            {
                Milestone milestone = milestonesListToUpdate.Find(x => x.ID == item.ID);

                //Update milestone
                if (milestone != null)
                {
                    milestone.ContractID = item.ContractID;
                    milestone.ContractLine = mdbDataContext.ContractLines.Single(cl => cl.ID == item.ContractLineID);  //item.ContractLineID;
                    milestone.Description = item.Description;
                    //milestone.ConditionType = "MS";
                    //milestone.ApprovedByCustomer = false;
                    milestone.IsApprovalRequired = item.IsApprovalRequired;
                    milestone.MaintenanceID = item.ContractMaintenanceID;
                    milestone.EstimatedDate = item.InvoiceDate;
                    milestone.RenewalStartDate = item.RenewalStartDate.Value;
                    milestone.RenewalEndDate = item.RenewalEndDate.Value;
                    milestone.Amount = Math.Round(item.Amount, 2);
                    milestone.MilestoneStatusID = item.MilestoneStatusID;
                    milestone.MilestoneStatus = item.MilestoneStatusName;
                    milestone.LastUpdatedBy = item.LastUpdatedByUserId;
                    milestone.LastUpdatedDate = DateTime.Now;
                    milestone.IsDeleted = item.IsDeleted;
                    milestone.Uplift = item.Uplift;
                    milestone.UpliftFixedRate = item.UpliftFixedRate;
                    milestone.ChargingUpliftID = item.ChargingUpliftID;
                    milestone.UpliftRate = item.UpliftRate;
                    milestone.IndexRate = item.IndexRate;
                    milestone.PreviousValue = item.PreviousValue.Value;

                    //Edit milestone billing lines
                    foreach (var billingLine in item.MilestoneBillingLineVos)
                    {
                        if (billingLine.LineSequance < milestone.MilestoneBillingLines.Count)
                        {
                            MilestoneBillingLine milestoneBillingLine = milestone.MilestoneBillingLines[billingLine.LineSequance];
                            milestoneBillingLine.LineSequance = billingLine.LineSequance;
                            milestoneBillingLine.LineText = billingLine.LineText;
                            milestoneBillingLine.IsDeleted = billingLine.IsDeleted;
                            milestoneBillingLine.LastUpdatedBy = item.LastUpdatedByUserId;
                            milestoneBillingLine.LastUpdatedDate = DateTime.Now;
                        }
                    }
                }
            }

            //MilestoneStatusDAL milestoneStatusDAL = new MilestoneStatusDAL();
            //int milestoneStatusID = milestoneStatusDAL.GetMilestoneStatusList().Find(x => x.Order == 1).ID;
            foreach (var item in milestonesToAdd)
            {
                //Add milestone
                Milestone milestone = new Milestone();
                milestone.ContractID = item.ContractID;
                milestone.ContractLineID = item.ContractLineID;
                milestone.Description = item.Description;
                milestone.ConditionType = "MS";
                milestone.ApprovedByCustomer = false;
                milestone.IsApprovalRequired = item.IsApprovalRequired;
                milestone.MaintenanceID = item.ContractMaintenanceID;
                milestone.EstimatedDate = item.InvoiceDate;
                milestone.RenewalStartDate = item.RenewalStartDate.Value;
                milestone.RenewalEndDate = item.RenewalEndDate.Value;
                milestone.Amount = Math.Round(item.Amount, 2);
                milestone.MilestoneStatusID = item.MilestoneStatusID;
                milestone.MilestoneStatus = item.MilestoneStatusName;
                milestone.IsDeleted = item.IsDeleted;
                milestone.ApprovedStatus = item.ApprovedStatus;
                milestone.CreatedBy = item.CreatedByUserId;
                milestone.CreationDate = DateTime.Now;
                milestone.Uplift = item.Uplift;
                milestone.UpliftFixedRate = item.UpliftFixedRate;
                milestone.ChargingUpliftID = item.ChargingUpliftID;
                milestone.UpliftRate = item.UpliftRate;
                milestone.IndexRate = item.IndexRate;
                milestone.PreviousValue = item.PreviousValue.Value;

                //Add milestone billing lines
                foreach (var milestoneBillingLine in item.MilestoneBillingLineVos)
                {
                    milestone.MilestoneBillingLines.Add(new MilestoneBillingLine()
                    {
                        LineSequance = milestoneBillingLine.LineSequance,
                        LineText = milestoneBillingLine.LineText,
                        IsDeleted = milestoneBillingLine.IsDeleted,
                        ContractID = milestoneBillingLine.ContractId,
                        CreatedBy = item.CreatedByUserId,
                        CreationDate = DateTime.Now
                    });
                }

                contractMaintenance.Milestones.Add(milestone);
            }

            //Delete milestones
            foreach (var item in milestonesToDelete)
            {
                Milestone milestone = milestonesListToUpdate.Find(x => x.ID == item.ID);
                if (milestone != null)
                {
                    milestone.IsDeleted = true;
                }
            }

            //Delete billing line texts for the deleted milestones
            foreach (var item in milestonesToDelete)
            {
                //Delete billing line tags
                List<MilestoneBillingLine> maintenanceBillingLines = mdbDataContext.MilestoneBillingLines.Where(c => c.MilestoneID == item.ID).ToList();
                foreach (var maintenanceBillingLine in maintenanceBillingLines)
                {
                    maintenanceBillingLine.IsDeleted = true;
                    maintenanceBillingLine.LastUpdatedDate = DateTime.Now;
                    maintenanceBillingLine.LastUpdatedBy = item.LastUpdatedByUserId;
                }
            }

            //Submit changes
            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Recalculate milestone
        /// </summary>
        public List<ContractMaintenanceVO> GetContractMaintenanceForRecalculate(List<int> Ids)
        {
            List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();

            List<ContractMaintenance> contractMaintenances = null;

            //Recalculate for non index milestone
            if (Ids.Count == 1 && Ids[0] == 0)
            {

                contractMaintenances = mdbDataContext.ContractMaintenances
                                                    .Where(cm => (cm.UpliftRequired == false || cm.UpliftRequired == null)
                                                        //ARBS-126
                                                        //&& cm.ChargeFrequencyID != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                                                        //&& cm.ChargeFrequencyID != Convert.ToInt32(Constants.ChargeFrequency.CREDIT)
                                                        //ARBS-145
                                                        //&& (cm.FinalRenewalEndDate == null || cm.FinalRenewalEndDate.Value >= DateTime.Now)
                                                        && !cm.IsDeleted && (cm.IncludeInForecast == 0 && cm.FirstRenewalDate != null)).ToList(); // && cm.ID >= 44920

                foreach (var contractMaintenance in contractMaintenances)
                {
                    contractMaintenance.Milestones.OrderBy(m => m.EstimatedDate);
                    contractMaintenanceVOList.Add(new ContractMaintenanceVO(contractMaintenance,null));
                }

            }
            else
            {
                foreach (var id in Ids)
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances
                                                         .Where(cm => cm.UpliftRequired == true && cm.FirstAnnualUpliftDate.HasValue
                                                             //ARBS-126
                                                             //&& cm.ChargeFrequencyID != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                                                             //&& cm.ChargeFrequencyID != Convert.ToInt32(Constants.ChargeFrequency.CREDIT)
                                                             //ARBS-145
                                                             //&& (cm.FinalRenewalEndDate == null || cm.FinalRenewalEndDate.Value >= DateTime.Now)
                                                             && cm.InflationIndexID == id && cm.IncludeInForecast == 0 && !cm.IsDeleted).ToList();  //&& cm.ID >= 43663

                    foreach (var contractMaintenance in contractMaintenances)
                    {
                        contractMaintenance.Milestones.OrderBy(m => m.EstimatedDate);
                        contractMaintenanceVOList.Add(new ContractMaintenanceVO(contractMaintenance,null));
                    }
                }
            }

            return contractMaintenanceVOList;
        }

        /// <summary>
        /// Get invoice in advance value from division table
        /// </summary>
        /// <param name="contractId">contract id</param>
        /// <returns>Invoice in advanced from division</returns>
        public int GetInvoiceInAdvanceFromDivision(int contractId)
        {
            int defaultInvoiceInAdvanced = 0;
            if (contractId != 0)
            {
                int divisionId = mdbDataContext.Contracts.FirstOrDefault(c => c.ID == contractId).DivisionID;
                defaultInvoiceInAdvanced = mdbDataContext.Divisions.FirstOrDefault(x => x.ID == divisionId).DefaultInvoiceInAdvanced;
            }
            return defaultInvoiceInAdvanced;
        }

        /// <summary>
        /// Gets the Billing Lines Texts filtered by Contract ID
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>Returns a list of Billing Line Text string</returns>
        public List<string> GetBillingLineTextByContractID(int contractId)
        {
            List<ContractMaintenance> contractMaintenances =
                mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId &&
                    c.IsDeleted == false).ToList();

            return new ContractMaintenanceVO(contractMaintenances).MaintenanceBillingLineText;
        }

        /// <summary>
        /// Delete milestones and associated milestone billing line text
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        private void DeleteMilestones(ContractMaintenanceVO contractMaintenanceVO)
        {
            //Delete milestones and associated  billing line texts
            List<Milestone> milestones = mdbDataContext.Milestones.Where(c => c.MaintenanceID == contractMaintenanceVO.ID && c.IsDeleted == false).ToList();
            foreach (var milestone in milestones)
            {
                if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                {
                    milestone.IsDeleted = true;
                    milestone.LastUpdatedDate = DateTime.Now;
                    milestone.LastUpdatedBy = contractMaintenanceVO.LastUpdatedByUserId;

                    //Delete billing line tags
                    List<MilestoneBillingLine> milestoneBillingLines = mdbDataContext.MilestoneBillingLines.Where(c => c.MilestoneID == milestone.ID).ToList();
                    foreach (var milestoneBillingLine in milestoneBillingLines)
                    {
                        milestoneBillingLine.IsDeleted = true;
                        milestoneBillingLine.LastUpdatedDate = milestone.LastUpdatedDate;
                        milestoneBillingLine.LastUpdatedBy = milestone.LastUpdatedBy;
                    }
                }
            }
        }

        /// <summary>
        /// Copy billing lines of copied contract
        /// </summary>
        /// <param name="contractMaintenanceVOList">contractMaintenance Value Objcet List</param>
        /// <param name="contractId">contract Id</param>
        /// <param name="contractLineId">contractLine Id</param>
        /// <param name="userId">login Id</param>
        public void SaveCopyContractMaintenanceForContract(List<ContractMaintenanceVO> contractMaintenanceVOList, int contractId, int contractLineId,int? userId)
        {

            foreach (var contractMaintenanceVO in contractMaintenanceVOList)
            {
                ContractMaintenance copyContractMaintenance = new ContractMaintenance();

                copyContractMaintenance.ContractID = contractId;
                copyContractMaintenance.ContractLineID = contractLineId;
                copyContractMaintenance.ChargeFrequencyID = contractMaintenanceVO.PeriodFrequencyId;
                //  contractMaintenance.ContractLine.ActivityCode = contractMaintenanceVO.ActivityCodeId;

                copyContractMaintenance.ProductID = contractMaintenanceVO.ProductId;

                copyContractMaintenance.SubProductID = contractMaintenanceVO.SubProductId;

                if (contractMaintenanceVO.InflationIndexId > 0)
                {
                    copyContractMaintenance.InflationIndexID = contractMaintenanceVO.InflationIndexId;
                }
                copyContractMaintenance.InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
                copyContractMaintenance.ReasonCode = contractMaintenanceVO.ReasonId;
                copyContractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;
                copyContractMaintenance.BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
                copyContractMaintenance.FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
                copyContractMaintenance.FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate;
                copyContractMaintenance.FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate;
                copyContractMaintenance.FirstAnnualUpliftDate = contractMaintenanceVO.FirstAnnualUpliftDate;
                copyContractMaintenance.FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate;
                copyContractMaintenance.FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
                copyContractMaintenance.EndAmount = contractMaintenanceVO.EndAmount;
                //contractMaintenance.ContractLine.OAActivityCode.ActivityCode = contractMaintenanceVO.ActivityCode;
                copyContractMaintenance.ForecastBillingStartDate = contractMaintenanceVO.ForecastBillingStartDate;
                copyContractMaintenance.QTY = contractMaintenanceVO.QTY;
                //Khushboo
                //At the time of Backlog, the Activation Date should be null
                if (copyContractMaintenance.IncludeInForecast == 0)
                {
                    copyContractMaintenance.ReasonDate = DateTime.Now.Date;
                }
                else
                {
                    copyContractMaintenance.ReasonDate = null;
                }
               
                copyContractMaintenance.CreationDate = DateTime.Now.Date;
                //copyContractMaintenance.CreationDate = contractMaintenanceVO.CreationDate;
                copyContractMaintenance.DeleteDate = contractMaintenanceVO.DeleteDate;
                //copyContractMaintenance.ReasonDate = contractMaintenanceVO.ReasonDate;
                copyContractMaintenance.InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;
                //contractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;

                copyContractMaintenance.UpliftRequired = contractMaintenanceVO.UpliftRequired;
                copyContractMaintenance.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional.HasValue ? contractMaintenanceVO.InflationFixedAdditional / 100 : contractMaintenanceVO.InflationFixedAdditional;                
                //contractMaintenance.TerminationReason = contractMaintenanceVO.TerminationReason;
                copyContractMaintenance.DeleteReason = contractMaintenanceVO.DeleteReason;

                //Added fields for grouping of billing lines
                copyContractMaintenance.GroupId = contractMaintenanceVO.GroupId;
                copyContractMaintenance.GroupName = contractMaintenanceVO.GroupName;
                copyContractMaintenance.IsGrouped = contractMaintenanceVO.IsGrouped;
                copyContractMaintenance.IsDefaultLineInGroup = contractMaintenanceVO.IsDefaultLineInGroup.HasValue ? contractMaintenanceVO.IsDefaultLineInGroup : null;

                copyContractMaintenance.DocumentTypeID = contractMaintenanceVO.DocumentTypeId;

                copyContractMaintenance.Comment = !string.IsNullOrEmpty(contractMaintenanceVO.Comment) ? contractMaintenanceVO.Comment.Trim() : contractMaintenanceVO.Comment;
                //to store current date and time with user id in database               
                copyContractMaintenance.CreatedBy = userId;
                copyContractMaintenance.CreatedDate = DateTime.Now;

                //Save contract maintenance lines
                copyContractMaintenance.MaintenanceBillingLines = new System.Data.Linq.EntitySet<MaintenanceBillingLine>();
                foreach (var item in contractMaintenanceVO.MaintenanceBillingLineVos)
                {
                    copyContractMaintenance.MaintenanceBillingLines.Add(new MaintenanceBillingLine() { LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText, LineSequance = item.LineSequance, CreatedBy = contractMaintenanceVO.CreatedByUserId, CreationDate = DateTime.Now });
                }

                mdbDataContext.ContractMaintenances.InsertOnSubmit(copyContractMaintenance);
                mdbDataContext.SubmitChanges();
            }

        }

        /// <summary>
        /// copy billing line
        /// </summary>
        /// <param name="contractMaintenanceVO">contractMaintenance Value Object</param>
        /// <param name="isCreditRecord">check is Credit record</param>
        /// <param name="userId">login user id</param>
        public void SaveContractMaintenanceCopy(ContractMaintenanceVO contractMaintenanceVO, bool isCreditRecord,int? userId)
        {
            ContractMaintenance copyContractMaintenance = new ContractMaintenance();

            copyContractMaintenance.ContractID = contractMaintenanceVO.ContractId;
            copyContractMaintenance.ContractLineID = contractMaintenanceVO.ContractLineId;

            //To check if record is to convert into Credit or not
            if (isCreditRecord != true)
            {
                copyContractMaintenance.ChargeFrequencyID = contractMaintenanceVO.PeriodFrequencyId;
                copyContractMaintenance.BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount;
                copyContractMaintenance.FirstPeriodAmount = contractMaintenanceVO.FirstPeriodAmount;
                copyContractMaintenance.EndAmount = contractMaintenanceVO.EndAmount;

                //Add Uplift information
                if(contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC))
                {
                    copyContractMaintenance.UpliftRequired = contractMaintenanceVO.UpliftRequired;
                    copyContractMaintenance.FirstAnnualUpliftDate = contractMaintenanceVO.FirstAnnualUpliftDate;
                    copyContractMaintenance.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional.HasValue ? contractMaintenanceVO.InflationFixedAdditional / 100 : contractMaintenanceVO.InflationFixedAdditional;
                    if (contractMaintenanceVO.InflationIndexId > 0)
                    {
                        copyContractMaintenance.InflationIndexID = contractMaintenanceVO.InflationIndexId;
                    }
                }
                else
                {
                    //Add Uplift Information for Ad-hoc as null
                    copyContractMaintenance.UpliftRequired = false;
                    copyContractMaintenance.FirstAnnualUpliftDate = null;
                    copyContractMaintenance.InflationFixedAdditional = null;
                    copyContractMaintenance.InflationIndexID = null;
                }

                //Added fields for grouping of billing lines
                copyContractMaintenance.GroupId = contractMaintenanceVO.GroupId;
                copyContractMaintenance.GroupName = contractMaintenanceVO.GroupName;
                copyContractMaintenance.IsGrouped = contractMaintenanceVO.IsGrouped;
                copyContractMaintenance.IsDefaultLineInGroup = contractMaintenanceVO.IsDefaultLineInGroup.HasValue ? contractMaintenanceVO.IsDefaultLineInGroup = false : null;
            }
            else
            {
                copyContractMaintenance.ChargeFrequencyID = Convert.ToInt32(Constants.ChargeFrequency.CREDIT);
                copyContractMaintenance.BaseAnnualAmount = contractMaintenanceVO.BaseAnnualAmount * -1;
                copyContractMaintenance.FirstPeriodAmount = contractMaintenanceVO.BaseAnnualAmount * -1;
                copyContractMaintenance.EndAmount = contractMaintenanceVO.EndAmount.HasValue ? contractMaintenanceVO.EndAmount * -1 : null;

                //Uplift values are null for Credit charge frequency
                copyContractMaintenance.UpliftRequired = false;
                copyContractMaintenance.FirstAnnualUpliftDate = null;
                copyContractMaintenance.InflationFixedAdditional = null;
                copyContractMaintenance.InflationIndexID = null;
                

                //Added fields for grouping of billing lines
                copyContractMaintenance.GroupId = null;
                copyContractMaintenance.GroupName = "Ungroup";
                copyContractMaintenance.IsGrouped = null;
                copyContractMaintenance.IsDefaultLineInGroup = null;

            }
            //  contractMaintenance.ContractLine.ActivityCode = contractMaintenanceVO.ActivityCodeId;

            copyContractMaintenance.ProductID = contractMaintenanceVO.ProductId;
            copyContractMaintenance.SubProductID = contractMaintenanceVO.SubProductId;

            copyContractMaintenance.InvoiceInAdvance = contractMaintenanceVO.InvoiceAdvancedId;
            copyContractMaintenance.ReasonCode = contractMaintenanceVO.ReasonId;
            copyContractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;
            copyContractMaintenance.FirstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate;
            copyContractMaintenance.FirstRenewalDate = contractMaintenanceVO.FirstRenewalDate;            
            copyContractMaintenance.FinalRenewalStartDate = contractMaintenanceVO.FinalRenewalStartDate;
            copyContractMaintenance.FinalRenewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
            //contractMaintenance.ContractLine.OAActivityCode.ActivityCode = contractMaintenanceVO.ActivityCode;
            copyContractMaintenance.ForecastBillingStartDate = contractMaintenanceVO.ForecastBillingStartDate;
            copyContractMaintenance.QTY = contractMaintenanceVO.QTY;
            //copyContractMaintenance.CreationDate = contractMaintenanceVO.CreationDate;
            //Khushboo
            //At the time of Backlog, the Activation Date should be null
            if (copyContractMaintenance.IncludeInForecast == 0)
            {
                copyContractMaintenance.ReasonDate = DateTime.Now.Date;
            }
            else
            {
                copyContractMaintenance.ReasonDate = null;
            }
            copyContractMaintenance.CreationDate = DateTime.Now.Date;
            
            copyContractMaintenance.DeleteDate = contractMaintenanceVO.DeleteDate;
            //copyContractMaintenance.ReasonDate = contractMaintenanceVO.ReasonDate;
            
            copyContractMaintenance.InvoiceAdvancedArrears = contractMaintenanceVO.InvoiceAdvancedArrears;
            //contractMaintenance.IncludeInForecast = contractMaintenanceVO.IncludeInForecast;            
            //contractMaintenance.TerminationReason = contractMaintenanceVO.TerminationReason;
            copyContractMaintenance.DeleteReason = contractMaintenanceVO.DeleteReason;

            copyContractMaintenance.DocumentTypeID = contractMaintenanceVO.DocumentTypeId;

            copyContractMaintenance.Comment = !string.IsNullOrEmpty(contractMaintenanceVO.Comment) ? contractMaintenanceVO.Comment.Trim() : contractMaintenanceVO.Comment;
            //to store current date and time with user id in database
            copyContractMaintenance.CreatedBy = userId;
            copyContractMaintenance.CreatedDate = DateTime.Now;

            //Save contract maintenance lines
            copyContractMaintenance.MaintenanceBillingLines = new System.Data.Linq.EntitySet<MaintenanceBillingLine>();
            foreach (var item in contractMaintenanceVO.MaintenanceBillingLineVos)
            {
                copyContractMaintenance.MaintenanceBillingLines.Add(new MaintenanceBillingLine() { LineText = !string.IsNullOrEmpty(item.LineText) ? item.LineText.Trim() : item.LineText, LineSequance = item.LineSequance, CreatedBy = contractMaintenanceVO.CreatedByUserId, CreationDate = DateTime.Now });
            }

            mdbDataContext.ContractMaintenances.InsertOnSubmit(copyContractMaintenance);
            mdbDataContext.SubmitChanges();

        }

        /// <summary>
        /// To get billing lines details based on contract id on image click
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceDetails(int contractId)
        {
            List<ContractMaintenance> contractMaintenanceList = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.IsDeleted == false).ToList();
            List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();

            foreach (ContractMaintenance contractMaintenance in contractMaintenanceList)
            {
                contractMaintenanceVOList.Add(new ContractMaintenanceVO(contractMaintenance));
            }
            contractMaintenanceVOList = contractMaintenanceVOList.OrderBy(c => c.GroupId).ThenBy(c => c.GroupName).ToList();
            return contractMaintenanceVOList;
        }

        /// <summary>
        /// to know whether the end user is associated with any contract and with any maintenance billing lines
        /// </summary>
        /// <param name="endUserId"></param>
        /// <returns>boolean</returns>
        public bool GetMaintenanceBillingLines(string endUserId)
        {
            List<Contract> contractList = mdbDataContext.Contracts.Where(c => c.EndUserID == endUserId && c.IsDeleted == false).ToList();

            List<ContractMaintenance> contractMaintenanceList = new List<ContractMaintenance>();
            bool status = false;
            if (contractList.Count != 0)
            {
                foreach (Contract contract in contractList)
                {
                    contractMaintenanceList = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contract.ID && c.IsDeleted == false).ToList();
                    if (contractMaintenanceList.Count != 0)
                    {
                        //whenever there are billing lines for a particular contract
                        status = true;
                    }
                }
            }
            else
            {
                //whenever there are no billing lines for a particular contract
                status = false;
            }

            return status;
        }

        #region Commented Code

        ///// <summary>
        ///// Gets the list of contract maintenance
        ///// </summary>
        ///// <param name="contractLineId">The contract line id</param>
        ///// <param name="activityCodeId">The activity code id</param>
        ///// <returns>List of contracts maintenance</returns>
        //public List<ContractMaintenanceVO> GetContractMaintenanceList(int contractLineId, int activityCodeId)
        //{
        //    List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();

        //    List<ContractMaintenance> contractMaintenances =
        //        mdbDataContext.ContractMaintenances.Where(c => c.ContractLineID == contractLineId &&
        //            c.ContractLine.ActivityCodeID == activityCodeId &&
        //            c.IsDeleted == false).ToList();

        //    foreach (var item in contractMaintenances)
        //    {
        //        contractMaintenancesVOList.Add(new ContractMaintenanceVO(item));
        //    }

        //    return contractMaintenancesVOList;
        //}


        ////Delete billing line tags
        //List<MaintenanceBillingLine> maintenanceBillingLines = mdbDataContext.MaintenanceBillingLines.Where(c => c.MaintenanceID == id).ToList();
        //foreach (var maintenanceBillingLine in maintenanceBillingLines)
        //{
        //    maintenanceBillingLine.IsDeleted = true;
        //    maintenanceBillingLine.LastUpdatedDate = DateTime.Now;
        //    maintenanceBillingLine.LastUpdatedBy = userId;
        //}

        ////Delete milestones
        //List<Milestone> milestones = mdbDataContext.Milestones.Where(c => c.MaintenanceID == id && c.IsDeleted == false).ToList();
        //foreach (var milestone in milestones)
        //{
        //    milestone.IsDeleted = true;
        //    milestone.LastUpdatedDate = DateTime.Now;
        //    milestone.LastUpdatedBy = userId;
        //}

        ////Delete billing line texts for the deleted milestones
        //foreach (var item in milestones)
        //{
        //    //Delete billing line tags
        //    List<MilestoneBillingLine> milestoneBillingLines = mdbDataContext.MilestoneBillingLines.Where(c => c.MilestoneID == item.ID).ToList();
        //    foreach (var milestoneBillingLine in milestoneBillingLines)
        //    {
        //        milestoneBillingLine.IsDeleted = true;
        //        milestoneBillingLine.LastUpdatedDate = DateTime.Now;
        //        milestoneBillingLine.LastUpdatedBy = item.LastUpdatedBy;
        //    }
        //}

        #endregion Commented Code
    }
}