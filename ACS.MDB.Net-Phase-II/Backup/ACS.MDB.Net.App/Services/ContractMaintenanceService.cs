using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class ContractMaintenanceService
    {
        ContractMaintenanceDAL contractMaintenanceDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractMaintenanceService()
        {
            contractMaintenanceDAL = new ContractMaintenanceDAL();
        }

        /// <summary>
        /// Gets the list of contract Maintenance
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <returns>Contract maintenance List</returns>
        public List<ContractMaintenanceVO> GetContractMaintenanceListbyContractId(int contractId)
        {
            return contractMaintenanceDAL.GetContractMaintenanceListbyContractId(contractId);
        }

        /// <summary>
        /// Get contract maintenance details by Id
        /// </summary>
        /// <param name="contractMaintenanceId">contract maintenanceId id</param>
        /// <returns>Contract Maintenace Details</returns>
        public ContractMaintenanceVO GetContractMaintenanceById(int contractMaintenanceId)
        {
            return contractMaintenanceDAL.GetContractMaintenanceById(contractMaintenanceId);
        }

        /// <summary>
        /// Save contact maintenance details
        /// </summary>
        /// <param name="contractMaintenanceVO">Value Object Contract Maintenance</param>
        public void SaveContractMaintenance(ContractMaintenanceVO contractMaintenanceVO)
        {
            contractMaintenanceDAL.SaveContractMaintenance(contractMaintenanceVO);
        }

        /// <summary>
        /// Get the default billing lines details from respective group
        /// </summary>
        /// <param name="contractId">The contract id of the selected contract maintenance</param>
        /// <param name="chargedFrequency">The charged frequency of selected contract maintenance</param>
        /// <param name="groupId">The group id of the contrct maintenance</param>
        /// <returns>Return the default contract maintenance from the group</returns>
        public ContractMaintenanceVO GetDefaultLineOfContractMaintenanceGroup(int contractId, int chargedFrequency, int groupId)
        {
            ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
            List<ContractMaintenanceVO> contractMaintenanceVos = new List<ContractMaintenanceVO>();

            contractMaintenanceVos = contractMaintenanceGroupService.GetDefaultLineOfContractMaintenanceGroup(
                contractId, chargedFrequency, groupId);

            ContractMaintenanceVO contractMaintenanceVO =
                contractMaintenanceVos.SingleOrDefault(cm => cm.IsDefaultLineInGroup == true);

            return contractMaintenanceVO;

        }

        /// <summary>
        /// Gets the list of contract maintenance
        /// </summary>
        /// <param name="contractLineId">The contract line id</param>
        /// <returns>List of contracts maintenance</returns>
        /// <param name="userId">The logged in user id</param>
        public void DeleteContractMaintenanceList(List<int> Ids, int? userId)
        {
            //MilestoneService milestoneService = new MilestoneService();                 
            //foreach (var id in Ids)
            //{
            //    List<MilestoneVO> milestoneVOList = milestoneService.GetMilestoneList(id, true);
            //    ContractMaintenanceVO contractMaintenanceVO = GetContractMaintenanceById(id);
            //    if (milestoneVOList.Count == 0 && contractMaintenanceVO.IsDefaultLineInGroup != true)
            //    {                    
            //        if (contractMaintenanceVO.IsGrouped == true)
            //        {
            //            var count = 0;
            //            ContractMaintenanceGroupService contractMaintenanceGroupService = new ContractMaintenanceGroupService();
            //            count = contractMaintenanceGroupService.GetRecordCountofGroupedItemsByGroupId(contractMaintenanceVO.ContractId,
            //                                                                                          contractMaintenanceVO.PeriodFrequencyId,
            //                                                                                          contractMaintenanceVO.GroupId.Value);

            //            //count = contractMaintenanceGroupService.GetRecordCountOfGroupedElements(contractMaintenanceVO.ContractId,
            //            //                                                                        contractMaintenanceVO.PeriodFrequencyId,
            //            //                                                                        contractMaintenanceVO.FirstPeriodStartDate,
            //            //                                                                        contractMaintenanceVO.FinalRenewalEndDate);
            //            if (count > 2)
            //            {
            //                contractMaintenanceDAL.DeleteContractMaintenance(contractMaintenanceVO.ID, userId);
            //            }
            //            else if(contractMaintenanceVO.IsDefaultLineInGroup == true)
            //            { 

            //            }

            //        }
            //    }
            //}
            contractMaintenanceDAL.DeleteContractMaintenance(Ids, userId);
        }

        /// <summary>
        /// Add/Update/Delete milestones
        /// </summary>
        /// <param name="maintenanceId">The maintenance id</param>
        /// <param name="milestonesList">The milestone list</param>
        public void AddUpdateDeleteMilestones(int maintenanceId, List<MilestoneVO> milestonesList)
        {
            contractMaintenanceDAL.AddUpdateDeleteMilestones(maintenanceId, milestonesList);
        }

        /// <summary>
        /// Get Invoice in advance
        /// </summary>
        /// <param name="contractId">contract id</param>
        /// <returns>Invoice in advanced from division</returns>
        public int GetInvoiceInAdvanceFromDivision(int contractId)
        {
            return contractMaintenanceDAL.GetInvoiceInAdvanceFromDivision(contractId);
        }

        public List<ContractMaintenanceVO> GetContractMaintenanceForRecalculate(List<int> Ids)
        {
            return contractMaintenanceDAL.GetContractMaintenanceForRecalculate(Ids);
        }

        /// <summary>
        /// Gets the Billing Lines Texts filtered by Contract ID
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>Returns a list of Billing Line Text string</returns>
        public List<string> GetBillingLineTextByContractID(int contractId)
        {
            return contractMaintenanceDAL.GetBillingLineTextByContractID(contractId);
        }

        /// <summary>
        /// Save contractMaintenance Copy
        /// </summary>
        /// <param name="contractMaintenanceVO">contractMaintenanceVO object</param>
        /// <param name="isCreditRecord">to check is Credit record</param>
        /// <param name="userId">login userId</param>
        public void SaveContractMaintenanceCopy(ContractMaintenanceVO contractMaintenanceVO, bool isCreditRecord,int? userId)
        {
            contractMaintenanceDAL.SaveContractMaintenanceCopy(contractMaintenanceVO, isCreditRecord, userId);
        }

        /// <summary>
        /// to know whether the end user is associated with any contract and with any maintenance billing lines
        /// </summary>
        /// <param name="endUserId"></param>
        /// <returns>boolean</returns>
        public bool GetMaintenanceBillingLine(string endUserId)
        {
            return contractMaintenanceDAL.GetMaintenanceBillingLines(endUserId);
        }

        #region Commented code
        ///// <summary>
        ///// Gets the list of contract maintenance
        ///// </summary>
        ///// <param name="contractLineId">The contract line id</param>
        ///// <param name="activityCodeId">The activity code id</param>
        ///// <returns>List of contracts maintenance</returns>
        //public List<ContractMaintenanceVO> GetContractMaintenanceList(int contractLineId, int activityCodeId)
        //{
        //    return contractMaintenanceDAL.GetContractMaintenanceList(contractLineId, activityCodeId);
        //}

        ///// <summary>
        ///// Gets the Contract maintenance list by contract line id.
        ///// </summary>
        ///// <param name="contractLineId">Contract Line id</param>
        ///// <returns>Contract maintenance List</returns>
        //public List<ContractMaintenanceVO> GetContractMaintenanceListbyContractLineId(int contractLineId)
        //{
        //    return contractMaintenanceDAL.GetContractMaintenanceListbyContractLineId(contractLineId);
        //}
        #endregion
    }
}