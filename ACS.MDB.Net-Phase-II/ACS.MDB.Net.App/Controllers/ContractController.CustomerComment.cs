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
        /// Returns CustomerComment index view
        /// </summary>
        /// <returns>CustomerComment index view</returns>
        /// GET: /ContractController/CustomerComment/
        public ActionResult CustomerCommentIndex()
        {
            return View();
        }

        /// <summary>
        /// Gets the customer comment list
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Customer comment List</returns>
        public ActionResult CustomerCommentList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                List<Company> companyList = Session.GetUserAssociatedCompanyList();
                List<CompanyVO> companyVOList = new List<CompanyVO>();
                foreach (var item in companyList)
                {
                    //companyVOList.Add(new CompanyVO(item));
                    companyVOList.Add(item.Transpose());
                }

                CustomerCommentService customerCommentService = new CustomerCommentService();
                List<MODEL.CustomerComment> customerCommentList = new List<CustomerComment>();

                List<CustomerCommentVO> customerCommentVOList = customerCommentService.GetCustomerCommentList(companyVOList);

                foreach (var item in customerCommentVOList)
                {
                    customerCommentList.Add(new MODEL.CustomerComment(item));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetCustomerCommentOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, customerCommentList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new customer comment
        /// </summary>
        /// <returns>The Customer comment details view</returns>
        public ActionResult CustomerCommentCreate()
        {
            try
            {
                MODEL.CustomerComment customerComment = new MODEL.CustomerComment();

                customerComment.OAcompanyList = Session.GetUserAssociatedCompanyList();
                return PartialView("CustomerCommentDetails", customerComment);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit customer comment
        /// </summary>
        /// <param name="id">The customer comment id</param>
        /// <returns>Customer comment details</returns>
        public ActionResult CustomerCommentEdit(int id)
        {
            MODEL.CustomerComment customercomment = new CustomerComment();
            try
            {
                CustomerCommentService customerCommentService = new CustomerCommentService();

                //Get Customercomment details
                CustomerCommentVO customerCommentVO = customerCommentService.GetCustomerCommentById(id);
                if (customerCommentVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.CUSTOMERCOMMENT));
                }
                else
                {
                    customercomment = new CustomerComment(customerCommentVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("CustomerCommentDetails", customercomment);
        }

        /// <summary>
        /// Save the customer comment
        /// </summary>
        /// <param name="model">model object</param>
        /// <returns></returns>
        public ActionResult CustomerCommentSave(MODEL.CustomerComment model)
        {
            try
            {
                bool ismodelValid = ModelState.IsValid;
                if (!ismodelValid)
                {
                    ismodelValid = IsModelValidForMultilineTextbox("Comment", model.Comment, 220);

                    if (ismodelValid)
                    {
                        model.Comment = model.Comment.Replace("\r\n", "\n");
                    }
                }

                if (ismodelValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    CustomerCommentService customerCommentService = new CustomerCommentService();
                    //CustomerCommentVO customerCommentVO = new CustomerCommentVO(model, userId);

                    CustomerCommentVO customerCommentVO = model.Transpose(userId);

                    customerCommentService.SaveCustomerComment(customerCommentVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.CUSTOMERCOMMENT));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete the Customer comment(s)
        /// </summary>
        /// <param name="Ids">Ids of customerComment to be deleted</param>
        public ActionResult CustomerCommentDelete(List<int> ids)
        {
            try
            {
                //Get User Id
                int? userId = Session.GetUserId();

                CustomerCommentService customerCommentService = new CustomerCommentService();
                customerCommentService.DeleteCustomerComment(ids, userId);
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
        public JsonResult GetInvoiceCustomerListByCompany(int companyId)
        {
            try
            {
                InvoiceCustomerService invoiceCustomerService = new InvoiceCustomerService();
                List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(companyId);
                return Json(invoiceCustomerVOList);
            }
            catch (Exception e)
            {
                return Json(new ApplicationException());
            }
        }

        /// The function used to return field used for sorting
        /// </summary>
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<BaseModel, object> GetCustomerCommentOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((MODEL.CustomerComment)obj).InvoiceCustomer;
                    break;

                case 3:
                    sortFunction = obj => ((MODEL.CustomerComment)obj).Company;
                    break;

                case 4:
                    sortFunction = obj => ((MODEL.CustomerComment)obj).Comment;
                    break;

                default:
                    sortFunction = obj => ((MODEL.CustomerComment)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}