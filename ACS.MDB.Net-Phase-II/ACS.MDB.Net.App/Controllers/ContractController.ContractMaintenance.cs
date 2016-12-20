using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController
    {
        #region Globle variables for generation of milestones

        /// <summary>
        /// Globle variables for Generating Milestones
        /// </summary>
        private decimal upliftReturn;

        private double upliftCountDivisor, upliftCount;
        private DateTime upliftDateFrom, upliftDateTo;
        private decimal uplift, upliftFixed, upliftFinal, previousUplift;
        private decimal iPreIndex, iCurrentIndex, dPercentage;
        private int upliftId, iLoop;
        private bool isUsedIndex; // this variable is use to check if the value is updated or not

        #endregion Globle variables for generation of milestones

        /// <summary>
        /// Returns contract maintenance index view
        /// </summary>
        /// <returns>contract index view</returns>
        /// GET: /ContractController.Contract/ContractIndex
        public ActionResult ContractMaintenanceIndex()
        {
            return View();
        }

        /// <summary>
        /// Gets contract maintenance list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="contractId">contract id</param>
        /// <returns>Contract maintenance list</returns>
        public ActionResult ContractMaintenanceList(MODEL.jQueryDataTableParamModel param, int contractId)
        {
            try
            {
                ContractMaintenanceService contractManitenanceService = new ContractMaintenanceService();
                List<MODEL.ContractMaintenance> contractMaintenance = new List<MODEL.ContractMaintenance>();
                List<ContractMaintenanceVO> contractVOList = new List<ContractMaintenanceVO>();

                //Get contract maintenance by contract id
                contractVOList = contractManitenanceService.GetContractMaintenanceListbyContractId(contractId);
                foreach (var item in contractVOList)
                {
                    contractMaintenance.Add(new Models.ContractMaintenance(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjectsForContractMaintenance(param, contractMaintenance, orderingFunction);
                //var result = GetFilteredObjects(param, contractMaintenance);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Returns a partial view that is used to create a new contract maintenance line
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="contractId">contract id</param>
        /// <returns></returns>
        public ActionResult ContractMaintenanceCreate(int companyId, int contractId, int invoiceCustomerId)
        {
            try
            {
                MODEL.ContractMaintenance contractMaintenance = new MODEL.ContractMaintenance();

                ContractService contractService = new ContractService();
                ContractVO contractVO = new ContractVO();
                //to get contract details based on contract id
                contractVO = contractService.GetContractById(contractId);

                contractMaintenance.InflationIndexList = GetInflationIndexList();
                contractMaintenance.ContractLineList = GetContractLineList(contractId);
                contractMaintenance.InvoiceAdvancedList = GetInvoiceAdvancedArrears();
                contractMaintenance.InvoiceAdvancedValueList = GetInvoiceAdvancedValue();
                contractMaintenance.ProductList = GetProductList();
                contractMaintenance.ProductId = 1;
                contractMaintenance.QTY = 1;
                contractMaintenance.SubProductList = GetSubproductList(contractMaintenance.ProductId);
                contractMaintenance.SubProductId = 1;
                contractMaintenance.AuditReasonList = GetReasonCodeList();
                contractMaintenance.ChargingFrequencyList = GetChargeFrequencyList();
                contractMaintenance.ContractId = contractId;
                contractMaintenance.CompanyId = companyId;
                contractMaintenance.InvoiceAdvancedArrears = 1;
                contractMaintenance.InvoiceInAdvance = GetInvoiceInAdvanceFromDivision(contractId);
                contractMaintenance.CreationDate = System.DateTime.Now;
                contractMaintenance.IncludeInForecast = 0;
                contractMaintenance.DocumentTypeId = 1;
                //ARBS-98- to display Activation Date as default and Delete Date should not be defaulted
                contractMaintenance.ReasonDate = System.DateTime.Now;
                //contractMaintenance.DeleteDate = System.DateTime.Now;
                contractMaintenance.CustomerComment = GetCustomerCommentForContractmaintenance(companyId, invoiceCustomerId);

                //ARBS-97 to display end username when invoice customer and end user are different
                if (contractVO.InvoiceCustomerId.ToString() != contractVO.EndUserId)
                {
                    EndUserService endUserService = new EndUserService();
                    //to get end user name based on Contract
                    string endUserName = endUserService.GetEndUserName(contractVO.EndUserId);
                    //to display end user name in the billing line tag
                    contractMaintenance.billingText1 = "End User: " + endUserName;
                }

                return PartialView("ContractMaintenanceDetails", contractMaintenance);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit Contract Maintenance by id
        /// </summary>
        /// <param name="id">Contract Maintenance Id</param>
        /// <returns>Contract Maintenance details view</returns>
        public ActionResult ContractMaintenanceEdit(int id)
        {
            MODEL.ContractMaintenance contractMaintenance = new MODEL.ContractMaintenance();

            try
            {
                //Get contractLine details
                ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                ContractMaintenanceVO contractMaintenanceVO = contractMaintenanceService.GetContractMaintenanceById(id);

                if (contractMaintenance == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.CONTRACT_MAINTENANCE_LINE));
                }
                else
                {
                    contractMaintenance = new MODEL.ContractMaintenance(contractMaintenanceVO);
                    UserService userService = new UserService();
                    // to get email id of user who updated the record
                    string emailId = userService.GetUserEmailIdById(contractMaintenanceVO.LastUpdatedByUserId);
                    contractMaintenance.LastUpdatedByEmailId = emailId;
                    //if ((contractMaintenance.IncludeInForecast == 1) || (contractMaintenance.IncludeInForecast == -1))
                    //{
                    //    contractMaintenance.ReasonDate = System.DateTime.Now;
                    //}
                    contractMaintenance.ContractLineList = GetContractLineList(contractMaintenanceVO.ContractId);
                    contractMaintenance.InflationIndexList = GetInflationIndexList();
                    contractMaintenance.InvoiceAdvancedList = GetInvoiceAdvancedArrears();
                    contractMaintenance.InvoiceAdvancedValueList = GetInvoiceAdvancedValue();
                    contractMaintenance.ProductList = GetProductList();
                    contractMaintenance.SubProductList = GetSubproductList(contractMaintenanceVO.ProductId);
                    contractMaintenance.AuditReasonList = GetReasonCodeList();
                    contractMaintenance.ChargingFrequencyList = GetChargeFrequencyList();
                    contractMaintenance.CustomerComment = GetCustomerCommentForContractmaintenance(contractMaintenance.CompanyId, contractMaintenance.InvoiceCustomerId);
                    //FillMaintenanceBillingLines(contractMaintenance, contractMaintenanceVO.MaintenanceBillingLineVos);
                    FillMaintenanceBillingLines(contractMaintenance, contractMaintenance.MaintenanceBillingLines);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("ContractMaintenanceDetails", contractMaintenance);
        }

        /// <summary>
        /// Save contact maintenance details
        /// </summary>
        /// <param name="contractMaintenanceVO">Value Object Contract Maintenance</param>
        [ValidateInput(false)]
        public ActionResult ContractMaintenanceSave(ContractMaintenance contractMaintenance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Validate user input
                    ValidateInput(contractMaintenance);

                    //Get user id
                    int? userId = Session.GetUserId();
                    ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                    //ContractMaintenanceVO contractMaintenanceVO = new ContractMaintenanceVO(contractMaintenance, userId);
                    ContractMaintenanceVO contractMaintenanceVO = contractMaintenance.Transpose(userId);

                    foreach (var maintenanceBillingLine in contractMaintenanceVO.MaintenanceBillingLineVos)
                    {
                        if (!String.IsNullOrEmpty(maintenanceBillingLine.LineText) && !ValidateBillingLine(maintenanceBillingLine.LineText))
                        {
                            throw new ApplicationException(String.Format(Constants.INVALID_BILLING_TEXT, maintenanceBillingLine.LineSequance));
                        }
                    }

                    //foreach (MDB.Net.App.DataAccess.LINQ.MaintenanceBillingLine maintenanceBillingLine in contractMaintenanceVO.billingLinesToSave)
                    //{
                    //    if (!String.IsNullOrEmpty(maintenanceBillingLine.LineText) && !ValidateBillingLine(maintenanceBillingLine.LineText))
                    //    {
                    //        throw new ApplicationException(String.Format(Constants.INVALID_BILLING_TEXT, maintenanceBillingLine.LineSequance));
                    //    }
                    //}

                    contractMaintenanceService.SaveContractMaintenance(contractMaintenanceVO);

                    int contractMaintenanceId = contractMaintenanceVO.ID;
                    //Generate Milestones after saving billng line
                    if (contractMaintenanceVO.IncludeInForecast == 0)
                    {
                        GenerateMilestones(contractMaintenanceId);
                    }

                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    foreach (var item in ModelState)
                    {
                        if (item.Key == "FirstPeriodStartDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "First Period Start Date"));
                        }
                        if (item.Key == "FirstRenewalDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "First Renewal Date"));
                        }
                        if (item.Key == "FinalRenewalStartDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Final billing period Start Date"));
                        }
                        if (item.Key == "FinalRenewalEndDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Final billing period End Date"));
                        }
                        if (item.Key == "FirstAnnualUpliftDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "First Annual Uplift Date"));
                        }
                        if (item.Key == "ReasonDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Activation Date"));
                        }
                        if (item.Key == "CreationDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Create Date"));
                        }
                        if (item.Key == "DeleteDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Termination Date"));
                        }
                        if (item.Key == "ForecastBillingStartDate" && item.Value.Errors.Count > 0)
                        {
                            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.INVALID_DATE_FOR, "Termination Date"));
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
        /// Get Subproduct List based on product id
        /// </summary>
        /// <param name="productId">product Id</param>
        /// <returns>Jsonresult having list of subproducts</returns>
        public JsonResult GetSubproductListAsJson(int productId)
        {
            try
            {
                SubProductService subProductService = new SubProductService();
                List<SubProductVO> subProductVO = subProductService.GetSubProductListById(productId);
                return Json(subProductVO);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete contract Maintenance and associated details
        /// </summary>
        /// <param name="Ids">Ids of contact maintenance to be deleted</param>
        public ActionResult ContractMaintenanceDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                contractMaintenanceService.DeleteContractMaintenanceList(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Generate milestones for selected maintenance line.
        /// </summary>
        /// <param name="id">The maintenance id</param>
        public ActionResult GenerateMilestones(int id)
        {
            #region Local Variables

            List<MilestoneVO> milestones = new List<MilestoneVO>();
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

            #endregion Local Variables

            try
            {
                //Get Maintanance object
                ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                ContractMaintenanceVO contractMaintenanceVO = contractMaintenanceService.GetContractMaintenanceById(id);

                if (contractMaintenanceVO != null)
                {
                    //Set inflation fixed additional
                    if (contractMaintenanceVO.InflationFixedAdditional.HasValue)
                    {
                        contractMaintenanceVO.InflationFixedAdditional = contractMaintenanceVO.InflationFixedAdditional / 100;
                    }

                    //Get if inflation index associated with billing details, if yes get it's UseIndex value and set in isUsedIndex.
                    if (contractMaintenanceVO.InflationIndexId.HasValue && contractMaintenanceVO.InflationIndexId.Value > 0)
                    {
                        InflationIndexService inflationIndexService = new InflationIndexService();
                        InflationIndexVO inflationIndexVO = inflationIndexService.GetInflationIndexById(contractMaintenanceVO.InflationIndexId.Value);
                        isUsedIndex = inflationIndexVO.UseIndex;
                    }

                    //Set start date, last date, start amount, next amount
                    SetStartDate(contractMaintenanceVO, ref currentDate, ref renewalEndDate, ref currentDate1, ref renewalEndDate1);
                    currentEndDate = SetLastDate(contractMaintenanceVO);
                    SetStartAmount(contractMaintenanceVO, ref previousAmount, ref currentAmount, ref previousAmount1, ref currentAmount1);
                    SetNextAmount(contractMaintenanceVO, isUsedIndex, ref currentDate, ref renewalEndDate, ref previousAmount, ref currentAmount);

                    //Get all active milestones for selected billing details
                    MilestoneService milestoneService = new MilestoneService();
                    milestones = milestoneService.GetMilestoneList(id);

                    //Get user id
                    int? userId = Session.GetUserId();

                    //Add/Update first milestone
                    AddFirstMilestone(contractMaintenanceVO, currentDate1, renewalEndDate1, currentAmount1, milestones, previousAmount, userId);

                    //loop through till current date is less then current end date
                    while (currentDate.Value < currentEndDate.Value)
                    {
                        MilestoneVO milestoneVO = milestones.Find(x => x.RenewalStartDate == currentDate);
                        if (milestoneVO == null)
                        {
                            //Add milestone
                            milestones.Add(AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId));
                        }
                        else
                        {
                            //Edit milestone
                            if (milestoneVO.MilestoneStatusID == Convert.ToUInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING) || String.IsNullOrEmpty(milestoneVO.MilestoneStatusName))
                            {
                                //Edit milestone
                                EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                            }
                            else
                            {
                                //Get amount so, all generated milestones below this will use this amount
                                currentAmount = milestoneVO.Amount;
                                currentDate = milestoneVO.RenewalStartDate;
                            }
                        }

                        //Set next date, next amount for next milestone
                        SetNextDate(contractMaintenanceVO, ref currentDate, ref renewalEndDate, ref previousDate);
                        SetNextAmount(contractMaintenanceVO, isUsedIndex, ref currentDate, ref renewalEndDate, ref previousAmount, ref currentAmount);
                    }

                    //Delete milestones whose renewal start date is greater then current date after generation of all invoices
                    foreach (var item in milestones.FindAll(x => x.RenewalStartDate.Value >= currentDate.Value
                        && x.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING) &&
                        (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                        && contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))))
                    {
                        item.IsDeleted = true;
                        item.LastUpdatedByUserId = userId;
                    }

                    //Set last date. This will be used to identify that invoices are to be stooped on given final billing period start date.
                    DateTime? lastDate = GetLastMilestoneDate(previousDate, milestones, currentDate);

                    if (lastDate == null || lastDate < contractMaintenanceVO.FinalRenewalStartDate)
                    {
                        //Add last milestone
                        AddORUpdateLastMilestone(contractMaintenanceVO, currentDate,
                                                previousDate, renewalEndDate, currentAmount,
                                                previousAmount, userId, null, milestones);
                    }
                    else
                    {
                        MilestoneVO milestoneVO = milestones.Find(x => x.RenewalStartDate == contractMaintenanceVO.FinalRenewalStartDate);
                        if (milestoneVO != null)
                        {
                            //Edit last milestone
                            AddORUpdateLastMilestone(contractMaintenanceVO, currentDate,
                                            previousDate, renewalEndDate, currentAmount,
                                         previousAmount, userId, milestoneVO, milestones);
                        }
                    }

                    //Calculate billing line text
                    foreach (MilestoneVO milestoneVO in milestones)
                    {
                        CalculateBillingLineText(milestoneVO, contractMaintenanceVO);
                    }

                    //Add/Update/Delete milestones
                    contractMaintenanceService.AddUpdateDeleteMilestones(contractMaintenanceVO.ID, milestones);
                }
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// to know whether the end user has any contract or billing line
        /// </summary>
        /// <param name="endUserId"></param>
        /// <returns></returns>
        public bool GetMaintenanceBillingLineStatus(string endUserId)
        {
            ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
            return contractMaintenanceService.GetMaintenanceBillingLine(endUserId);
        }

        /// <summary>
        /// Save ContractMaintenance Copy
        /// </summary>
        /// <param name="contractMaintenanceId">contractMaintenance Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveContractMaintenanceCopy(int contractMaintenanceId, bool isCreditRecord)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
                ContractMaintenanceVO contractMaintenanceVO = new ContractMaintenanceVO();
                contractMaintenanceVO = contractMaintenanceService.GetContractMaintenanceById(contractMaintenanceId);
                //userId:-to pass login userId as CreatedBy
                contractMaintenanceService.SaveContractMaintenanceCopy(contractMaintenanceVO, isCreditRecord, userId);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<MODEL.BaseModel, object> GetContractMaintenanceOrderingFunction(int sortCol)
        {
            Func<MODEL.BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).ActivityCode;
                    break;

                case 3:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).OAJobCodeId;
                    break;

                case 4:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).BaseAnnualAmount;
                    break;

                case 5:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).PeriodFrequency;
                    break;

                case 6:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).FirstPeriodAmount;
                    break;

                case 7:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).FirstPeriodStartDate;
                    break;

                case 8:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).FirstRenewalDate;
                    break;

                case 9:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).FinalRenewalStartDate;
                    break;
                case 10:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).FinalRenewalEndDate;
                    break;
                case 11:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).EndAmount;
                    break;

                case 15:
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).QTY;
                    break;

                default:
                    //sortFunction = obj => ((MODEL.Contract)obj).ContractNumber;
                    sortFunction = obj => ((MODEL.ContractMaintenance)obj).ID;
                    break;
            }

            return sortFunction;
        }

        /// <summary>
        /// Function returns a list of objects filtered as per the specified
        /// search criteria. Used for populating/searching the data table grid
        /// </summary>
        /// <param name="param">The search parameters (criteria)</param>
        /// <param name="objects">The original list of entities</param>
        /// <returns>List of filtered entities</returns>
        protected JsonResult GetFilteredObjectsForContractMaintenance(jQueryDataTableParamModel param, IEnumerable<BaseModel> objects,
                                                Func<BaseModel, object> orderingFunction = null)
        {
            IEnumerable<BaseModel> filteredObjects;
            Func<MODEL.BaseModel, object> sortFunctionForGroupName = obj => ((MODEL.ContractMaintenance)obj).GroupName;
            Func<MODEL.BaseModel, object> sortFunctionForGroupId = obj => ((MODEL.ContractMaintenance)obj).GroupId;

            //filter the object based on search criteria
            if (String.IsNullOrEmpty(param.sSearch))
            {
                filteredObjects = objects;
            }
            else
            {
                filteredObjects = objects.Where(obj => obj.Contains(param.sSearch)).ToList<BaseModel>();
            }

            //order the object if order function  is specified
            if (orderingFunction != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];

                //If sort column (ID column) index is 0 then get the records in descending order by ID
                //This will happen if user has not done sorting on any columns and we want to dispaly data in desc order.
                if (sortColumnIndex == 0)
                {
                    filteredObjects = filteredObjects.OrderBy(sortFunctionForGroupId).ThenBy(sortFunctionForGroupName).ThenBy(orderingFunction);
                }
                else
                {
                    if (sortDirection == "asc")
                    {
                        filteredObjects = filteredObjects.OrderBy(sortFunctionForGroupId).ThenBy(sortFunctionForGroupName).ThenBy(orderingFunction);
                    }
                    else
                    {
                        filteredObjects = filteredObjects.OrderBy(sortFunctionForGroupId).ThenBy(sortFunctionForGroupName).ThenByDescending(orderingFunction);
                    }
                }
            }

            IEnumerable<BaseModel> displayedObjects = filteredObjects.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from obj in displayedObjects select obj.GetModelValue();
            var tableList = new
            {
                sEcho = param.sEcho,
                iTotalRecords = filteredObjects.Count(),
                iTotalDisplayRecords = filteredObjects.Count(),
                aaData = result
            };

            return Json(tableList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the Billing Lines Texts filtered by Contract ID
        /// </summary>
        /// <param name="contractId">contract Id</param>
        /// <returns>Json result having a List of Billing Line Texts</returns>
        public JsonResult GetBillingLineTextByContractID(int contractId)
        {
            try
            {
                ContractMaintenanceService contractManitenanceService = new ContractMaintenanceService();
                List<string> billingLineTexts = contractManitenanceService.GetBillingLineTextByContractID(contractId);
                return Json(billingLineTexts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ApplicationException(e.Message));
            }
        }

        #region Private Methods

        /// <summary>
        /// Gets the list of inflation index
        /// </summary>
        /// <returns>Inflation Index List</returns>
        private List<InflationIndexVO> GetInflationIndexList()
        {
            InflationIndexService inflationIndexService = new InflationIndexService();
            List<InflationIndexVO> inflationIndexVOlist = inflationIndexService.GetInflationIndexList().OrderBy(c => c.Description).ToList();
            return inflationIndexVOlist;
        }

        /// <summary>
        /// Get the list of products
        /// </summary>
        /// <returns>List of products</returns>
        private List<ProductVO> GetProductList()
        {
            ProductService productService = new ProductService();
            List<ProductVO> productVOList = productService.GetProductList();
            return productVOList;
        }

        /// <summary>
        /// Get Sub product List based on product id
        /// </summary>
        /// <param name="productId">product Id</param>
        /// <returns>Json result having list of sub products</returns>
        private List<SubProductVO> GetSubproductList(int productId)
        {
            SubProductService subProductService = new SubProductService();
            return subProductService.GetSubProductListById(productId);
        }

        /// <summary>
        /// Get the list of invoice advanced arrears
        /// </summary>
        /// <returns>List of invoice advanced arrears</returns>
        private List<MODEL.InvoiceAdvanced> GetInvoiceAdvancedArrears()
        {
            List<InvoiceAdvanced> invoiceAdvancedList = new List<InvoiceAdvanced>();
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 0, Value = "Invoice on Renewal Date" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 1, Value = "Invoice in Advance" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 2, Value = "Invoice in Arrears" });
            return invoiceAdvancedList;
        }

        /// <summary>
        /// Get the list of invoice in advanced 
        /// </summary>
        /// <returns>List of invoice in advanced </returns>
        private List<MODEL.InvoiceAdvanced> GetInvoiceAdvancedValue()
        {
            List<InvoiceAdvanced> invoiceAdvancedList = new List<InvoiceAdvanced>();
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 0, Value = "0" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 1, Value = "1" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 2, Value = "2" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 3, Value = "3" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 4, Value = "4" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 5, Value = "5" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 6, Value = "6" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 7, Value = "7" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 8, Value = "8" });
            invoiceAdvancedList.Add(new InvoiceAdvanced() { ID = 9, Value = "9" });
            return invoiceAdvancedList;
        }

        /// <summary>
        /// Get the audit reason List
        /// </summary>
        /// <returns>Value object of audit reason List</returns>
        private List<AuditReason> GetReasonCodeList()
        {
            AuditReasonService auditReasonService = new AuditReasonService();

            List<AuditReasonVO> auditReasonVos = auditReasonService.GetReasonCodeList();

            List<AuditReason> auditReasons = new List<AuditReason>();

            foreach (var auditReason in auditReasonVos)
            {
                auditReasons.Add(new AuditReason(auditReason));

            }
            return auditReasons;
        }

        /// <summary>
        /// Get the contract line List
        /// </summary>
        /// <param name="ContractId">contract Id</param>
        /// <returns>Contract line List</returns>
        private List<ContractLine> GetContractLineList(int ContractId)
        {
            List<MODEL.ContractLine> contracLineList = new List<ContractLine>();
            ContractLineService contractLineService = new ContractLineService();
            List<ContractLineVO> contractLineVOList = new List<ContractLineVO>();
            contractLineVOList = contractLineService.GetContractLineByContractId(ContractId);

            if (contractLineVOList != null)
            {
                foreach (var item in contractLineVOList)
                {
                    //contracLineList.Add(new ContractLine() {ID = item.ContractLineID, ContractLineDetails = item.ContractLineDetails });
                    contracLineList.Add(new ContractLine(item));
                }
            }

            return contracLineList;
        }

        /// <summary>
        /// This method used to get charing frequency list.
        /// </summary>
        /// <returns></returns>
        private List<ChargingFrequencyVO> GetChargeFrequencyList()
        {
            ChargingFrequencyService chargingFrequencyService = new ChargingFrequencyService();
            return chargingFrequencyService.GetChargingFrequencyList();
        }

        /// <summary>
        /// Validate the input.
        /// </summary>
        /// <param name="contractMaintenance">contractmaintenance model object</param>
        private void ValidateInput(ContractMaintenance contractMaintenance)
        {
            if (contractMaintenance.FirstPeriodStartDate.HasValue && !contractMaintenance.FirstRenewalDate.HasValue)
            {
                throw new ApplicationException(Constants.RENEWALDATE_REQUIRED);
            }
            else if (contractMaintenance.FirstPeriodStartDate > contractMaintenance.FirstRenewalDate)
            {
                throw new ApplicationException(Constants.FIRST_PERIOD_START_DATE_CANNOT_BE_GREATER_THAN_RENEWAL_DATE);
            }
            //else if (contractMaintenance.FinalRenewalStartDate.HasValue && !contractMaintenance.FinalRenewalEndDate.HasValue)
            //{
            //    throw new ApplicationException(Constants.FINAL_BILLIING_RENEWAL_END_DATE_REQUIRED);
            //}
            //else if (!contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue)
            //{
            //    throw new ApplicationException(Constants.FINAL_BILLIING_RENEWAL_START_DATE_REQUIRED);
            //}
            //else if (contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue && !contractMaintenance.EndAmount.HasValue)
            //{
            //    throw new ApplicationException(Constants.FINAL_BILLIING_AMOUNT_REQUIRED);
            //}
            else if (contractMaintenance.FinalRenewalStartDate > contractMaintenance.FinalRenewalEndDate)
            {
                throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_CANNOT_BE_GREATER_THAN_fINAL_RENEWAL_END_DATE);
            }
            else if (contractMaintenance.FirstAnnualUpliftDate.HasValue && !contractMaintenance.FirstRenewalDate.HasValue)
            {
                throw new ApplicationException(Constants.RENEWALDATE_REQUIRED);
            }
            else if (contractMaintenance.UpliftRequired)
            {
                if (contractMaintenance.IncludeInForecast == 0)
                {
                    if (!contractMaintenance.FirstAnnualUpliftDate.HasValue)
                    {
                        throw new ApplicationException(Constants.FIRST_ANNUAL_UPLIFT_DATE_REQUIRE);
                    }
                    else if (!contractMaintenance.InflationIndexId.HasValue)
                    {
                        throw new ApplicationException(Constants.INFLATION_INDEX_REQUIRE);
                    }
                    //else if (!contractMaintenance.InflationFixedAdditional.HasValue)
                    //{
                    //    throw new ApplicationException(Constants.ADDITIONAL_FIXED_REQUIRE);
                    //}
                }
            }
            else if ((contractMaintenance.IncludeInForecast == -1) && (!contractMaintenance.ForecastBillingStartDate.HasValue))
            {
                throw new ApplicationException("Please enter Forecast Billing Start Date");
            }
            else if ((contractMaintenance.IncludeInForecast == 1) && (!contractMaintenance.ForecastBillingStartDate.HasValue))
            {
                throw new ApplicationException("Please enter Forecast Billing Start Date");
            }

            //If charge frequency is credit or ad-hoc then final renewal start,end date and amount are require
            if ((contractMaintenance.PeriodFrequencyId == 5 || contractMaintenance.PeriodFrequencyId == 6) && (contractMaintenance.IncludeInForecast == 0))
            {
                if (contractMaintenance.FinalRenewalStartDate.HasValue && !contractMaintenance.FinalRenewalEndDate.HasValue && contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED_FOR_CREDIT_OR_ADHOC);
                }
                else if (!contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue && contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED_FOR_CREDIT_OR_ADHOC);
                }
                else if (contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue && !contractMaintenance.EndAmount.HasValue && contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED_FOR_CREDIT_OR_ADHOC);
                }
                else if (contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue && contractMaintenance.EndAmount.HasValue && contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED_FOR_CREDIT_OR_ADHOC);
                }

                if (!contractMaintenance.FinalRenewalStartDate.HasValue && !contractMaintenance.FinalRenewalEndDate.HasValue && contractMaintenance.EndAmount.HasValue && contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_AND_END_DATE_REQUIRE);
                }
                //ARBS-98-Termination reason is mandatory for credit-adhoc records
                //if (contractMaintenance.DeleteReason == null)
                //{
                //    throw new ApplicationException("Please select Termination Reason");
                //}
                else if (!contractMaintenance.FinalRenewalStartDate.HasValue ||
                    !contractMaintenance.FinalRenewalEndDate.HasValue ||
                    !contractMaintenance.EndAmount.HasValue || contractMaintenance.DeleteReason == null)
                {
                    throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_END_DATE_AND_AMOUNT_REQUIRE);
                }
            }
            else
            {
                if (contractMaintenance.FinalRenewalStartDate.HasValue && !contractMaintenance.FinalRenewalEndDate.HasValue)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                }
                else if (!contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue)
                {
                    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                }
                //else if (contractMaintenance.FinalRenewalStartDate.HasValue && contractMaintenance.FinalRenewalEndDate.HasValue && !contractMaintenance.EndAmount.HasValue)
                //{
                //    throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                //}
                //If final renewal start, end and amount are filled then termination date & termination reason are mandatory/required
                if (contractMaintenance.FinalRenewalStartDate.HasValue &&
                       contractMaintenance.FinalRenewalEndDate.HasValue &&
                       contractMaintenance.IncludeInForecast == 0 && (contractMaintenance.EndAmount.HasValue || !contractMaintenance.EndAmount.HasValue) &&
                    //contractMaintenance.EndAmount.HasValue && contractMaintenance.IncludeInForecast == 0 &&
                       (!contractMaintenance.DeleteDate.HasValue || contractMaintenance.DeleteReason == null))
                {
                    if (!contractMaintenance.DeleteDate.HasValue
                        && contractMaintenance.DeleteReason != null)
                    {
                        throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                    }
                    else if (contractMaintenance.DeleteDate.HasValue
                        && contractMaintenance.DeleteReason == null)
                    {
                        throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                    }
                    else
                    {
                        throw new ApplicationException(Constants.FINAL_BILLIING_INFO_REQUIRED);
                    }
                }
            }

            //Termination date cannot be less than final renewal start date
            //if (contractMaintenance.DeleteDate.HasValue && contractMaintenance.FinalRenewalStartDate.HasValue)
            //{
            //    if (contractMaintenance.DeleteDate < contractMaintenance.FinalRenewalStartDate)
            //    {
            //        throw new ApplicationException(Constants.TERMINATION_DATE_CANNOT_BE_LESS_THAN_START_DATE);
            //    }
            //}

            //If charge frequency is credit, amount and base annual amount must be in negative
            if (contractMaintenance.PeriodFrequencyId == 6 && (contractMaintenance.EndAmount >= 0 || contractMaintenance.BaseAnnualAmount >= 0))
            {
                throw new ApplicationException(Constants.NEGATIVE_AMOUNT_REQUIRE);
            }

            //If charge frequency is not credit, amount and base annual amount must be in positive
            if (contractMaintenance.PeriodFrequencyId != 6 && (contractMaintenance.EndAmount < 0 || contractMaintenance.BaseAnnualAmount < 0))
            {
                throw new ApplicationException(Constants.POSITIVE_AMOUNT_REQUIRE);
            }

            ////Final renewal start date should cannot be less than First billing renewal date
            //if (contractMaintenance.FirstRenewalDate.HasValue && contractMaintenance.FinalRenewalStartDate.HasValue)
            //{
            //    DateTime firstRenewalDate = contractMaintenance.FirstRenewalDate.Value;
            //    DateTime finalRenewalStartDate = contractMaintenance.FinalRenewalStartDate.Value;
            //    bool validUpliftDate = false;

            //    if (finalRenewalStartDate >= firstRenewalDate)
            //    {
            //        validUpliftDate = true;
            //    }

            //    if (!validUpliftDate)
            //    {
            //        throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_CANNOT_BE_LESS_THAN_FIRST_BILLING_RENEWAL_DATE);
            //    }
            //}

            //Final Renewal start date should not be less than First period start date
            if (contractMaintenance.FirstPeriodStartDate.HasValue && contractMaintenance.FinalRenewalStartDate.HasValue)
            {
                DateTime firstPeriodStartDate = contractMaintenance.FirstPeriodStartDate.Value;
                //DateTime firstRenewalDate = contractMaintenance.FirstRenewalDate.Value;
                DateTime finalRenewalStartDate = contractMaintenance.FinalRenewalStartDate.Value;
                bool validUpliftDate = false;

                if (finalRenewalStartDate >= firstPeriodStartDate)
                {
                    validUpliftDate = true;
                }

                if (!validUpliftDate)
                {
                    throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_CANNOT_BE_LESS_THAN_FIRST_PERIOD_START_DATE);
                }
            }
            else if (contractMaintenance.FirstPeriodStartDate == null && contractMaintenance.FirstRenewalDate.HasValue && contractMaintenance.FinalRenewalStartDate.HasValue)
            {
                DateTime firstRenewalDate = contractMaintenance.FirstRenewalDate.Value;
                DateTime finalRenewalStartDate = contractMaintenance.FinalRenewalStartDate.Value;
                bool validUpliftDate = false;

                if (finalRenewalStartDate >= firstRenewalDate)
                {
                    validUpliftDate = true;
                }

                if (!validUpliftDate)
                {
                    throw new ApplicationException(Constants.FINAL_RENEWAL_START_DATE_CANNOT_BE_LESS_THAN_FIRST_BILLING_RENEWAL_DATE);
                }
            }

            //First annual uplift date should be anniversary of first renewal date
            if (contractMaintenance.FirstAnnualUpliftDate.HasValue && contractMaintenance.FirstRenewalDate.HasValue)
            {
                DateTime upliftDate = contractMaintenance.FirstAnnualUpliftDate.Value;
                DateTime renewalDate = contractMaintenance.FirstRenewalDate.Value;
                bool validUpliftDate = false;

                if (upliftDate >= renewalDate && upliftDate <= renewalDate.AddYears(5))
                {
                    int monthsToAdd = GetFrequencyMultipleToValidateFirstAnnualUpliftDate(contractMaintenance.PeriodFrequencyId);
                    int totalMonths = monthsToAdd;

                    //Check the date till 5 years
                    while (totalMonths <= 60)
                    {
                        if (renewalDate == upliftDate || renewalDate.AddMonths(totalMonths) == upliftDate)
                        {
                            validUpliftDate = true;
                            break;
                        }
                        totalMonths += monthsToAdd;
                    }
                }

                if (!validUpliftDate)
                {
                    throw new ApplicationException(Constants.FIRST_ANNUAL_UPLIFT_DATE_SHOULD_BE_AN_ANNIVERSARY_OF_FIRST_RENEWAL_DATE);
                }
            }

            //If IncludeInForecast(Backlog) is not selected then First renewal date and Activation date is mandatory/required
            if (contractMaintenance.IncludeInForecast == 0)
            {
                if (!contractMaintenance.FirstRenewalDate.HasValue)
                {
                    throw new ApplicationException(Constants.RENEWALDATE_REQUIRED);
                }
                else if (!contractMaintenance.ReasonDate.HasValue)
                {
                    throw new ApplicationException(Constants.ACTIVATIONDATE_REQUIRED);
                }
            }
        }

        #region Methods For Generating Milestones

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
            if (isUseIndex)
            {
                currentAmount = currentAmount + Math.Round((currentAmount * GetUpliftIndexRate(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value)), 2);
            }
            else
            {
                currentAmount = currentAmount + Math.Round((currentAmount * GetUplift(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value)), 2);
            }
        }

        /// <summary>
        /// Gets frequency multiple in months.
        /// </summary>
        /// <param name="frequency">The frequency</param>
        /// <returns>The multiple in months</returns>
        private int GetFrequencyMultiple(int frequency)
        {
            int frequencyMultiple = 1;

            if (frequency == Convert.ToInt32(Constants.ChargeFrequency.YEARLY)
                || frequency == Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                || frequency == Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                frequencyMultiple = 1;
            }
            else if (frequency == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
            {
                frequencyMultiple = 2;
            }
            else if (frequency == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
            {
                frequencyMultiple = 4;
            }
            else if (frequency == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
            {
                frequencyMultiple = 6;
            }
            else if (frequency == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
            {
                frequencyMultiple = 12;
            }
            return frequencyMultiple;
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
                if (contractMaintenanceVO.FirstRenewalDate.Value.Day == 29 && contractMaintenanceVO.FirstRenewalDate.Value.Month == 2)
                {
                    tempDate = new DateTime(DateTime.Now.Year, 2, 28);
                }
                else
                {
                    tempDate = new DateTime(DateTime.Now.Year,
                                                contractMaintenanceVO.FirstRenewalDate.Value.Month,
                                                contractMaintenanceVO.FirstRenewalDate.Value.Day
                        );
                }
            }

            if (!contractMaintenanceVO.FinalRenewalStartDate.HasValue)
            {
                currentEndDate = PeriodAdd(contractMaintenanceVO, tempDate, 3 * GetFrequencyMultiple(contractMaintenanceVO.PeriodFrequencyId));
            }
            else
            {
                currentEndDate = contractMaintenanceVO.FinalRenewalStartDate.Value;
            }
            return currentEndDate;
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

            if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.YEARLY))
            {
                periodAddedDate = dateToAdd.AddYears(periodToAdd);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.HALF_YEARLY))
            {
                periodAddedDate = dateToAdd.AddMonths(periodToAdd * 6);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.QUARTERLY))
            {
                periodAddedDate = dateToAdd.AddMonths(periodToAdd * 3);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.BI_MONTHLY))
            {
                periodAddedDate = dateToAdd.AddMonths(periodToAdd * 2);
            }
            else if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.MONTHLY))
            {
                periodAddedDate = dateToAdd.AddMonths(periodToAdd);
            }
            return periodAddedDate;
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
            if (contractMaintenanceVO.UpliftRequired.HasValue && contractMaintenanceVO.UpliftRequired.Value == true)
            {
                //  Decimal upliftFinal = 0;
                DateTime currentUpliftDate;

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
                        currentUpliftDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate, renewalEndDate);

                        //upliftDateFrom = currentUpliftDate.AddDays(-1);
                        upliftDateFrom = currentUpliftDate.AddDays(-(currentUpliftDate.Day - 1));
                        upliftDateTo = upliftDateFrom.AddMonths(1).AddDays(-1);

                        //If inflation index is available, get index rate per annum, else use last years
                        if (contractMaintenanceVO.InflationIndexId.HasValue)
                        {
                            InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();
                            InflationIndexRateVO inflationIndexRateVO =
                                inflationIndexRateService.GetInflationIndexRateById(
                                contractMaintenanceVO.InflationIndexId.Value,
                                upliftDateFrom, upliftDateTo);

                            if (inflationIndexRateVO != null)
                            {
                                upliftId = inflationIndexRateVO.InflationIndexId;
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

                //upliftDateFrom = currentUpliftDate.AddDays(-1);
                currentUpliftDate = GetDateForAdvancedArrears(contractMaintenanceVO, currentDate, renewalEndDate);
                upliftDateFrom = currentUpliftDate.AddDays(-(currentUpliftDate.Day - 1));
                upliftDateTo = upliftDateFrom.AddMonths(1).AddDays(-1);
                InflationIndexRateVO inflationIndexRateVO = null;

                //If inflation index is available
                //else use old index rate
                if (contractMaintenanceVO.InflationIndexId.HasValue)
                {
                    InflationIndexRateService inflationIndexRateService = new InflationIndexRateService();
                    inflationIndexRateVO = inflationIndexRateService.GetInflationIndexRateById(
                                            contractMaintenanceVO.InflationIndexId.Value,
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

                        //chinh update 091217 - round 1 decimal
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

                            //chinh update 091217 - round 1 decimal
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

                                //chinh update 091217 - round 1 decimal
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
        /// Get Date after applying invoice in arrears.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <param name="currentDate"></param>
        /// <param name="renewalEndDate"></param>
        /// <returns></returns>
        private DateTime GetDateForAdvancedArrears(ContractMaintenanceVO contractMaintenanceVO, DateTime currentDate, DateTime renewalEndDate)
        {
            DateTime dateForAdvanceArrears = currentDate;
            if (contractMaintenanceVO.InvoiceAdvancedArrears == 0)
            {
                dateForAdvanceArrears = currentDate;
            }
            else if (contractMaintenanceVO.InvoiceAdvancedArrears == 1)
            {
                dateForAdvanceArrears = currentDate.AddMonths(-contractMaintenanceVO.InvoiceAdvancedId);
            }
            else if (contractMaintenanceVO.InvoiceAdvancedArrears == 2)
            {
                dateForAdvanceArrears = renewalEndDate.AddMonths(contractMaintenanceVO.InvoiceAdvancedId);
            }
            return dateForAdvanceArrears;
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
            if (contractMaintenanceVO.PeriodFrequencyId == Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                milestoneVO.Description = "Maintenance Credit";
            }
            else
            {
                milestoneVO.Description = Constants.MAINTENANCE;
            }

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

            milestoneVO.ConditionType = "MS";
            milestoneVO.CreatedByUserId = userId;

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
            milestoneVO.MilestoneStatusID = Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING);
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

            return milestoneVO;
        }

        /// <summary>
        /// Add last milestone.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <param name="currentDate"></param>
        /// <param name="renewalEndDate"></param>
        /// <param name="currentAmount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private void AddORUpdateLastMilestone(ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
            DateTime? previousDate, DateTime? renewalEndDate, Decimal? currentAmount, decimal? previousAmount,
            int? userId, MilestoneVO milestoneVO, List<MilestoneVO> milestoneVOList)
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

            if (milestoneVO == null)
            {
                MilestoneVO lastMilestone = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate,
                                                         currentAmount, previousAmount, userId);
                if (contractMaintenanceVO.EndAmount != null)
                {
                    lastMilestone.Uplift = 0;
                    lastMilestone.UpliftFixedRate = 0;
                    lastMilestone.IndexRate = null;
                }

                milestoneVOList.Add(lastMilestone);
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
                    MilestoneVO lastMilestone =
                    EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);

                    if (contractMaintenanceVO.EndAmount != null)
                    {
                        lastMilestone.Uplift = 0;
                        lastMilestone.UpliftFixedRate = 0;
                        lastMilestone.IndexRate = null;
                    }
                }
            }
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
        /// <param name="userId">The user id</param>
        private void AddFirstMilestone(ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
             DateTime? renewalEndDate, Decimal? currentAmount, List<MilestoneVO> milestones, decimal previousAmount, int? userId)
        {
            //Add/Update first billing line.
            //This needs to be done if user has provided first period start date.
            if (contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.AD_HOC)
                && contractMaintenanceVO.PeriodFrequencyId != Convert.ToInt32(Constants.ChargeFrequency.CREDIT))
            {
                if (contractMaintenanceVO.FirstRenewalDate.HasValue && contractMaintenanceVO.FirstPeriodStartDate.HasValue &&
                    contractMaintenanceVO.FirstRenewalDate.Value > contractMaintenanceVO.FirstPeriodStartDate.Value)
                {
                    //Add first milestone
                    if (milestones.Count == 0)
                    {
                        MilestoneVO milestoneVO = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                        milestoneVO = SetUpliftToZeroForFirstMilestone(milestoneVO);
                        milestones.Add(milestoneVO);
                    }
                    else
                    {
                        MilestoneVO milestoneVO = milestones.Find(x => x.RenewalStartDate == currentDate);
                        if (milestoneVO != null)
                        {
                            //Edit the milestone if not ready for calculate
                            if (milestoneVO.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                            {
                                milestoneVO = EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                                milestoneVO = SetUpliftToZeroForFirstMilestone(milestoneVO);
                            }
                        }
                        else
                        {
                            milestoneVO = AddMilestone(contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, previousAmount, userId);
                            milestoneVO = SetUpliftToZeroForFirstMilestone(milestoneVO);
                            milestones.Add(milestoneVO);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the billing line text for the milestone based on the maintenance billing line.
        /// </summary>
        private void CalculateBillingLineText(MilestoneVO milestoneVO, ContractMaintenanceVO contractMaintenanceVO)
        {
            int linecount = 0;

            ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
            List<MaintenanceBillingLineVO> maintenanceBillingLineVos = contractMaintenanceVO.MaintenanceBillingLineVos;

            // Get default printing line for coding details
            if (contractMaintenanceVO.IsGrouped == true && contractMaintenanceVO.GroupId != null && contractMaintenanceVO.IsDefaultLineInGroup == false)
            {
                ContractMaintenanceVO contractMaintenance =
                    contractMaintenanceService.GetDefaultLineOfContractMaintenanceGroup(
                        contractMaintenanceVO.ContractId, contractMaintenanceVO.PeriodFrequencyId, (int)contractMaintenanceVO.GroupId);

                maintenanceBillingLineVos = contractMaintenance.MaintenanceBillingLineVos;
            }

            //Loop though all billing details billing lines
            foreach (MaintenanceBillingLineVO maintenanceBillingLine in maintenanceBillingLineVos)
            {
                //Edit milestone billing line
                if (linecount < milestoneVO.MilestoneBillingLineVos.Count)
                {
                    //MilestoneBillingLine milestoneBillingLine = milestoneVO.MilestoneBillingLines[linecount];
                    //milestoneBillingLine.LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText);
                    //milestoneBillingLine.LineSequance = linecount;

                    MilestoneBillingLineVO milestoneBillingLineVO = milestoneVO.MilestoneBillingLineVos[linecount];
                    milestoneBillingLineVO.LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText);
                    milestoneBillingLineVO.LineSequance = linecount;
                }
                else
                {
                    //Add milestone billing line
                    if (maintenanceBillingLineVos.Count > linecount)
                    {
                        milestoneVO.MilestoneBillingLineVos.Add(new MilestoneBillingLineVO()
                        {
                            LineText = CreateBillingLineText(milestoneVO, maintenanceBillingLine.LineText),
                            LineSequance = linecount,
                            ContractId = contractMaintenanceVO.ContractId,
                        });
                    }
                }

                linecount++;
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
                    tempString = tempString + maintenanceBillingLineText.Substring(prevpos, maintenanceBillingLineText.Length - prevpos);
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
                case "RENEWAL_START_DATE": fieldValue = milestoneVO.RenewalStartDate.HasValue ? milestoneVO.RenewalStartDate.Value.Date.ToString(Constants.DATE_FORMAT) : string.Empty; break;
                case "RENEWAL_END_DATE": fieldValue = milestoneVO.RenewalEndDate.HasValue ? milestoneVO.RenewalEndDate.Value.Date.ToString(Constants.DATE_FORMAT) : string.Empty; break;
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

        #endregion Methods For Generating Milestones

        /// <summary>
        /// Fill maintenance billing lines
        /// </summary>
        /// <param name="contractMaintenance">The contract maintenance model object</param>
        /// <param name="maintenanceBillingLines">The maintenance billing lines</param>
        //private void FillMaintenanceBillingLines(ContractMaintenance contractMaintenance, List<MaintenanceBillingLine> maintenanceBillingLines)
        //{
        //    if (maintenanceBillingLines.Count > 0)
        //    {
        //        contractMaintenance.billingText1 = maintenanceBillingLines[0].LineText;
        //        contractMaintenance.billingTextID1 = maintenanceBillingLines[0].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 1)
        //    {
        //        contractMaintenance.billingText2 = maintenanceBillingLines[1].LineText;
        //        contractMaintenance.billingTextID2 = maintenanceBillingLines[1].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 2)
        //    {
        //        contractMaintenance.billingText3 = maintenanceBillingLines[2].LineText;
        //        contractMaintenance.billingTextID3 = maintenanceBillingLines[2].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 3)
        //    {
        //        contractMaintenance.billingText4 = maintenanceBillingLines[3].LineText;
        //        contractMaintenance.billingTextID4 = maintenanceBillingLines[3].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 4)
        //    {
        //        contractMaintenance.billingText5 = maintenanceBillingLines[4].LineText;
        //        contractMaintenance.billingTextID5 = maintenanceBillingLines[4].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 5)
        //    {
        //        contractMaintenance.billingText6 = maintenanceBillingLines[5].LineText;
        //        contractMaintenance.billingTextID6 = maintenanceBillingLines[5].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 6)
        //    {
        //        contractMaintenance.billingText7 = maintenanceBillingLines[6].LineText;
        //        contractMaintenance.billingTextID7 = maintenanceBillingLines[6].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 7)
        //    {
        //        contractMaintenance.billingText8 = maintenanceBillingLines[7].LineText;
        //        contractMaintenance.billingTextID8 = maintenanceBillingLines[7].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 8)
        //    {
        //        contractMaintenance.billingText9 = maintenanceBillingLines[8].LineText;
        //        contractMaintenance.billingTextID9 = maintenanceBillingLines[8].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 9)
        //    {
        //        contractMaintenance.billingText10 = maintenanceBillingLines[9].LineText;
        //        contractMaintenance.billingTextID10 = maintenanceBillingLines[9].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 10)
        //    {
        //        contractMaintenance.billingText11 = maintenanceBillingLines[10].LineText;
        //        contractMaintenance.billingTextID11 = maintenanceBillingLines[10].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 11)
        //    {
        //        contractMaintenance.billingText12 = maintenanceBillingLines[11].LineText;
        //        contractMaintenance.billingTextID12 = maintenanceBillingLines[11].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 12)
        //    {
        //        contractMaintenance.billingText13 = maintenanceBillingLines[12].LineText;
        //        contractMaintenance.billingTextID13 = maintenanceBillingLines[12].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 13)
        //    {
        //        contractMaintenance.billingText14 = maintenanceBillingLines[13].LineText;
        //        contractMaintenance.billingTextID14 = maintenanceBillingLines[13].BillingLineID;
        //    }
        //    if (maintenanceBillingLines.Count > 14)
        //    {
        //        contractMaintenance.billingText15 = maintenanceBillingLines[14].LineText;
        //        contractMaintenance.billingTextID15 = maintenanceBillingLines[14].BillingLineID;
        //    }

        //    //contractMaintenance.billingText2 = (maintenanceBillingLines.Count > 2) ? maintenanceBillingLines[2].LineText : String.Empty;
        //    //contractMaintenance.billingText3 = (maintenanceBillingLines.Count > 3) ? maintenanceBillingLines[3].LineText : String.Empty;
        //    //contractMaintenance.billingText4 = (maintenanceBillingLines.Count > 4) ? maintenanceBillingLines[4].LineText : String.Empty;
        //    //contractMaintenance.billingText5 = (maintenanceBillingLines.Count > 5) ? maintenanceBillingLines[5].LineText : String.Empty;
        //    //contractMaintenance.billingText6 = (maintenanceBillingLines.Count > 6) ? maintenanceBillingLines[6].LineText : String.Empty;
        //    //contractMaintenance.billingText7 = (maintenanceBillingLines.Count > 7) ? maintenanceBillingLines[7].LineText : String.Empty;
        //    //contractMaintenance.billingText8 = (maintenanceBillingLines.Count > 8) ? maintenanceBillingLines[8].LineText : String.Empty;
        //    //contractMaintenance.billingText9 = (maintenanceBillingLines.Count > 9) ? maintenanceBillingLines[9].LineText : String.Empty;
        //    //contractMaintenance.billingText10 = (maintenanceBillingLines.Count > 10) ? maintenanceBillingLines[10].LineText : String.Empty;
        //    //contractMaintenance.billingText11 = (maintenanceBillingLines.Count > 11) ? maintenanceBillingLines[11].LineText : String.Empty;
        //    //contractMaintenance.billingText12 = (maintenanceBillingLines.Count > 12) ? maintenanceBillingLines[12].LineText : String.Empty;
        //    //contractMaintenance.billingText13 = (maintenanceBillingLines.Count > 13) ? maintenanceBillingLines[13].LineText : String.Empty;
        //    //contractMaintenance.billingText14 = (maintenanceBillingLines.Count > 14) ? maintenanceBillingLines[14].LineText : String.Empty;
        //    //contractMaintenance.billingText15 = (maintenanceBillingLines.Count > 15) ? maintenanceBillingLines[15].LineText : String.Empty;
        //}

        /// <summary>
        /// Fill maintenance billing lines
        /// </summary>
        /// <param name="contractMaintenance">The contract maintenance model object</param>
        /// <param name="maintenanceBillingLines">The maintenance billing lines</param>
        private void FillMaintenanceBillingLines(ContractMaintenance contractMaintenance, List<MaintenanceBillingLine> maintenanceBillingLines)
        {
            foreach (var item in maintenanceBillingLines)
            {
                switch (item.LineSequance)
                {
                    case 1:
                        contractMaintenance.billingText1 = item.LineText;
                        contractMaintenance.billingTextID1 = item.BillingLineID;
                        break;

                    case 2:
                        contractMaintenance.billingText2 = item.LineText;
                        contractMaintenance.billingTextID2 = item.BillingLineID;
                        break;

                    case 3:
                        contractMaintenance.billingText3 = item.LineText;
                        contractMaintenance.billingTextID3 = item.BillingLineID;
                        break;

                    case 4:
                        contractMaintenance.billingText4 = item.LineText;
                        contractMaintenance.billingTextID4 = item.BillingLineID;
                        break;

                    case 5:
                        contractMaintenance.billingText5 = item.LineText;
                        contractMaintenance.billingTextID5 = item.BillingLineID;
                        break;

                    case 6:
                        contractMaintenance.billingText6 = item.LineText;
                        contractMaintenance.billingTextID6 = item.BillingLineID;
                        break;

                    case 7:
                        contractMaintenance.billingText7 = item.LineText;
                        contractMaintenance.billingTextID7 = item.BillingLineID;
                        break;

                    case 8:
                        contractMaintenance.billingText8 = item.LineText;
                        contractMaintenance.billingTextID8 = item.BillingLineID;
                        break;

                    case 9:
                        contractMaintenance.billingText9 = item.LineText;
                        contractMaintenance.billingTextID9 = item.BillingLineID;
                        break;

                    case 10:
                        contractMaintenance.billingText10 = item.LineText;
                        contractMaintenance.billingTextID10 = item.BillingLineID;
                        break;

                    case 11:
                        contractMaintenance.billingText11 = item.LineText;
                        contractMaintenance.billingTextID11 = item.BillingLineID;
                        break;

                    case 12:
                        contractMaintenance.billingText12 = item.LineText;
                        contractMaintenance.billingTextID12 = item.BillingLineID;
                        break;

                    case 13:
                        contractMaintenance.billingText13 = item.LineText;
                        contractMaintenance.billingTextID13 = item.BillingLineID;
                        break;

                    case 14:
                        contractMaintenance.billingText14 = item.LineText;
                        contractMaintenance.billingTextID14 = item.BillingLineID;
                        break;

                    case 15:
                        contractMaintenance.billingText15 = item.LineText;
                        contractMaintenance.billingTextID15 = item.BillingLineID;
                        break;
                }
            }
        }
        /// <summary>
        /// Gets frequency multiple in months.
        /// </summary>
        /// <param name="frequency">The frequency</param>
        /// <returns>The multiple in months</returns>
        private int GetFrequencyMultipleToValidateFirstAnnualUpliftDate(int frequency)
        {
            int frequencyMultiple = 1;

            //Yearly, Ad-hoc, or Credit
            if (frequency == 4 || frequency == 5 || frequency == 6)
            {
                frequencyMultiple = 12;
            }
            //Half yearly
            else if (frequency == 3)
            {
                frequencyMultiple = 6;
            }
            //Quarterly
            else if (frequency == 2)
            {
                frequencyMultiple = 3;
            }
            //Bi-monthly
            else if (frequency == 7)
            {
                frequencyMultiple = 2;
            }
            //Monthly
            else if (frequency == 1)
            {
                frequencyMultiple = 1;
            }
            return frequencyMultiple;
        }

        /// <summary>
        /// Get default invoice in advance from division table
        /// </summary>
        /// <param name="contractId">contract id</param>
        /// <returns>Invoice in advanced from division</returns>
        private int GetInvoiceInAdvanceFromDivision(int contractId)
        {
            ContractMaintenanceService contractMaintenanceService = new ContractMaintenanceService();
            return contractMaintenanceService.GetInvoiceInAdvanceFromDivision(contractId);
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


        /// <summary>
        /// Gets the Customer Comment based on company id and customer id
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">invoice customer id</param>
        /// <returns>Customer comment</returns>
        private string GetCustomerCommentForContractmaintenance(int companyId, int invoiceCustomerId)
        {
            MODEL.ContractMaintenance contractMaintenance = new MODEL.ContractMaintenance();

            CustomerCommentService customerCommentService = new CustomerCommentService();
            CustomerCommentVO customerCommentVO = customerCommentService.GetCustomerCommentByCompanyAndCutomer(companyId, invoiceCustomerId);
            if (customerCommentVO != null)
            {
                contractMaintenance.CustomerComment = customerCommentVO.CustomerComment;
            }

            return (contractMaintenance.CustomerComment);
        }
        #endregion Private Methods

        #region commented code

        /// <summary>
        /// Gets list of contracts for the specified company and selection criteria.
        /// </summary>
        /// <param name="param">The data table search criteria</param>
        /// <param name="companyId">The company for which contracts are required</param>
        /// <param name="activityCodeId">The activity code id</param>
        /// <returns>List of contracts</returns>
        //public ActionResult ContractMaintenanceList(MODEL.jQueryDataTableParamModel param, int contractLineId, int activityCodeId)
        //{
        //    try
        //    {
        //        ContractMaintenanceService contractManitenanceService = new ContractMaintenanceService();
        //        List<ContractMaintenanceVO> contractVOList = contractManitenanceService.GetContractMaintenanceList(contractLineId, activityCodeId);

        //        List<MODEL.ContractMaintenance> contractMaintenance = new List<MODEL.ContractMaintenance>();
        //        foreach (var item in contractVOList)
        //        {
        //            contractMaintenance.Add(new Models.ContractMaintenance(item));
        //        }

        //        //get the field on with sorting needs to happen and set the
        //        //ordering function/delegate accordingly.
        //        int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //        var orderingFunction = GetContractMaintenanceOrderingFunction(sortColumnIndex);

        //        var result = GetFilteredObjects(param, contractMaintenance, orderingFunction);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, e.Message);
        //    }
        //}

        /// <summary>
        /// Returns a partial view that is used to create a new contract maintenance line
        /// </summary>
        /// <returns></returns>
        //public ActionResult ContractMaintenanceCreate(int companyId, int contractId, int contractLineId, int activityCodeId, string activityCode)
        //{
        //    try
        //    {
        //        MODEL.ContractMaintenance contractMaintenance = new MODEL.ContractMaintenance();

        //        contractMaintenance.InflationIndexList = GetInflationIndexList();
        //        //contractMaintenance.ActivityCodeList = GetContractAssociatedActivityCodeList(contractId, contractLineId);
        //        //  contractMaintenance.IncludeForecastList = GetIncludeForcastList();
        //        contractMaintenance.InvoiceAdvancedList = GetInvoiceAdvancedArrears();
        //        contractMaintenance.InvoiceAdvancedValueList = GetInvoiceAdvancedValue();
        //        contractMaintenance.ProductList = GetProductList();
        //        contractMaintenance.AuditReasonList = GetReasonCodeList();
        //        contractMaintenance.ChargingFrequencyList = GetChargeFrequencyList();

        //        //Set default values
        //        contractMaintenance.ActivityCodeId = activityCodeId;
        //        contractMaintenance.ActivityCode = activityCode;
        //        contractMaintenance.ContractId = contractId;
        //        contractMaintenance.ContractLineId = contractLineId;
        //        contractMaintenance.CompanyId = companyId;
        //        contractMaintenance.InvoiceAdvancedArrears = 1;
        //        contractMaintenance.InvoiceInAdvance = 2;
        //        contractMaintenance.IncludeInForecast = true;
        //        //contractMaintenance.Contract.CompanyId = companyId;

        //        return PartialView("ContractMaintenanceDetails", contractMaintenance);
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeAndErrorResult(500, e.Message);
        //    }
        //}

        /// <summary>
        /// Get activity code list associated with contract and contract lines
        /// </summary>
        /// <param name="ContractId">The contract id</param>
        /// <returns>List of activity code</returns>
        //private List<ActivityCode> GetContractAssociatedActivityCodeList(int ContractId, int contractLineId)
        //{
        //    //List<ActivityCode> activityCodeList = new List<ActivityCode>();
        //    //ContractLineService contractLineService = new ContractLineService();
        //    //List<ContractLineVO> contractLineVOList = contractLineService.GetContractLineByContractId(ContractId);
        //    //List<MODEL.ContractLine> contractLines = new List<MODEL.ContractLine>();
        //    //foreach (var item in contractLineVOList)
        //    //{
        //    //    if (!activityCodeList.Exists(x => x.ID == item.ActivityCodeId))
        //    //    {
        //    //        activityCodeList.Add(new ActivityCode() { ID = item.ActivityCodeId, Name = item.ActivityCode });
        //    //    }
        //    //}
        //    //return activityCodeList;

        //    List<ActivityCode> activityCodeList = new List<ActivityCode>();
        //    ContractLineService contractLineService = new ContractLineService();
        //    ContractLineVO contractLineVO = contractLineService.GetContractLineById(contractLineId);
        //    if (contractLineVO != null)
        //    {
        //        activityCodeList.Add(new ActivityCode() { ID = contractLineVO.ActivityCodeId, Name = contractLineVO.ActivityCode });
        //    }

        //    return activityCodeList;
        //}

        /// <summary>
        /// Get include in forecast list
        /// </summary>
        /// <returns>List of include in forcast</returns>
        //private List<IncludeForecast> GetIncludeForcastList()
        //{
        //    List<IncludeForecast> includeForecastList = new List<IncludeForecast>();
        //    includeForecastList.Add(new IncludeForecast() { IncludeInForcast = false, Type = "No" });
        //    includeForecastList.Add(new IncludeForecast() { IncludeInForcast = true, Type = "Yes" });
        //    return includeForecastList;
        //}

        ///// <summary>
        ///// Fill billing lines to store in database
        ///// </summary>
        ///// <param name="contractMaintenance"></param>
        //private void FillBillingLines(MODEL.ContractMaintenance contractMaintenance)
        //{
        //    if (!String.IsNullOrEmpty(contractMaintenance.billingText1))
        //    {
        //        MaintenanceBillingLines.Add(new MODEL.MaintenanceBillingLine() { LineSequance = 1, LineText = contractMaintenance.billingText1 });
        //    }
        //}

        /// <summary>
        /// Update last milestone.
        /// </summary>
        /// <param name="contractMaintenanceVO"></param>
        /// <param name="currentDate"></param>
        /// <param name="renewalEndDate"></param>
        /// <param name="currentAmount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //private MilestoneVO UpdateLastMilestone(MilestoneVO milestoneVO, ContractMaintenanceVO contractMaintenanceVO, DateTime? currentDate,
        //    DateTime? previousDate, DateTime? renewalEndDate, Decimal? currentAmount, decimal? previousAmount, int? userId)
        //{
        //    decimal? endAmount = null;
        //    if (contractMaintenanceVO.FinalRenewalStartDate.HasValue)
        //    {
        //        if (!contractMaintenanceVO.EndAmount.HasValue)
        //        {
        //            if (contractMaintenanceVO.FinalRenewalStartDate > previousDate)
        //            {
        //                DateTime newYearDate = previousDate.Value.AddYears(1);
        //                int daysInYear = (newYearDate - previousDate.Value).Days;
        //                endAmount = previousAmount * ((contractMaintenanceVO.FinalRenewalStartDate.Value - previousDate.Value).Days / daysInYear);
        //                endAmount = endAmount + Math.Round(endAmount.Value * GetUplift(contractMaintenanceVO, currentDate.Value, renewalEndDate.Value));
        //            }
        //            else
        //            {
        //                endAmount = currentAmount;
        //            }
        //        }
        //        else
        //        {
        //            endAmount = contractMaintenanceVO.EndAmount;
        //        }

        //        //@TODO : Need to check. if this requires
        //        currentDate = contractMaintenanceVO.FinalRenewalStartDate;
        //        renewalEndDate = contractMaintenanceVO.FinalRenewalEndDate;
        //    }

        //    previousAmount = currentAmount;
        //    currentAmount = endAmount;

        //    //Set defalt milestone status
        //    milestoneVO.MilestoneStatusName = "RC";
        //    milestoneVO.MilestoneStatusID = 9;
        //    milestoneVO.IsApprovalRequired = true;
        //    milestoneVO.ApprovedStatus = "U";

        //   return EditMilestone(milestoneVO, contractMaintenanceVO, currentDate, renewalEndDate, currentAmount, userId);
        //}
        #endregion commented code
    }
}