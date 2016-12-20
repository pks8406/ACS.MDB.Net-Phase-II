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
    public partial class ContractController
    {
        /// <summary>
        /// Returns enduser index view
        /// </summary>
        /// <returns>enduser index view</returns>
        /// GET: /ContractController/EndUserMaintenanceIndex
        public ActionResult EndUserIndex()
        {
            MODEL.EndUser endUserModel = new EndUser();
            try
            {
                endUserModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            }
            catch (Exception)
            {
            }
            return View(endUserModel);
        }

        /// <summary>
        /// Get the list of EndUser based on companyId and customerId
        /// </summary>
        /// <param name="param"></param>
        /// <param name="companyId">company Id</param>
        /// <param name="customerId">customer Id</param>
        /// <returns>End User List</returns>
        public ActionResult EndUserList(MODEL.jQueryDataTableParamModel param, int companyId, int customerId)
        {
            try
            {
                EndUserService enduserService = new EndUserService();
                List<MODEL.EndUser> enduserList = new List<EndUser>();

                List<EndUserVO> enduserVOlist = enduserService.GetEndUserListByCompanyIdandCustomerId(companyId, customerId);

                foreach (var item in enduserVOlist)
                {
                    enduserList.Add(new MODEL.EndUser(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetEndUserOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, enduserList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new EndUser specified to company and customer
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <param name="customerId">customer Id</param>
        /// <returns>End User details view</returns>
        public ActionResult EndUserCreate(int companyId, int customerId)
        {
            try
            {
                MODEL.EndUser enduser = new MODEL.EndUser();

                enduser.CompanyId = companyId;
                enduser.InvoiceCustomerId = customerId;

                return PartialView("EndUserDetails", enduser);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Save the end user
        /// </summary>
        /// <param name="model">The EndUser model</param>
        public ActionResult EndUserSave(MODEL.EndUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    EndUserService endUserService = new EndUserService();
                    EndUserVO endUserVO = model.Transpose(userId);
                    endUserService.SaveEndUser(endUserVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.ENDUSER));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit end user
        /// </summary>
        /// <param name="id">The end user Id</param>
        /// <returns>The enduser details view</returns>
        public ActionResult EndUserEdit(int id)
        {
            MODEL.EndUser enduser = new EndUser();
            try
            {
                EndUserService enduserService = new EndUserService();

                //Get EndUser details
                EndUserVO enduserVO = enduserService.GetEndUserById(id);
                if (enduserVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.ENDUSER));
                }
                else
                {
                    enduser = new EndUser(enduserVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("EndUserDetails", enduser);
        }

        /// <summary>
        /// Delete the EndUser(s)
        /// </summary>
        /// <param name="Ids">Ids of endusers to be deleted</param>
        public ActionResult EndUserDelete(List<int> ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                EndUserService enduserService = new EndUserService();
                enduserService.DeleteEndUser(ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Get InvoiceCustomer List based on companyId
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>Invoice customer List based on company Id</returns>
        public JsonResult GetInvoiceCustomerList(int companyId)
        {
            try
            {
                List<InvoiceCustomerVO> invoiceCustomerVOList = null;
             
                if (companyId != -1)
                {
                    InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
                    invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(companyId);

                    List<InvoiceCustomer> invoiceCustomers = new List<InvoiceCustomer>();
                    foreach (var invoiceCustomer in invoiceCustomerVOList)
                    {
                        invoiceCustomers.Add(new InvoiceCustomer(invoiceCustomer));
                    }
                }

                var jsonResult = Json(invoiceCustomerVOList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;

                return jsonResult;
            }
            catch (Exception e)
            {
                return Json(new ApplicationException(e.Message));
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns field to sort</returns>
        public Func<BaseModel, object> GetEndUserOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.EndUser)obj).Name;
                    break;

                default:
                    sortFunction = obj => ((MODEL.EndUser)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}