using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

using MODEL = ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class AdministrationController
    {
        /// <summary>
        /// Returns division index view
        /// </summary>
        /// <returns>division index view</returns>
        // GET: /Administration/DivisionIndex
        public ActionResult DivisionIndex()
        {
            MODEL.Division divisionModel = new Division();
            try
            {
                divisionModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            }
            catch (Exception)
            {
            }
            return IndexViewForAuthorizeUser(divisionModel);
        }

        /// <summary>
        /// Returns a list of divisions for the specified company and selection criteria.
        /// </summary>
        /// <param name="param">The data table search criteria</param>
        /// <param name="companyId">The company for which divisions are required. </param>
        /// <returns>List of divisions</returns>
        public ActionResult DivisionList(MODEL.jQueryDataTableParamModel param, int? companyId = 0)
        {
            try
            {
                DivisionService divisionService = new DivisionService();
                List<MODEL.Division> divisionList = new List<MODEL.Division>();

                List<DivisionVO> divisionVOlist = divisionService.GetDivisionListByCompany(companyId);
                foreach (DivisionVO divisionVO in divisionVOlist)
                {
                    divisionList.Add(new MODEL.Division(divisionVO));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetDivisionOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjectsOrderByAscending(param, divisionList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new division for the specified company
        /// </summary>
        /// <returns></returns>
        public ActionResult DivisionCreate(int companyId, string companyName)
        {
            try
            {
                MODEL.Division division = new MODEL.Division();
                division.CompanyId = companyId;
                division.CompanyName = companyName;
                division.defaultInvoiceInAdvancedList = GetInvoiceInAdvancedList();
                division.IsActive = true;
                return PartialView("DivisionDetails", division);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit division details.
        /// </summary>
        /// <param name="Id">Division Id</param>
        /// <returns> Divisiondetails</returns>
        public ActionResult DivisionEdit(int id)
        {
            MODEL.Division division = null;
            try
            {
                DivisionService divisionService = new DivisionService();

                //Get division details
                DivisionVO divisionVO = divisionService.GetDivisionById(id);
                if (divisionVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.DIVISION));
                }
                else
                {
                    division = new Division(divisionVO);
                    division.defaultInvoiceInAdvancedList = GetInvoiceInAdvancedList();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("DivisionDetails", division);
        }

        /// <summary>
        /// Save the Division details.
        /// </summary>
        /// <param name="model">The division model</param>
        /// <returns></returns>
        public ActionResult DivisionSave(MODEL.Division model)
        {
            try
            {
                DivisionService divisionService = new DivisionService();
                List<MODEL.Division> Division = new List<MODEL.Division>();

                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();

                    //DivisionVO divisionVO = new DivisionVO(model, userId.Value);

                    DivisionVO divisionVO = model.Transpose(userId);

                    divisionService.SaveDivision(divisionVO);

                    List<DivisionVO> divisionVOlist = divisionService.GetDivisionListByCompany(model.CompanyId);

                    foreach (DivisionVO division in divisionVOlist)
                    {
                        Division.Add(new MODEL.Division(division));
                    }
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.DIVISION));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<BaseModel, object> GetDivisionOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((Division)obj).DivisionName;
                    break;

                case 3:
                    sortFunction = obj => ((Division)obj).DocumentTypeCR;
                    break;

                case 4:
                    sortFunction = obj => ((Division)obj).DocumentTypeIN;
                    break;

                case 5:
                    sortFunction = obj => ((Division)obj).DocumentTypeDepositInvoices;
                    break;

                case 6:
                    sortFunction = obj => ((Division)obj).DocumentTypeDepositCredits;
                    break;

                case 7:
                    sortFunction = obj => ((Division)obj).IsActive;
                    break;

                default:
                    sortFunction = obj => ((Division)obj).DivisionName;
                    break;
            }

            return sortFunction;
        }


        /// <summary>
        /// Get the list of default invoice in advanced
        /// </summary>
        /// <returns>List of default invoice in advanced</returns>
        private List<MODEL.InvoiceAdvanced> GetInvoiceInAdvancedList()
        {
            List<InvoiceAdvanced> defaultInvoiceInAdvancedList = new List<InvoiceAdvanced>();
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 0, Value = "0" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 1, Value = "1" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 2, Value = "2" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 3, Value = "3" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 4, Value = "4" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 5, Value = "5" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 6, Value = "6" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 7, Value = "7" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 8, Value = "8" });
            defaultInvoiceInAdvancedList.Add(new InvoiceAdvanced() { ID = 9, Value = "9" });
            return defaultInvoiceInAdvancedList;
        }
    }
}