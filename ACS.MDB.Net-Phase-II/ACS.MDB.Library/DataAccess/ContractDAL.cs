using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ContractDAL : BaseDAL
    {
        /// <summary>
        /// Get contract details by Id
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>IndexId Details</returns>
        public ContractVO GetContractById(int contractId)
        {
            Contract contract = mdbDataContext.Contracts.SingleOrDefault(c => c.ID == contractId);
            ContractVO contractVO = null;
            if (contract != null)
            {
                contractVO = new ContractVO(contract);
            }
            return contractVO;
        }

        /// <summary>
        /// Gets the list of Contracts
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">invoicecustomer id</param>
        /// <returns>List of contracts</returns>
        public List<ContractVO> GetContractList(int companyId, int? invoiceCustomerId)
        {
            List<Contract> contracts = null;
            if (invoiceCustomerId != -1)
            {
                contracts = mdbDataContext.Contracts.Where(c => c.CompanyID == companyId && c.InvoiceCustomerID == invoiceCustomerId && c.IsDeleted == false).ToList();
            }
            else
            {
                contracts = mdbDataContext.Contracts.Where(c => c.CompanyID == companyId && c.IsDeleted == false).ToList();
            }

            List<ContractVO> contractVOList = new List<ContractVO>();

            List<EndUser> endUsers =
                mdbDataContext.EndUsers.Where(e => e.IsDeleted == false && e.BusinessPartner.CompanyID == companyId)
                              .ToList();

            //Get all invoice customers associated with company
            List<OACustomer> invoiceCustomerList = mdbDataContext.OACustomers.Where(x => x.CompanyID == companyId).ToList();
            int enduserId = 0;
            OACustomer customer = null;

            foreach (var item in contracts)
            {
                ContractVO contractVO = new ContractVO(item);

                //If enduser is not invoice customer
                if (contractVO.EndUserId.StartsWith("E"))
                {
                    EndUser endUser = endUsers.FirstOrDefault(c => c.EndUserTextID == contractVO.EndUserId);
                    if (endUser != null)
                    {
                        contractVO.EndUserName = endUser.EndUserName;
                    }
                }
                else
                {
                    //If enduser is invoice customer
                    if (contractVO.EndUserId != contractVO.InvoiceCustomerId.ToString())
                    {
                        enduserId = Convert.ToInt32(contractVO.EndUserId);
                        customer = invoiceCustomerList.FirstOrDefault(x => x.ID == enduserId);
                        contractVO.EndUserName = (customer != null) ? customer.CustomerName : contractVO.EndUserName;
                    }
                }

                contractVOList.Add(contractVO);
            }

            return contractVOList;
        }

        /// <summary>
        /// Save contact details
        /// </summary>
        /// <param name="contractVO">Value Object Contract</param>
        public void SaveContract(ContractVO contractVO)
        {
            if (contractVO != null)
            {
                Contract contract = null;
                if (contractVO.ID <= 0)
                {
                    //Add new contract
                    contract = new Contract();

                    contract.ContractNumber = contractVO.ContractNumber;
                    contract.CompanyID = contractVO.CompanyId;
                    contract.DivisionID = contractVO.DivisionId;
                    contract.CurrencyID = contractVO.CurrencyId;
                    contract.EndUserID = contractVO.EndUserId;
                    contract.InvoiceCustomerID = contractVO.InvoiceCustomerId;
                    contract.POReferenceNo = contractVO.POReferenceNumber;
                    contract.AtRisk = contractVO.AtRisk;
                    contract.EarlyTerminationDate = contractVO.EarlyTerminationDate;
                    contract.CreationDate = DateTime.Now;
                    contract.CreatedBy = contractVO.CreatedByUserId;

                    mdbDataContext.Contracts.InsertOnSubmit(contract);
                    mdbDataContext.SubmitChanges();

                    //Set newly added contract id
                    if (contractVO.ID == 0)
                    {
                        contractVO.ID = contract.ID;
                    }
                }
                else
                {
                    //Update contract details
                    contract = mdbDataContext.Contracts.Where(c => c.ID == contractVO.ID && c.IsDeleted == false).SingleOrDefault();
                    contract.ContractNumber = contractVO.ContractNumber;
                    contract.CompanyID = contractVO.CompanyId;
                    contract.DivisionID = contractVO.DivisionId;
                    contract.CurrencyID = contractVO.CurrencyId;
                    contract.EndUserID = contractVO.EndUserId;
                    contract.POReferenceNo = contractVO.POReferenceNumber;
                    contract.AtRisk = contractVO.AtRisk;
                    contract.EarlyTerminationDate = contractVO.EarlyTerminationDate;
                    contract.LastUpdatedDate = DateTime.Now;
                    contract.LastUpdatedBy = contractVO.LastUpdatedByUserId;
                    contract.InvoiceCustomerID = contractVO.InvoiceCustomerId;

                    mdbDataContext.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Delete contract and associated details
        /// </summary>
        /// <param name="Ids">Ids of contact to be deleted</param>
        public void DeleteContract(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //Delete contract
                    Contract contract = new Contract();
                    contract = mdbDataContext.Contracts.Where(c => c.ID == id).SingleOrDefault();
                    contract.IsDeleted = true;
                    contract.LastUpdatedDate = DateTime.Now;
                    contract.LastUpdatedBy = userId;

                    //Delete coading (contract lines) lines
                    List<ContractLine> contractLines = mdbDataContext.ContractLines.Where(c => c.ContractID == id && c.IsDeleted == false).ToList();
                    foreach (var contractLine in contractLines)
                    {
                        contractLine.IsDeleted = true;
                        contractLine.LastUpdatedDate = DateTime.Now;
                        contractLine.LastUpdatedBy = userId;
                    }

                    //Delete contract maintenance (Billing Lines) and associated  billing line texts
                    List<ContractMaintenance> contractMaintenance = mdbDataContext.ContractMaintenances.Where(c => c.ContractID == id && c.IsDeleted == false).ToList();
                    foreach (var item in contractMaintenance)
                    {
                        item.IsDeleted = true;
                        item.LastUpdatedDate = DateTime.Now;
                        item.LastUpdatedBy = userId;

                        //Delete billing line tags
                        List<MaintenanceBillingLine> maintenanceBillingLines = mdbDataContext.MaintenanceBillingLines.Where(c => c.ContractMaintenance.ContractID == id && c.IsDeleted == false).ToList();
                        foreach (var maintenanceBillingLine in maintenanceBillingLines)
                        {
                            maintenanceBillingLine.IsDeleted = true;
                            maintenanceBillingLine.LastUpdatedDate = DateTime.Now;
                            maintenanceBillingLine.LastUpdatedBy = userId;
                        }
                    }

                    //Delete milestones and associated  billing line texts
                    List<Milestone> milestones = mdbDataContext.Milestones.Where(c => c.ContractID == id && c.IsDeleted == false).ToList();
                    foreach (var milestone in milestones)
                    {
                        milestone.IsDeleted = true;
                        milestone.LastUpdatedDate = DateTime.Now;
                        milestone.LastUpdatedBy = userId;

                        //Delete billing line tags
                        List<MilestoneBillingLine> milestoneBillingLines = mdbDataContext.MilestoneBillingLines.Where(c => c.ContractID == id && c.IsDeleted == false).ToList();
                        foreach (var milestoneBillingLine in milestoneBillingLines)
                        {
                            milestoneBillingLine.IsDeleted = true;
                            milestoneBillingLine.LastUpdatedDate = DateTime.Now;
                            milestoneBillingLine.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Save the copied contract
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="userId">login userId</param>
        public void SaveCopyContract(ContractVO contractVO,int? userId)
        {
            //ContractVO contractVO = GetContractById(contractId);
            Contract copyContract = new Contract();
            ContractLineDAL contractlineDAL = new ContractLineDAL();
            List<ContractLineVO> contractLineVOList = contractlineDAL.GetContractLineByContractId(contractVO.ID);

            if (contractVO != null)
            {
                string contractNumber = contractVO.ContractNumber;
                if (contractNumber.Length > 20)
                {
                    contractNumber = contractNumber.Substring(0, 20) + "-Copy";
                }
                else
                {
                    contractNumber = contractNumber + "-Copy";
                }

                copyContract.ContractNumber = contractNumber;
                copyContract.CompanyID = contractVO.CompanyId;
                copyContract.DivisionID = contractVO.DivisionId;
                copyContract.CurrencyID = contractVO.CurrencyId;
                copyContract.EndUserID = contractVO.EndUserId;
                copyContract.InvoiceCustomerID = contractVO.InvoiceCustomerId;
                copyContract.POReferenceNo = contractVO.POReferenceNumber;
                copyContract.AtRisk = contractVO.AtRisk;
                copyContract.EarlyTerminationDate = contractVO.EarlyTerminationDate;
                copyContract.CreationDate = DateTime.Now;
                copyContract.CreatedBy = userId;

                mdbDataContext.Contracts.InsertOnSubmit(copyContract);
                mdbDataContext.SubmitChanges();

                //If contractLine list is not empty
                if (contractLineVOList != null)
                {
                    //Set newly added contract id
                    int contractId = copyContract.ID;
                    
                    contractlineDAL.SaveCopyContractLine(contractLineVOList, contractId,userId);
                }
            }

        }

        /// <summary>
        /// to get contract maintenance billing lines based on contract id
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>List of contractVO</returns>
        public List<ContractVO> GetContractMaintenanceDetailsBasedOnContractId(int contractId)
        {
            ContractMaintenanceDAL contractMaintenanceDAL = new ContractMaintenanceDAL();
            ContractVO contractVO = new ContractVO();
            List<ContractVO> contractVOList = new List<ContractVO>();
           
            //to get all maintenance  billing lines based on contract id
            List<ContractMaintenanceVO>  contractMaintenanceVOList = contractMaintenanceDAL.GetContractMaintenanceDetails(contractId);

            //to get all billing lines of a contract
            foreach (ContractMaintenanceVO contractMaintenanceVO in contractMaintenanceVOList)
            {
                //to get billing line tags of a particular billing line
                foreach (MaintenanceBillingLineVO maintenanceBillingLine in contractMaintenanceVO.MaintenanceBillingLineVos)
                {
                    contractVO.MaintenanceBillingLineVOList.Add(maintenanceBillingLine);
                }
                contractVO.ContractMaintenanceVOList.Add(contractMaintenanceVO);
            }
            contractVOList.Add(contractVO);

            return contractVOList;
        }

        #region Commented Code
        /// <summary>
        /// Gets the list of contracts
        /// </summary>
        /// <returns>List of Contracts</returns>
        //public List<ContractVO> GetContractList(int companyId)
        //{
        //    List<Contract> contracts = mdbDataContext.Contracts.Where(c => c.CompanyID == companyId && c.IsDeleted == false).ToList();
        //    List<ContractVO> contractVOList = new List<ContractVO>();

        //    List<EndUser> endUsers =
        //        mdbDataContext.EndUsers.Where(e => e.IsDeleted == false && e.BusinessPartner.CompanyID == companyId)
        //                      .ToList();

        //    foreach (var item in contracts)
        //    {
        //        ContractVO contractVO = new ContractVO(item);

        //        if (contractVO.EndUserId.StartsWith("E"))
        //        {
        //            EndUser endUser = endUsers.FirstOrDefault(c => c.EndUserTextID == contractVO.EndUserId);
        //            if (endUser != null)
        //            {
        //                contractVO.EndUserName = endUser.EndUserName;
        //            }
        //        }

        //        contractVOList.Add(contractVO);
        //    }

        //    return contractVOList;
        //}
        #endregion
    }
}