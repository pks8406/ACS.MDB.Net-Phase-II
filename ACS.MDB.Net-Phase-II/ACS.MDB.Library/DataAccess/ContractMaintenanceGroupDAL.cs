using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ContractMaintenanceGroupDAL : BaseDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="userId"></param>
        public void GroupContractMaintenance(List<int> Ids, int? userId, int defaultInGroupId)
        {
            string groupName = string.Empty;
            int groupId = 0;
                        
            if (Ids.Count > 0)
            {
                foreach (var item in Ids)
                {
                    if (item != 0)
                    {
                        ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == item);
                        if (contractMaintenance.GroupId != null && contractMaintenance.GroupName != "Ungroup")
                        {
                            groupId = contractMaintenance.GroupId.Value;
                            groupName = contractMaintenance.GroupName;
                            break;
                        }
                        else
                        {
                            groupId = GetGroupId(contractMaintenance);
                            groupName = GetGroupName(contractMaintenance) + " - " + groupId;                            
                        }
                    }
                }
                //ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.Where(cm => cm.ID == Ids[0]).FirstOrDefault();
                //groupName = GetGroupName(contractMaintenance);
                //groupId = GetGroupId(contractMaintenance);
            }

            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == id);

                    if (id == defaultInGroupId)
                    {
                        contractMaintenance.IsDefaultLineInGroup = true;
                    }
                    else
                    {
                        contractMaintenance.IsDefaultLineInGroup = false;
                    }

                    contractMaintenance.GroupId = groupId;
                    contractMaintenance.GroupName = groupName;
                    contractMaintenance.IsGrouped = true;
                    contractMaintenance.LastUpdatedDate = DateTime.Now;
                    contractMaintenance.LastUpdatedBy = userId;
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Gets the list of contract Maintenance
        /// </summary>
        /// <param name="periodFrequencyId">periodFrequencyId</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupList(int contractId, int periodFrequencyId, 
                                                                           DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                           DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                                           int invoiceAdvancedArrears,int invoiceInAdvance)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenance> tempContractManteinance = new List<ContractMaintenance>();

            #region Khushboo
            if (firstPeriodStartDate != null)
            {
                //chargeFrequency=Monthly
                if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Month, firstPeriodStartDate.Value.Month)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }
            }
            else if (firstPeriodStartDate == null)
            {
                //chargeFrequency=Monthly
                if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }
            }
            //chargeFrequency= credit or adhoc
            if ((periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) || (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
            {
                contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                    && c.ChargeFrequencyID == periodFrequencyId
                                                                                                    && object.Equals(c.FinalRenewalStartDate, finalBillingPeriodStartDate)
                                                                                                    && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                    && c.DocumentTypeID == documentTypeId
                                                                                                    && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                    && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                    && c.IsDeleted == false).ToList();
            }
            #endregion
            #region Commented Code
            //if (firstPeriodStartDate != null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId                                                                                          
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate == null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId                                                                                          
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate != null && finalBillingPeriodEndDate == null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId                                                                                          
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId                                                                                         
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsDeleted == false).ToList();
            //} 
            #endregion
   
            //List<LINQ.AuditReason> AuditReasonList = mdbDataContext.AuditReasons.ToList();

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            contractMaintenancesVOList = contractMaintenancesVOList.OrderByDescending(c => c.IsGrouped).ToList();
            return contractMaintenancesVOList;
        }

        #region Khushboo
        
        /// <summary>
        /// Get ungrouped billing lines which is matching the group criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="firstRenewalDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="invoiceAdvancedArrears"></param>
        /// <param name="invoiceInAdvance"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        private List<ContractMaintenance> GetContractMaintenanceGroupList(int contractId, int periodFrequencyId,
                                                        DateTime? firstPeriodStartDate, DateTime? firstRenewalDate,
                                                        DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                        int invoiceAdvancedArrears, int invoiceInAdvance, int months)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenance> tempContractManteinance = new List<ContractMaintenance>();

            DateTime? maxFirstPeriodStartDate = null;
            DateTime? minFirstPeriodStartDate = null;
            DateTime? maxFirstRenewalDate = null;
            DateTime? minFirstRenewalDate = null;

            if (firstPeriodStartDate != null)
            {
                //get maximum FirstPeriodStartDate for a particular contract and for a particular charge frequency from the database
                maxFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstPeriodStartDate).Value;

                //get minimum FirstPeriodStartDate for a particular contract and for a particular charge frequency from the database
                minFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstPeriodStartDate).Value;

                DateTime? beforeDate = firstPeriodStartDate;

                while (beforeDate >= minFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(beforeDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstPeriodStartDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                DateTime? afterDate = firstPeriodStartDate.Value.AddMonths(months);
                while (afterDate <= maxFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(afterDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }
            else if (firstPeriodStartDate == null)
            {
                //get maximum FirstRenewalDate for a particular contract and for a particular charge frequency from the database
                maxFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstRenewalDate).Value;

                //get minimum FirstRenewalDate for a particular contract and for a particular charge frequency from the database
                minFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstRenewalDate).Value;

                DateTime? beforeDate = firstRenewalDate;

                while (beforeDate >= minFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(beforeDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstRenewalDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                DateTime? afterDate = firstRenewalDate.Value.AddMonths(months);
                while (afterDate <= maxFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(afterDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }
            return contractMaintenances;
        }

        /// <summary>
        /// Get Ungrouped item list based on the grouping criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="firstRenewalDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="invoiceAdvancedArrears"></param>
        /// <param name="invoiceInAdvance"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        private List<ContractMaintenance> GetContractMaintenanceofUnGroupedItemList(int contractId, int periodFrequencyId,
                                                        DateTime? firstPeriodStartDate, DateTime? firstRenewalDate,
                                                        DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                        int invoiceAdvancedArrears, int invoiceInAdvance, int months)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenance> tempContractManteinance = new List<ContractMaintenance>();

            DateTime? maxFirstPeriodStartDate = null;
            DateTime? minFirstPeriodStartDate = null;
            DateTime? maxFirstRenewalDate = null;
            DateTime? minFirstRenewalDate = null;

            if (firstPeriodStartDate != null)
            {
                //get maximum FirstPeriodStartDate for a particular contract and for a particular charge frequency from the database
                maxFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstPeriodStartDate).Value;

                //get minimum FirstPeriodStartDate for a particular contract and for a particular charge frequency from the database
                minFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstPeriodStartDate).Value;

                DateTime? beforeDate = firstPeriodStartDate;

                while (beforeDate >= minFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(beforeDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstPeriodStartDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                DateTime? afterDate = firstPeriodStartDate.Value.AddMonths(months);
                while (afterDate <= maxFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(afterDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }
            else if (firstPeriodStartDate == null)
            {
                //get maximum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                maxFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstRenewalDate).Value;

                //get minimum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                minFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstRenewalDate).Value;

                DateTime? beforeDate = firstRenewalDate;

                while (beforeDate >= minFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(beforeDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstRenewalDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                DateTime? afterDate = firstRenewalDate.Value.AddMonths(months);
                while (afterDate <= maxFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(afterDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }

            return contractMaintenances;
        }
        /// <summary>
        /// Get grouped item list based on the grouping criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="firstRenewalDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="invoiceAdvancedArrears"></param>
        /// <param name="invoiceInAdvance"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        private List<ContractMaintenance> GetContractMaintenanceofGroupedItemList(int contractId, int periodFrequencyId,
                                                        DateTime? firstPeriodStartDate, DateTime? firstRenewalDate,
                                                        DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                        int invoiceAdvancedArrears, int invoiceInAdvance, int months)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenance> tempContractManteinance = new List<ContractMaintenance>();

            DateTime? maxFirstPeriodStartDate = null;
            DateTime? minFirstPeriodStartDate = null;
            DateTime? maxFirstRenewalDate = null;
            DateTime? minFirstRenewalDate = null;

            if (firstPeriodStartDate != null)
            {
                //get maximum FirstPeriodStartDate for that particular contract and for a particular charge frequency from the database
                maxFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c =>  c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted ).Max(c => c.FirstPeriodStartDate).Value;

                //get minimum FirstPeriodStartDate for that particular contract and for a particular charge frequency from the database
                minFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted ).Min(c => c.FirstPeriodStartDate).Value;

                DateTime? beforeDate = firstPeriodStartDate;

                while (beforeDate >= minFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(beforeDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == true
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstPeriodStartDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                DateTime? afterDate = firstPeriodStartDate.Value.AddMonths(months);
                while (afterDate <= maxFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(afterDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == true
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }
            else if (firstPeriodStartDate == null)
            {
                //get maximum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                maxFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId  && !c.IsDeleted ).Max(c => c.FirstRenewalDate).Value;

                //get minimum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                minFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted ).Min(c => c.FirstRenewalDate).Value;

                DateTime? beforeDate = firstRenewalDate;

                while (beforeDate >= minFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(beforeDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == true
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstRenewalDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                DateTime? afterDate = firstRenewalDate.Value.AddMonths(months);
                while (afterDate <= maxFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(afterDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped == true
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }
            return contractMaintenances;
        }

        /// <summary>
        /// Get Group Name based on the grouping criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="firstRenewalDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="invoiceAdvancedArrears"></param>
        /// <param name="invoiceInAdvance"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        private List<ContractMaintenance> GetContractMaintenanceGroupNameList(int contractId, int periodFrequencyId,
                                                        DateTime? firstPeriodStartDate, DateTime? firstRenewalDate,
                                                        DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                        int invoiceAdvancedArrears, int invoiceInAdvance, int months)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenance> tempContractManteinance = new List<ContractMaintenance>();

            DateTime? maxFirstPeriodStartDate = null;
            DateTime? minFirstPeriodStartDate = null;
            DateTime? maxFirstRenewalDate = null;
            DateTime? minFirstRenewalDate = null;

            if (firstPeriodStartDate != null)
            {
                //get maximum FirstPeriodStartDate for that particular contract and for a particular charge frequency from the database
                maxFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstPeriodStartDate).Value;

                //get minimum FirstPeriodStartDate for that particular contract and for a particular charge frequency from the database
                minFirstPeriodStartDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstPeriodStartDate).Value;

                DateTime? beforeDate = firstPeriodStartDate;

                while (beforeDate >= minFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(beforeDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped != null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstPeriodStartDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                DateTime? afterDate = firstPeriodStartDate.Value.AddMonths(months);
                while (afterDate <= maxFirstPeriodStartDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstPeriodStartDate.Equals(afterDate)
                                                                                                       && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped != null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstPeriodStartDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);

                }
            }
            else if (firstPeriodStartDate == null)
            {
                //get maximum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                maxFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Max(c => c.FirstRenewalDate).Value;

                //get minimum FirstRenewalDate for that particular contract and for a particular charge frequency from the database
                minFirstRenewalDate = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId && c.ChargeFrequencyID == periodFrequencyId && !c.IsDeleted).Min(c => c.FirstRenewalDate).Value;

                DateTime? beforeDate = firstRenewalDate;

                while (beforeDate >= minFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(beforeDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped != null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date before the firstRenewalDate on the basis of PeriodFrequency
                    beforeDate = beforeDate.Value.AddMonths(-months);
                }

                //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                DateTime? afterDate = firstRenewalDate.Value.AddMonths(months);
                while (afterDate <= maxFirstRenewalDate)
                {
                    tempContractManteinance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                       && c.ChargeFrequencyID == periodFrequencyId
                                                                                                       && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                       && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                       && c.FirstRenewalDate.Equals(afterDate)
                                                                                                       && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                       && c.IsGrouped != null
                                                                                                       && c.DocumentTypeID == documentTypeId
                                                                                                       && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                       && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                       && c.IsDeleted == false).ToList();



                    if (tempContractManteinance.Count != 0)
                    {
                        contractMaintenances.AddRange(tempContractManteinance);
                    }

                    //calculate date after the firstRenewalDate on the basis of PeriodFrequency
                    afterDate = afterDate.Value.AddMonths(months);
                }
            }

            return contractMaintenances;
        }

        #endregion

        /// <summary>
        /// Gets group Id
        /// </summary>
        /// <param name="contractMaintenance"></param>
        /// <returns>Group Id</returns>
        public int GetGroupId(ContractMaintenance contractMaintenance)
        {
            int groupId;
            ChargingFrequencyDAL chargeFrequencyDAL = new ChargingFrequencyDAL();
            //List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupList(contractMaintenance.ContractID,
            //                                                                                           contractMaintenance.ChargeFrequencyID,
            //                                                                                           contractMaintenance.FirstPeriodStartDate,
            //                                                                                           contractMaintenance.FinalRenewalEndDate);
            List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupListByContractIdAndPeriodFrequencyId(contractMaintenance.ContractID,
                                                                                                       contractMaintenance.ChargeFrequencyID);

            var countGroupId = contractMaintenanceGroupList.Max(cm => cm.GroupId);

            //Check if Group name is already available or not
            if (countGroupId == null)
            {
                groupId = 1;
            }
            else
            {
                groupId = countGroupId.Value + 1;
            }

            return groupId;
        }

        /// <summary>
        /// Gets group name 
        /// </summary>
        /// <param name="contractMaintenance"></param>
        /// <returns>Group Name</returns>
        public string GetGroupName(ContractMaintenance contractMaintenance)
        {
            string groupName = string.Empty;
            string chargeFrequencyName = string.Empty;
            ChargingFrequencyDAL chargeFrequencyDAL = new ChargingFrequencyDAL();
            chargeFrequencyName = chargeFrequencyDAL.GetChargeFrequencyNameById(contractMaintenance.ChargeFrequencyID);

            List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupListByContractIdAndPeriodFrequencyId(contractMaintenance.ContractID,
                                                                                                       contractMaintenance.ChargeFrequencyID);

            var countGroupId = contractMaintenanceGroupList.Max(cm => cm.GroupId);
            

            //Check if Group name is already available or not
            if (countGroupId == null)
            {
                contractMaintenance.GroupName = chargeFrequencyName;
                groupName = contractMaintenance.GroupName;
            }
            else
            {
                groupName = contractMaintenance.GroupName = chargeFrequencyName;
            }

            return groupName;
        }

        /// <summary>
        /// Returns is contractMaintenance grouped or not
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="chargeFrequencyId">chargeFrequencyId</param>
        /// <returns>Is contractMaintenance Grouped or not</returns>
        public bool IsContractMaintenanceGrouped(int contractId, int periodFrequencyId, 
                                                 DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                 DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                 int invoiceAdvancedArrears,int invoiceInAdvance)
        {            

            List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,finalBillingPeriodEndDate, documentTypeId,invoiceAdvancedArrears, invoiceInAdvance);
            var countIsGrouped = contractMaintenanceGroupList.Max(cm => cm.IsGrouped);
            bool isGrouped;

            if (countIsGrouped == null)
            {
                isGrouped = true;
            }
            else
            {
                isGrouped = false;
            }

            return isGrouped;
        }
        

        /// <summary>
        /// Return number of ungrouped records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns></returns>
        public int GetRecordCountOfUngroupedElements(int contractId, int periodFrequencyId, 
                                                     DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                     DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                     int invoiceAdvancedArrears,int invoiceInAdvance)
        {
            List<ContractMaintenanceVO> contractMaintenances = GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,finalBillingPeriodEndDate, documentTypeId,invoiceAdvancedArrears,invoiceInAdvance);

            //List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate.Value, finalBillingPeriodEndDate.Value);
            var countIsGrouped = contractMaintenances.Count;

            return countIsGrouped;
        }

        /// <summary>
        /// Return number of grouped records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns></returns>
        public int GetRecordCountOfGroupedElements(int contractId, int periodFrequencyId, 
                                                   DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                   DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                   int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            List<ContractMaintenanceVO> contractMaintenances = GetContractMaintenanceGroupedItemsList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate, finalBillingPeriodEndDate, documentTypeId,invoiceAdvancedArrears,invoiceInAdvance);

            var countIsGrouped = contractMaintenances.GroupBy(c => c.GroupId).Count();

            return countIsGrouped;
        }

        /// <summary>
        /// Get total no of records in contract maintenance group list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodfrequency Id</param>
        /// <param name="groupId">group Id</param>
        /// <returns>Total no of records in list</returns>
        public int GetRecordCountofGroupedItemsByGroupId(int contractId, int periodFrequencyId, int groupId)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = GetContractMaintenanceGroupListByGroupId(contractId, periodFrequencyId, groupId);

            //List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate.Value, finalBillingPeriodEndDate.Value);
            var countIsGrouped = contractMaintenancesVOList.Count;

            return countIsGrouped;
        }

        /// <summary>
        /// Get contract maintenance Group list by group id and given criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        /// <returns>contract Maintenance group list</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupId(int contractId, int periodFrequencyId, int groupId)
        {            
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();
            List<ContractMaintenance> contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && c.GroupId == groupId && !c.IsDeleted).ToList();

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            return contractMaintenancesVOList;
        }        

        /// <summary>
        /// Returns list of Grouped items with provided criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupedItemsList(int contractId, int periodFrequencyId, 
                                                                                  DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                                  DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                                                  int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();

            if (firstPeriodStartDate != null)
            {
                #region Khushboo

                //chargeFrequency=Monthly
                if (periodFrequencyId ==  Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.IsGrouped == true
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Month, firstPeriodStartDate.Value.Month)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.IsGrouped == true
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }

            }
            else if(firstPeriodStartDate == null)
            {
                //chargeFrequency=Monthly
                if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.IsGrouped == true
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.IsGrouped == true
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceofGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }

            }
            //chargeFrequency= credit or adhoc
            if ((periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) || (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
            {
                contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                    && c.ChargeFrequencyID == periodFrequencyId
                                                                                                    && object.Equals(c.FinalRenewalStartDate, finalBillingPeriodStartDate)
                                                                                                    && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                    && c.IsGrouped == true
                                                                                                    && c.DocumentTypeID == documentTypeId
                                                                                                    && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                    && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                    && c.IsDeleted == false).ToList();
            }
            #endregion
            #region Commented Code
            //if (firstPeriodStartDate != null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsGrouped == true
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate == null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsGrouped == true
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate != null && finalBillingPeriodEndDate == null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsGrouped == true
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsGrouped == true
            //                                                                              && c.IsDeleted == false).ToList();
            //} 
            #endregion

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            return contractMaintenancesVOList;
        }

        /// <summary>
        /// Returns list of ungrouped items with provided criteria
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceUngroupedItemsList(int contractId, 
                                                                                    int periodFrequencyId, 
                                                                                    DateTime? firstPeriodStartDate,
                                                                                    DateTime? firstRenewalDate,
                                                                                    DateTime? finalBillingPeriodStartDate,
                                                                                    DateTime? finalBillingPeriodEndDate,
                                                                                    int documentTypeId,
                                                                                    int invoiceAdvancedArrears, int invoiceInAdvance,
                                                                                    string groupName = null)
        {
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();

            if (string.IsNullOrEmpty(groupName))
            {
                if (firstPeriodStartDate != null)
                {
                    #region Khushboo
                    //chargeFrequency=Monthly
                    if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                    {
                        contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                            && c.ChargeFrequencyID == periodFrequencyId
                                                                                                            && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                            && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                            && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                            && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                            && c.IsGrouped == null
                                                                                                            && c.DocumentTypeID == documentTypeId
                                                                                                            && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                            && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                            && c.IsDeleted == false).ToList();
                    }

                    //chargeFrequency=Quarterly
                    //last parameter is to calculate date at the interval of 3 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                    }

                    //chargeFrequency= HalfYearly
                    //last parameter is to calculate date at the interval of 6 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                    }

                    //chargeFrequency= Yearly
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                    {
                        contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                         && c.ChargeFrequencyID == periodFrequencyId
                                                                                                         && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                         && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                         && object.Equals(c.FirstPeriodStartDate.Value.Month, firstPeriodStartDate.Value.Month)
                                                                                                         && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                         && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                         && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                         && c.IsGrouped==null
                                                                                                         && c.DocumentTypeID == documentTypeId
                                                                                                         && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                         && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                         && c.IsDeleted == false).ToList();
                    }

             

                    //chargeFrequency=Bi Monthly
                    //last parameter is to calculate date at the interval of 2 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                    }
                }
                else if(firstPeriodStartDate == null)
                {
                    //chargeFrequency=Monthly
                    if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                    {
                        contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                            && c.ChargeFrequencyID == periodFrequencyId
                                                                                                            && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                            && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                            && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                            && c.IsGrouped == null
                                                                                                            && c.DocumentTypeID == documentTypeId
                                                                                                            && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                            && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                            && c.IsDeleted == false).ToList();
                    }

                    //chargeFrequency=Quarterly
                    //last parameter is to calculate date at the interval of 3 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                    }

                    //chargeFrequency= HalfYearly
                    //last parameter is to calculate date at the interval of 6 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                    }

                    //chargeFrequency= Yearly
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                    {
                        contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                         && c.ChargeFrequencyID == periodFrequencyId
                                                                                                         && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                         && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                         && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                         && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                         && c.IsGrouped == null
                                                                                                         && c.DocumentTypeID == documentTypeId
                                                                                                         && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                         && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                         && c.IsDeleted == false).ToList();
                    }

                    //chargeFrequency=Bi Monthly
                    //last parameter is to calculate date at the interval of 2 months
                    else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                    {
                        contractMaintenances = GetContractMaintenanceofUnGroupedItemList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                    }
                    
                }

                //chargeFrequency= credit or adhoc
                if ((periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) || (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FinalRenewalStartDate, finalBillingPeriodStartDate)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.IsGrouped == null
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }
            }
            #endregion
            else
            {
                contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                              && c.ChargeFrequencyID == periodFrequencyId
                                                                                              && c.GroupName == groupName
                                                                                              && c.IsDeleted == false).ToList();
            }

            #region Commented Code
            //if (firstPeriodStartDate != null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsGrouped == null
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate == null && finalBillingPeriodEndDate != null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == finalBillingPeriodEndDate
            //                                                                              && c.IsGrouped == null
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else if (firstPeriodStartDate != null && finalBillingPeriodEndDate == null)
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == firstPeriodStartDate
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsGrouped == null
            //                                                                              && c.IsDeleted == false).ToList();
            //}
            //else
            //{
            //    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
            //                                                                              && c.ChargeFrequencyID == periodFrequencyId
            //                                                                              && c.FirstPeriodStartDate == null
            //                                                                              && c.FinalRenewalEndDate == null
            //                                                                              && c.IsGrouped == null
            //                                                                              && c.IsDeleted == false).ToList();
            //} 
            #endregion

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }
            
            return contractMaintenancesVOList;
        }

        /// <summary>
        /// Gets list of grouped items 
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="groupName">Group Id</param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupName(int contractId, int periodFrequencyId, string groupName)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();
            List<ContractMaintenance> contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                          && c.ChargeFrequencyID == periodFrequencyId                                                                                                                                                                                    
                                                                                          && c.GroupName == groupName                                                                                          
                                                                                          && c.IsDeleted == false).ToList();
            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            return contractMaintenancesVOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<string> GetContractMaintenanceGroupNameList(int contractId, int periodFrequencyId, 
                                                                DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                                int invoiceAdvancedArrears, int invoiceInAdvance) 
        {
            List<string> contractMaintenancesGroupList = new List<string>();
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();
            #region Khushboo
            
            if (firstPeriodStartDate != null)
            {
                //chargeFrequency=Monthly
                if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.IsGrouped != null
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }
                 //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Day, firstPeriodStartDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstPeriodStartDate.Value.Month, firstPeriodStartDate.Value.Month)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && c.FirstPeriodStartDate.Equals(c.FirstRenewalDate)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.IsGrouped != null
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

        

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }
            }
            
            else if(firstPeriodStartDate == null)
            {
                //chargeFrequency=Monthly
                if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                        && c.ChargeFrequencyID == periodFrequencyId
                                                                                                        && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                        && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                        && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                        && c.IsGrouped != null
                                                                                                        && c.DocumentTypeID == documentTypeId
                                                                                                        && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                        && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                        && c.IsDeleted == false).ToList();
                }
                //chargeFrequency=Quarterly
                //last parameter is to calculate date at the interval of 3 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 3);
                }

                //chargeFrequency= HalfYearly
                //last parameter is to calculate date at the interval of 6 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 6);
                }

                //chargeFrequency= Yearly
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
                {
                    contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                     && c.ChargeFrequencyID == periodFrequencyId
                                                                                                     && object.Equals(c.FirstPeriodStartDate, firstPeriodStartDate)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Day, firstRenewalDate.Value.Day)
                                                                                                     && object.Equals(c.FirstRenewalDate.Value.Month, firstRenewalDate.Value.Month)
                                                                                                     && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                     && c.IsGrouped != null
                                                                                                     && c.DocumentTypeID == documentTypeId
                                                                                                     && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                     && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                     && c.IsDeleted == false).ToList();
                }

                //chargeFrequency=Bi Monthly
                //last parameter is to calculate date at the interval of 2 months
                else if (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
                {
                    contractMaintenances = GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, firstPeriodStartDate, firstRenewalDate, finalBillingPeriodEndDate, documentTypeId, invoiceAdvancedArrears, invoiceInAdvance, 2);
                }

            }

            //chargeFrequency= credit or adhoc
            if ((periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)) || (periodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT)))
            {
                contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                                    && c.ChargeFrequencyID == periodFrequencyId
                                                                                                    && object.Equals(c.FinalRenewalStartDate, finalBillingPeriodStartDate)
                                                                                                    && object.Equals(c.FinalRenewalEndDate, finalBillingPeriodEndDate)
                                                                                                    && c.IsGrouped != null
                                                                                                    && c.DocumentTypeID == documentTypeId
                                                                                                    && c.InvoiceAdvancedArrears == invoiceAdvancedArrears
                                                                                                    && c.InvoiceInAdvance == invoiceInAdvance
                                                                                                    && c.IsDeleted == false).ToList();
            }

            #endregion

            foreach (var item in contractMaintenances)
            {
                contractMaintenancesGroupList.Add(item.GroupName);
            }

            return contractMaintenancesGroupList.Distinct().ToList();
        }

        /// <summary>
        /// Gets the list of contract Maintenance
        /// </summary>
        /// <param name="periodFrequencyId">periodFrequencyId</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByContractIdAndPeriodFrequencyId(int contractId, int periodFrequencyId)
        {
            List<ContractMaintenanceVO> contractMaintenancesVOList = new List<ContractMaintenanceVO>();
            List<ContractMaintenance> contractMaintenances = new List<ContractMaintenance>();

            contractMaintenances = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == contractId
                                                                                        && c.ChargeFrequencyID == periodFrequencyId                                                                                        
                                                                                        && c.IsDeleted == false).ToList();
            
            foreach (var item in contractMaintenances)
            {
                contractMaintenancesVOList.Add(new ContractMaintenanceVO(item,null));
            }

            return contractMaintenancesVOList;
        }

        /// <summary>
        /// Ungroup the contractmaintenance Group
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        public void UngroupContractMaintenance(int contractId, int periodFrequencyId, int groupId, int? userId)
        {
            List<ContractMaintenanceVO> contractMaintenanceVOList = GetContractMaintenanceGroupListByGroupId(contractId, periodFrequencyId, groupId);            
            foreach (var item in contractMaintenanceVOList)
            {                
                ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == item.ID);

                contractMaintenance.GroupId = null;
                contractMaintenance.GroupName = "Ungroup";
                contractMaintenance.IsGrouped = null;
                contractMaintenance.IsDefaultLineInGroup = null;
                contractMaintenance.LastUpdatedDate = DateTime.Now;
                contractMaintenance.LastUpdatedBy = userId;

                //Delete Milestones with status ready for calculating
                DeleteMilestoneofUngroupedBillingLines(item.ID, userId);
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Ungroup the contractMaintenance item
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenance Id</param>
        /// <param name="userId">user Id</param>
        public void UngroupContractMaintenanceItem(int contractMaintenanceId, int? userId)
        {
            ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(c => c.ID == contractMaintenanceId);            

            contractMaintenance.GroupId = null;
            contractMaintenance.GroupName = "Ungroup";
            contractMaintenance.IsGrouped = null;
            contractMaintenance.IsDefaultLineInGroup = null;
            contractMaintenance.LastUpdatedDate = DateTime.Now;
            contractMaintenance.LastUpdatedBy = userId;

            //Delete Milestones with status ready for calculating
            DeleteMilestoneofUngroupedBillingLines(contractMaintenanceId, userId);

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Delete milestones if billing line is ungrouped
        /// </summary>
        /// <param name="contractMaintenanceId">contractmMaintenanceId</param>
        /// <param name="userId">user id</param>        
        private void DeleteMilestoneofUngroupedBillingLines(int contractMaintenanceId, int? userId)
        {
            MilestoneDAL milestoneDAL = new MilestoneDAL();            
            List<MilestoneVO> milestoneVOList = milestoneDAL.GetMilestoneList(contractMaintenanceId, false);
            //To check any milestatus is Ready for calculating
            bool checkMilestoneStatus = milestoneVOList.Any(m => m.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING) && m.IsDeleted == false);
            if (checkMilestoneStatus == true)
            {
                List<int> milestoneIds = new List<int>();
                foreach (var milestoneVO in milestoneVOList)
                {
                    milestoneIds.Add(milestoneVO.ID);
                }

                milestoneDAL.DeleteMilestone(milestoneIds, userId);
            }
        }

        #region Commented Code
        /// <summary>
        /// Return number of records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns></returns>
        //public int GetRecordCount(int contractId, int periodFrequencyId, DateTime? firstPeriodStartDate, DateTime? finalBillingPeriodEndDate)
        //{

        //    //List<ContractMaintenance> contractMaintenances = GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, firstPeriodStartDate, finalBillingPeriodEndDate);
        //    List<ContractMaintenanceVO> contractMaintenanceGroupList = GetContractMaintenanceGroupList(contractId, periodFrequencyId, firstPeriodStartDate, finalBillingPeriodEndDate);
        //    var countIsGrouped = contractMaintenanceGroupList.Count;
        //    return countIsGrouped;
        //}
        #endregion
    }
}
