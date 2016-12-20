using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class ContractMaintenanceGroupService
    {

        ContractMaintenanceGroupDAL contractMaintenanceGroupDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractMaintenanceGroupService()
        {
            contractMaintenanceGroupDAL = new ContractMaintenanceGroupDAL();
        }

        /// <summary>
        /// Group billing lines
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="userId"></param>
        /// <param name="defaultInGroupId"></param>
        public void GroupContractMaintenance(List<int> Ids, int? userId, int defaultInGroupId)
        {
            bool isDefaultLineValid = IsDefaultLineValid(Ids, defaultInGroupId);
            if (isDefaultLineValid == true)
            {
                contractMaintenanceGroupDAL.GroupContractMaintenance(Ids, userId, defaultInGroupId);
            }
            else
            {
                throw new ApplicationException("Please select valid radio button");
            }

        }

        /// <summary>
        /// Gets the list of contract Maintenance
        /// </summary>
        /// <param name="periodFrequencyId">periodFrequencyId</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupList(int contractId, int periodFrequencyId, 
                                                                           DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                           DateTime? finalBillingPeriodEndDate, int documentTypeId, 
                                                                           int invoiceAdvancedArrears, int invoiceInAdvance, bool? isGroupNew)
        {
            if(isGroupNew == true)
            {
                return contractMaintenanceGroupDAL.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, 
                                                                                            firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                            finalBillingPeriodEndDate, documentTypeId,
                                                                                            invoiceAdvancedArrears, invoiceInAdvance);
            }
            else
            {
                return contractMaintenanceGroupDAL.GetContractMaintenanceGroupList(contractId, periodFrequencyId, 
                                                                                   firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                   finalBillingPeriodEndDate, documentTypeId,
                                                                                   invoiceAdvancedArrears, invoiceInAdvance);
            }            
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
                                                 int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            return contractMaintenanceGroupDAL.IsContractMaintenanceGrouped(contractId, periodFrequencyId, 
                                                                            firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                            finalBillingPeriodEndDate, documentTypeId,
                                                                            invoiceAdvancedArrears, invoiceInAdvance);
        }
        

        /// <summary>
        /// Return number of Grouped records in the list
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
            return contractMaintenanceGroupDAL.GetRecordCountOfGroupedElements(contractId, periodFrequencyId, 
                                                                               firstPeriodStartDate,firstRenewalDate, finalBillingPeriodStartDate,
                                                                               finalBillingPeriodEndDate, documentTypeId,
                                                                               invoiceAdvancedArrears, invoiceInAdvance);
        }

        /// <summary>
        /// Return number of Ungrouped records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns></returns>
        public int GetRecordCountOfUngroupedElements(int contractId, int periodFrequencyId, 
                                                     DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                     DateTime? finalBillingPeriodEndDate, int documentTypeId,
                                                     int invoiceAdvancedArrears, int invoiceInAdvance)
        {
            return contractMaintenanceGroupDAL.GetRecordCountOfUngroupedElements(contractId, periodFrequencyId, 
                                                                                 firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                 finalBillingPeriodEndDate, documentTypeId,
                                                                                 invoiceAdvancedArrears, invoiceInAdvance);
        }

        /// <summary>
        /// Gets list of grouped items 
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="groupName">Group Id</param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupName(int contractId, int periodFrequencyId, 
                                                                                      DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                                      DateTime? finalBillingPeriodEndDate, int documentTypeId, 
                                                                                      int invoiceAdvancedArrears, int invoiceInAdvance, string groupName)
        {
            List<ContractMaintenanceVO> contractMaintenanceVOList = new List<ContractMaintenanceVO>();
            List<ContractMaintenanceVO> contractMaintenanceVO = new List<ContractMaintenanceVO>();
            contractMaintenanceVOList = contractMaintenanceGroupDAL.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, groupName);

            foreach (var item in contractMaintenanceVOList)
            {
                contractMaintenanceVO.Add(item);
            }
            contractMaintenanceVOList = contractMaintenanceGroupDAL.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, 
                                                                                                             firstPeriodStartDate,firstRenewalDate, finalBillingPeriodStartDate,
                                                                                                             finalBillingPeriodEndDate, documentTypeId,
                                                                                                             invoiceAdvancedArrears, invoiceInAdvance);
            foreach (var item in contractMaintenanceVOList)
            {
                contractMaintenanceVO.Add(item);
            }

            return contractMaintenanceVO;
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
            return contractMaintenanceGroupDAL.GetContractMaintenanceGroupNameList(contractId, periodFrequencyId, 
                                                                                   firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                   finalBillingPeriodEndDate, documentTypeId,
                                                                                   invoiceAdvancedArrears, invoiceInAdvance); 
        }

        /// <summary>
        /// Gets list of grouped items 
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="groupName">Group Id</param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceGroupListByGroupNameForDefaultLine(int contractId, int periodFrequencyId, string groupName)
        {
            return contractMaintenanceGroupDAL.GetContractMaintenanceGroupListByGroupName(contractId, periodFrequencyId, groupName);
        }

        /// <summary>
        /// Get Ungrouped Items list
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="firstPeriodStartDate"></param>
        /// <param name="finalBillingPeriodEndDate"></param>
        /// <returns></returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceUngroupedItemsList(int contractId, int periodFrequencyId, 
                                                                                    DateTime? firstPeriodStartDate, DateTime? firstRenewalDate, DateTime? finalBillingPeriodStartDate,
                                                                                    DateTime? finalBillingPeriodEndDate,int documentTypeId, 
                                                                                    int invoiceAdvancedArrears, int invoiceInAdvance, string groupName = null)
        {
            return contractMaintenanceGroupDAL.GetContractMaintenanceUngroupedItemsList(contractId, periodFrequencyId, 
                                                                                        firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                        finalBillingPeriodEndDate,documentTypeId,
                                                                                        invoiceAdvancedArrears, invoiceInAdvance, groupName);
        }

        /// <summary>
        /// Validate default Line
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="defaultInGroupId"></param>
        /// <returns></returns>
        public bool IsDefaultLineValid(List<int> Ids, int defaultInGroupId)
        {
            bool validateDefaultLine = false;
            foreach (var id in Ids)
            {
                if (id == defaultInGroupId)
                {
                    validateDefaultLine = true;
                    break;
                }                
            }

            return validateDefaultLine; 
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
            return contractMaintenanceGroupDAL.GetRecordCountofGroupedItemsByGroupId(contractId, periodFrequencyId, groupId);
        }

        /// <summary>
        /// Ungroup the contractmaintenance Group
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="groupId"></param>
        public void UngroupContractMaintenance(int contractId, int periodFrequencyId, int groupId, int? userId)
        {
            contractMaintenanceGroupDAL.UngroupContractMaintenance(contractId, periodFrequencyId, groupId, userId);
        }

        /// <summary>
        /// Ungroup the contractMaintenance item
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenance Id</param>
        /// <param name="userId">user Id</param>
        public void UngroupContractMaintenanceItem(int contractMaintenanceId, int? userId)
        {
            contractMaintenanceGroupDAL.UngroupContractMaintenanceItem(contractMaintenanceId, userId);
        }

        /// <summary>
        /// Get the list of all contract maintenance value object to get default billing lines of 
        /// Contract maintenance  
        /// </summary>
        /// <param name="contractId">The contract id of contract maintenance</param>
        /// <param name="groupId">The groupd id of selected contract maintenance line</param>
        /// <param name="chargedFrequency">The charged frequency of selected contract maintenance line</param>
        public List<ContractMaintenanceVO> GetDefaultLineOfContractMaintenanceGroup(int contractId, int chargedFrequency, int groupId)
        {
            return contractMaintenanceGroupDAL.GetContractMaintenanceGroupListByGroupId(contractId, chargedFrequency, groupId);
        }

        #region Commented Code
        /// <summary>
        /// Return number of records in the list
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="periodFrequencyId">periodFrequency Id</param>
        /// <param name="firstPeriodStartDate">firstPeriodStart Date</param>
        /// <param name="finalBillingPeriodEndDate">Final Billing Period End Date</param>
        /// <returns>record Count</returns>
        //public int GetRecordCount(int contractId, int periodFrequencyId, DateTime? firstPeriodStartDate, DateTime? finalBillingPeriodEndDate)
        //{

        //    return contractMaintenanceGroupDAL.GetRecordCount(contractId, periodFrequencyId, firstPeriodStartDate, finalBillingPeriodEndDate);
        //}
        #endregion
    }
}