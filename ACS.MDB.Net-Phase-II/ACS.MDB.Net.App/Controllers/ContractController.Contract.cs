using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;
using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController
    {

        /// <summary>
        /// Gets the list of Contracts
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">invoice customer id</param>
        /// <returns>List of contracts</returns>
        public ActionResult ContractIndex(int? companyId, int? invoiceCustomerId)
        {
            MODEL.Contract contract = new MODEL.Contract();
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                if (userId.HasValue)
                {
                    contract.OAcompanyList = Session.GetUserAssociatedCompanyList();
                   
                    //Company id and invoice Customer id will be available if we are coming from contract details page.
                    //We will select the company for which record is inserted/edited.
                    if (companyId.HasValue)
                    {
                        ViewBag.companyId = companyId;
                        ViewBag.invoiceCutomerId = invoiceCustomerId;

                        //Get all invoice customers associated with the company
                        var invoiceCustomerService = new InvoiceCustomerService();
                        List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(ViewBag.companyId);
                        foreach (var item in invoiceCustomerVOList)
                        {
                            contract.InvoiceCustomerList.Add(new MODEL.InvoiceCustomer(item));
                        }
                    }
                    else
                    {
                        //ViewBag.companyId = -1;
                        ViewBag.companyId = Session.GetDefaultCompanyId();

                        //Get all invoice customers associated with the company
                        var invoiceCustomerService = new InvoiceCustomerService();
                        List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(ViewBag.companyId);
                        foreach (var item in invoiceCustomerVOList)
                        {
                            contract.InvoiceCustomerList.Add(new MODEL.InvoiceCustomer(item));
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
            catch (Exception)
            {
            }
            return View(contract);
        }

        /// <summary>
        /// Gets the list of contracts
        /// </summary>
        /// <param name="param"></param>
        /// <param name="companyId">company Id</param>
        /// <param name="invoiceCustomerId">invoice Customer id</param>
        /// <returns>List of contracts</returns>
        public ActionResult ContractList(MODEL.jQueryDataTableParamModel param, int companyId, int? invoiceCustomerId)
        {
            try
            {
                ContractService contractService = new ContractService();
                //List<ContractVO> contractVOList = contractService.GetContractList(companyId);
                List<ContractVO> contractVOList = contractService.GetContractList(companyId, invoiceCustomerId);

                List<MODEL.Contract> contracts = contractVOList.Select(item => new Models.Contract(item)).ToList();

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetContractOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, contracts, orderingFunction);
                return result;
            }
            catch (Exception exception)
            {
                return new HttpStatusCodeAndErrorResult(500, exception.Message);
            }
        }

        /// <summary>
        /// Save the contract
        /// </summary>
        /// <param name="contract">The contract model object</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContractSave(MODEL.Contract contract)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Valiate the currency
                    ValidateCurrency(contract);

                    //to check whether EarlyTerminationDate is entered when AtRisk is Yes
                    if ((contract.AtRisk==0)|| ((contract.AtRisk == 1) && (contract.EarlyTerminationDate != null)))
                    {
                        //Get user id
                        int? userId = Session.GetUserId();
                        ContractService contractService = new ContractService();
                        //ContractVO contractVO = new ContractVO(contract, userId);

                        ContractVO contractVO = contract.Transpose(userId);

                        contractService.SaveContract(contractVO);

                        //Get customer comment for the selected customer and company                    
                        contract.CustomerComment = GetCustomerComment(contractVO.CompanyId, contractVO.InvoiceCustomerId);
                        //Set the flag to identify if to reload the page.
                        //If true means creating  new contract and reload the page.
                        //Else only update the contract header related details and remain on same page.
                        bool isNewRecord = contract.ID <= 0 ? true : false;
                        //return Json(new List<object>() { contractVO.CompanyId, contractVO.ID, isNewRecord });
                        return Json(new List<object>() { contractVO.CompanyId, contractVO.ID, isNewRecord, contractVO.InvoiceCustomerId, contract.CustomerComment });
                    }
                    else
                    {
                        return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.CANNOT_SAVE, Constants.CONTRACT));
                    }
                }
                else
                {
                    foreach (var item in ModelState)
                    {
                        if (item.Key == "EarlyTerminationDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format("Early Termination Date should be in dd/MM/yyyy format"));
                        }
                    }
                    return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.CANNOT_SAVE, Constants.CONTRACT));
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }
    

        /// <summary>
        /// Create or update contract details
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="contractId">The contract Id to look for, (null if new contract creation)</param>
        /// <param name="showCodingDetailTab">The flag to identify if to display coding line tab or billing details tab</param>
        /// <returns>The contact details</returns>
        public ActionResult ContractDetails(int companyId, int? contractId, int? invoiceCustomerId, bool showCodingDetailTab = false)
        {
            MODEL.Contract contract = new MODEL.Contract();
           
            try
            {
                //If editing contract, then get contract details
                if (contractId.HasValue)
                {
                    ContractService contractService = new ContractService();

                    //Get contract details
                    ContractVO contractVO = contractService.GetContractById(contractId.Value);
                    if (contractVO == null)
                    {
                        ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.CONTRACT));
                    }
                    else
                    {
                        contract = new MODEL.Contract(contractVO, Session.AllowUserToEdit());
                    
                    }
                }
                else
                {
                    //for creating new contract AtRisk= No
                    contract.AtRisk = 0;
                    contract.CompanyId = companyId;
                }

                //Get logged in user associated companies
                contract.OAcompanyList = Session.GetUserAssociatedCompanyList();

                //If new contract line then set company name
                if (!contractId.HasValue)
                {
                    //Set company name
                    var company = contract.OAcompanyList.Find(x => x.ID == companyId);
                    if (company != null)
                    {
                        contract.CompanyName = company.Name;
                    }
                }

                //Get all active divisions associated with the company
                contract.DivisionList = GetDivisionListForContract(companyId);

                //Get all active currency list
                contract.CurrencyList = GetCurrencyList();

                //Get all end users associated with the company
                contract.EndUserList = GetEndUserList(companyId, contract.InvoiceCustomerId);

                //Get all invoice customers associated with the company
                contract.InvoiceCustomerList = GetInvoiceCustomerListForContract(companyId);
                //Get customer comment
                contract.CustomerComment = GetCustomerComment(companyId, contract.InvoiceCustomerId);

                //Set view bag having showCodingDetailTab flag
                ViewBag.showCodingDetailTab = showCodingDetailTab;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(contract);
        }

        /// <summary>
        /// Delete contract and associated details
        /// </summary>
        /// <param name="Ids">Ids of contact to be deleted</param>
        public ActionResult ContractDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractService contractService = new ContractService();
                contractService.DeleteContract(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Save copied contract
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>copy the contract hirechy</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveContractCopy(int contractId)
        {
            try 
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractService contractService = new ContractService();

                ContractVO contractVO = new ContractVO();
                contractVO = contractService.GetContractById(contractId);
                MODEL.Contract contract = new MODEL.Contract(contractVO);
                contractVO = contract.Transpose(userId);

                contractService.SaveCopyContract(contractVO,userId);
                //Get customer comment for the selected customer and company                    
                //contract.CustomerComment = GetCustomerComment(contractVO.CompanyId, contractVO.InvoiceCustomerId);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Get Contract Maintenance Billing Lines based on the Contract ID on image click
        /// </summary>
        /// <param name="param"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public String GetContractMaintenanceDetailsBasedOnContractId(int contractId)
        {
            ContractService contractService = new ContractService();
            MODEL.Contract contract = new MODEL.Contract();

            //to get all maintenance billing lines for a particular contract
            List<ContractVO> contractVOList = contractService.GetContractMaintenanceDetailsByContractId(contractId).ToList();

            foreach (ContractVO contractVO in contractVOList)
            {
                contract.ContractMaintenanceVOList = contractVO.ContractMaintenanceVOList;
                contract.MaintenanceBillingLineVOList = contractVO.MaintenanceBillingLineVOList;
            }

            string billingLines = string.Empty;
            contract.BillingLines = string.Empty;
            contract.BillingLines = @"<table>
                        <tr class='MaintenanceBillingHeader'>
                        <th>&nbsp;&nbsp; Billing Lines</th>
                        <th>&nbsp;&nbsp; Activity Code </th>
                        <th>&nbsp;&nbsp; Job Code</th>
                        <th>&nbsp;&nbsp; Annual Amount</th>
                        <th>&nbsp;&nbsp; Period Amount</th>
                        <th>&nbsp;&nbsp; First Period Start Date</th>
                        <th>&nbsp;&nbsp; Final Billing End Date</th></tr>";

            if (contract.ContractMaintenanceVOList.Count > 0)
            {
                foreach (ContractMaintenanceVO contractMaintenanceVO in contract.ContractMaintenanceVOList)
                {
                    foreach (MaintenanceBillingLineVO maintenanceBillingLine in contractMaintenanceVO.MaintenanceBillingLineVos)
                    {
                        if (!String.IsNullOrEmpty(maintenanceBillingLine.LineText))
                        {
                            if (maintenanceBillingLine.LineText.Contains("<") || maintenanceBillingLine.LineText.Contains(">"))
                            {
                                billingLines += maintenanceBillingLine.LineText.Replace("<", "&lt;").Replace(">", "&gt;") + "<br>";
                            }
                            else
                            {
                                billingLines += maintenanceBillingLine.LineText + "<br>";
                            }
                        }
                    }


                    String firstPeriodStartDate = contractMaintenanceVO.FirstPeriodStartDate.HasValue ? contractMaintenanceVO.FirstPeriodStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : null;
                    String finalBillingEndDate = contractMaintenanceVO.FinalRenewalEndDate.HasValue ? contractMaintenanceVO.FinalRenewalEndDate.Value.Date.ToString(Constants.DATE_FORMAT) : null;
                    if (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
                    {
                        contract.BillingLines += @"<tr class='MaintenanceBillingTR'>"
                                                            + "<td class='BillingLine'><div class='BillingLine1'>" + billingLines + "</div></td>"
                                                            + "<td class='OtherDetails'>" + contractMaintenanceVO.ActivityCode + "-" + contractMaintenanceVO.OAActivityId + "</td>"
                                                            + "<td class='OtherDetails'>" + contractMaintenanceVO.OAJobCodeId + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:right;' >" + String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, contractMaintenanceVO.BaseAnnualAmount) + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:right;' >" + String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, contractMaintenanceVO.FirstPeriodAmount) + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:center;'>" + firstPeriodStartDate + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:center;'>" + finalBillingEndDate + "</td>"
                                            + "</tr>";

                    }
                    //to show Base Annual amount and First Period amount in Red when the amount is negative
                    else
                    {
                        contract.BillingLines += @"<tr class='MaintenanceBillingTR'>"
                                                            + "<td class='BillingLine'><div class='BillingLine1'>" + billingLines + "</div></td>"
                                                            + "<td class='OtherDetails'>" + contractMaintenanceVO.ActivityCode + "-" + contractMaintenanceVO.OAActivityId + "</td>"
                                                            + "<td class='OtherDetails'>" + contractMaintenanceVO.OAJobCodeId + "</td>"
                                                            + "<td class='OtherDetails' style='color:red;text-align:right;'>" + String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, contractMaintenanceVO.BaseAnnualAmount) + "</td>"
                                                            + "<td class='OtherDetails' style='color:red;text-align:right;'>" + String.Format(Constants.STRING_FORMAT_FOR_NUMERIC_VALUE, contractMaintenanceVO.FirstPeriodAmount) + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:center;'>" + firstPeriodStartDate + "</td>"
                                                            + "<td class='OtherDetails' style='text-align:center;'>" + finalBillingEndDate + "</td>"
                                                  + "</tr>";

                    }

                    billingLines = string.Empty;
                }
            }
            else
            {
                contract.BillingLines += @"<tr>
                            <td colspan='7' style='text-align:center;border-right:1px solid #DAD4DA;border-left:1px solid #DAD4DA;' class='NoData'>No data available in the table</td>
                             </tr>";

            }
            contract.BillingLines += "</table>";

            return contract.BillingLines;
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<MODEL.BaseModel, object> GetContractOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 1:
                    sortFunction = obj => ((MODEL.Contract)obj).ContractNumber;
                    break;

                case 2:
                    sortFunction = obj => ((MODEL.Contract)obj).InvoiceCustomer;
                    break;

                case 3:
                    sortFunction = obj => ((MODEL.Contract)obj).EndUser;
                    break;

                case 4:
                    sortFunction = obj => ((MODEL.Contract)obj).POReferenceNumber;
                    break;

                case 5:
                    sortFunction = obj => ((MODEL.Contract)obj).DivisionName;
                    break;

                case 6:
                    sortFunction = obj => ((MODEL.Contract)obj).Currency;
                    break;

                default:
                    //sortFunction = obj => ((MODEL.Contract)obj).ContractNumber;
                    sortFunction = obj => ((MODEL.Contract)obj).ID;
                    break;
            }

            return sortFunction;
        }


        #region Methods

        /// <summary>
        /// Gets the list of Division based on Company Id
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Division List</returns>
        private List<MODEL.Division> GetDivisionListForContract(int? companyId)
        {
            MODEL.Contract contract = new MODEL.Contract();
            //Get all active divisions associated with the company
            DivisionService divisionService = new DivisionService();
            List<DivisionVO> divisionListVO = divisionService.GetDivisionListByCompany(companyId).OrderBy(c => c.DivisionName).ToList();
            foreach (var division in divisionListVO)
            {
                if (division.IsActive)
                {
                    contract.DivisionList.Add(new MODEL.Division(division));
                }
            }

            return (contract.DivisionList);
        }

        /// <summary>
        /// Gets the list of Currency
        /// </summary>       
        /// <returns>Currency List</returns>
        private List<MODEL.Currency> GetCurrencyList()
        {
            MODEL.Contract contract = new MODEL.Contract();

            CurrencyService currencyService = new CurrencyService();
            List<CurrencyVO> currencies = currencyService.GetCurrencyList().OrderBy(c => c.CurrencyName).ToList();
            foreach (var currency in currencies)
            {
                if (currency.IsActive)
                {
                    contract.CurrencyList.Add(new MODEL.Currency(currency));
                }
            }

            return (contract.CurrencyList);
        }

        /// <summary>
        /// Gets a list of end users filtered based on selected company and invoice customer
        /// </summary>
        /// <param name="companyId">The company Id</param>
        /// <param name="invoiceCustomerId">The invoice customer Id</param>
        /// <returns>The List of End Users</returns>
        private List<MODEL.EndUser> GetEndUserList(int companyId, int invoiceCustomerId = 0)
        {
            MODEL.Contract contract = new MODEL.Contract();

            //Get all end users associated with the company
            EndUserService endUserService = new EndUserService();
            List<EndUserVO> endUserVOList = endUserService.GetEndUserList(companyId, invoiceCustomerId).OrderBy(c => c.Name).ToList();
            foreach (var item in endUserVOList)
            {
                contract.EndUserList.Add(new MODEL.EndUser(item));
            }

            return (contract.EndUserList);
        }

        /// <summary>
        /// Gets the list of Invoice customer based on Company Id
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>Invoice Customer List</returns>
        private List<MODEL.InvoiceCustomer> GetInvoiceCustomerListForContract(int companyId)
        {
            MODEL.Contract contract = new MODEL.Contract();

            //Get all invoice customers associated with the company
            InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
            List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(companyId);
            foreach (var item in invoiceCustomerVOList)
            {
                contract.InvoiceCustomerList.Add(new MODEL.InvoiceCustomer(item));
            }
            return (contract.InvoiceCustomerList);
        }

        /// <summary>
        /// Gets the Customer comment list
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="invoiceCustomerId">invoiceCustomer Id</param>
        /// <returns>Customer commnet list</returns>
        private string GetCustomerComment(int companyId, int invoiceCustomerId)
        {
            MODEL.Contract contract = new MODEL.Contract();

            CustomerCommentService customerCommentService = new CustomerCommentService();
            CustomerCommentVO customerCommentVO = customerCommentService.GetCustomerCommentByCompanyAndCutomer(companyId, invoiceCustomerId);
            if (customerCommentVO != null)
            {
                contract.CustomerComment = customerCommentVO.CustomerComment;
            }

            return (contract.CustomerComment);
        }

        /// <summary>
        /// Gets a list of end users filtered based on selected company and invoice customer
        /// </summary>
        /// <param name="companyId">The company Id</param>
        /// <param name="invoiceCustomerId">The invoice customer Id</param>
        /// <returns>The List of End Users</returns>
        public ActionResult GetEndUsersByCompanyAndInvoiceCustomer(int companyId, int invoiceCustomerId)
        {
            try
            {
                return Json(GetEndUserList(companyId, invoiceCustomerId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return new HttpStatusCodeAndErrorResult(500, exception.Message);
            }
        }

        /// <summary>
        /// Get currecny based on invoice customer id
        /// </summary>
        /// <param name="invoiceCustomerId">The invoice customer Id</param>
        /// <returns>The inovice customer value object</returns>
        public ActionResult GetCurrencyByCustomer(int invoiceCustomerId)
        {
            try
            {
                InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
                InvoiceCustomerVO invoiceCustomerVO = invoiceCustomerService.GetCurrencyByCustomer(invoiceCustomerId);
                //CurrencyVO currency = new CurrencyService().GetCurrencyList().FirstOrDefault(x => x.CurrencyName == invoiceCustomerVO.CurrencyId);
                //string currencyName = "does not exist";
                //currencyName = invoiceCustomerVO.CurrencyId;
                return Json(invoiceCustomerVO.CurrencyId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Validate the currency
        /// </summary>
        /// <param name="contract">contract Model</param>
        /// <returns></returns>
        private bool ValidateCurrency(MODEL.Contract contract)
        {
            bool isValidCurrency = false;
            CurrencyVO currency = new CurrencyService().GetCurrencyList().FirstOrDefault(x => x.CurrencyName == contract.Currency);
            if (currency != null)
            {
                isValidCurrency = true;
                contract.CurrencyId = currency.CurrencyID;
            }
            else
            {
                throw new ApplicationException(String.Format(Constants.CURRENCY_NOT_AVAILABLE_FOR_CUSTOMER, contract.Currency));
            }
            return isValidCurrency;
        }

        #endregion Methods

        #region Commented Code

        /// <summary>
        /// Returns contract index view
        /// </summary>
        /// <returns>contract index view</returns>
        /// GET: /ContractController.Contract/ContractIndex
        //public ActionResult ContractIndex(int? companyId)
        //{
        //    MODEL.Contract contract = new MODEL.Contract();
        //    try
        //    {
        //        //Get user id
        //        int? userId = Session.GetUserId();
        //        if (userId.HasValue)
        //        {
        //            contract.OAcompanyList = Session.GetUserAssociatedCompanyList();

        //            //Company id will be available if we are coming from contract details page.
        //            //We will select the company for which record is inserted/edited.
        //            if (companyId.HasValue)
        //            {
        //                ViewBag.companyId = companyId;
        //            }
        //            else
        //            {
        //                //ViewBag.companyId = -1;
        //                ViewBag.companyId = Session.GetDefaultCompanyId();
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Logout", "Login");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return View(contract);
        //}

        /// <summary>
        /// Gets list of contracts for the specified company and selection criteria.
        /// </summary>
        /// <param name="param">The data table search criteria</param>
        /// <param name="companyId">The company for which contracts are required. </param>
        /// <returns>List of contracts</returns>
        /// 
        //public ActionResult ContractList(MODEL.jQueryDataTableParamModel param, int companyId)
        //{
        //    try
        //    {
        //        ContractService contractService = new ContractService();
        //        List<ContractVO> contractVOList = contractService.GetContractList(companyId);

        //        List<MODEL.Contract> contracts = new List<MODEL.Contract>();
        //        foreach (var item in contractVOList)
        //        {
        //            contracts.Add(new Models.Contract(item));
        //        }

        //        //get the field on with sorting needs to happen and set the
        //        //ordering function/delegate accordingly.
        //        int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //        var orderingFunction = GetContractOrderingFunction(sortColumnIndex);

        //        var result = GetFilteredObjects(param, contracts, orderingFunction);
        //        return result;
        //    }
        //    catch (Exception exception)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, exception.Message);
        //    }
        //}
        #endregion Commented Code
    }
}