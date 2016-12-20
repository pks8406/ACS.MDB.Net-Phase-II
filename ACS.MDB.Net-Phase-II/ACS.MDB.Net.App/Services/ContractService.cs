using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;



namespace ACS.MDB.Net.App.Services
{
    public class ContractService : BaseService
    {
        ContractDAL contractDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractService()
        {
            contractDAL = new ContractDAL();
        }

        /// <summary>
        /// Get contract details by Id
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>IndexId Details</returns>
        public ContractVO GetContractById(int contractId)
        {
            return contractDAL.GetContractById(contractId);
        }

        /// <summary>
        /// Get contracts based on provided search criteria
        /// </summary>
        /// <param name="searchBy">The option by which to provide search - By contract number, By Invoice customer,By company OR By End user</param>
        /// <param name="searchText">The text to search for</param>
        /// <returns>The List of Contracts model objects</returns>
        /// <summary>
        //public List<ContractVO> GetContractList(int companyId)
        //{
        //    List<ContractVO> contractVOList = contractDAL.GetContractList(companyId);

        //    //InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
        //    //DivisionService divisionService = new DivisionService();
        //    //CurrencyService currencyService = new CurrencyService();
        //    //EndUserService endUserService = new EndUserService();

        //    //Hashtable invoiceCustomers = invoiceCustomerService.GetInvoiceCustomersByCompanyId(companyId);
        //    //Hashtable divisions = divisionService.GetDivisionsByCompanyId(companyId);
        //    //Hashtable currencys = currencyService.GetCurrencyNames();

        //    //foreach (var item in contractVOList)
        //    //{
        //    ////    item.InvoiceCustomerName = item.InvoiceCustomerId != null ?
        //    ////        (invoiceCustomers.ContainsKey(item.InvoiceCustomerId) ? invoiceCustomers[item.InvoiceCustomerId].ToString() : string.Empty) :
        //    ////        string.Empty;

        //    //    item.EndUserName = endUserService.GetEndUserNameByID(item.EndUserId, companyId);


        //    ////    item.DivisionName = divisions.ContainsKey(item.DivisionId) ? divisions[item.DivisionId].ToString() : string.Empty;
        //    ////    item.Currency = currencys.ContainsKey(item.CurrencyId) ? currencys[item.CurrencyId].ToString() : string.Empty;
        //    //}

        //    return contractVOList;
        //}

        /// <summary>
        /// Gets the list of Contracts
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">invoicecustomer id</param>
        /// <returns>List of contracts</returns>
        public List<ContractVO> GetContractList(int companyId, int? invoiceCustomerId)
        {
            return contractDAL.GetContractList(companyId, invoiceCustomerId);
        }

        /// <summary>
        /// Save contract details
        /// </summary>
        /// <param name="contract">The contract value object</param>
        public void SaveContract(ContractVO contract)
        {
            if (contract != null)
            {
                contractDAL.SaveContract(contract);
            }
        }

        /// <summary>
        /// Delete contract and associated details
        /// </summary>
        /// <param name="Ids">Ids of contact to be deleted</param>
        public void DeleteContract(List<int> Ids, int? userId)
        {
            contractDAL.DeleteContract(Ids, userId);
        }

        /// <summary>
        /// Save the copied contract
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <param name="userId">login userId</param>
        public void SaveCopyContract(ContractVO contractVO,int? userId)
        {
            contractDAL.SaveCopyContract(contractVO,userId);
        }

        /// <summary>
        /// Contract Maintenance Billing Lines based on Contract ID
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public List<ContractVO> GetContractMaintenanceDetailsByContractId(int contractId)
        {
            return contractDAL.GetContractMaintenanceDetailsBasedOnContractId(contractId);
        }
    }
}