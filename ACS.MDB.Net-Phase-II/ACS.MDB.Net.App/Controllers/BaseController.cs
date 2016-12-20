using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BaseController : Controller
    {
        ///<summary>
        /// Returns the view for admin and super user.
        ///</summary>
        ///<param name="model">The model to pass with view</param>
        ///<returns>The view</returns>
        protected ActionResult IndexViewForAuthorizeUser(object model = null)
        {
            string userType = Session.GetUserType();
            if (!String.IsNullOrEmpty(userType) &&
                (!userType.Equals("Admin") && !userType.Equals("SuperUser")))
            {
                return PartialView("Unauthorise");
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Function returns a list of objects filtered as per the specified
        /// search criteria. Used for populating/searching the data table grid
        /// </summary>
        /// <param name="param">The search parameters (criteria)</param>
        /// <param name="objects">The original list of entities</param>
        /// <returns>List of filtered entities</returns>
        protected JsonResult GetFilteredObjects(jQueryDataTableParamModel param, IEnumerable<BaseModel> objects,
                                                Func<BaseModel, object> orderingFunction = null)
        {
            IEnumerable<BaseModel> filteredObjects;

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
                    filteredObjects = filteredObjects.OrderByDescending(orderingFunction);
                }
                else
                {
                    if (sortDirection == "asc")
                    {
                        filteredObjects = filteredObjects.OrderBy(orderingFunction);
                    }
                    else
                    {
                        filteredObjects = filteredObjects.OrderByDescending(orderingFunction);
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
        /// Function returns a list of objects filtered as per the specified
        /// search criteria and in an ascending order. Used for populating/searching the data table grid
        /// </summary>
        /// <param name="param">The search parameters (criteria)</param>
        /// <param name="objects">The original list of entities</param>
        /// <returns>List of filtered entities in ascending order</returns>
        protected JsonResult GetFilteredObjectsOrderByAscending(jQueryDataTableParamModel param, IEnumerable<BaseModel> objects,
                                                Func<BaseModel, object> orderingFunction = null)
        {
            IEnumerable<BaseModel> filteredObjects;

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

                //If sort column (ID column) index is 0 then get the records in ascending order
                //This will happen if user has not done sorting on any columns and we want to dispaly data in asc order.
                if (sortColumnIndex == 0)
                {
                    filteredObjects = filteredObjects.OrderBy(orderingFunction);
                }
                else
                {
                    if (sortDirection == "asc")
                    {
                        filteredObjects = filteredObjects.OrderBy(orderingFunction);
                    }
                    else
                    {
                        filteredObjects = filteredObjects.OrderByDescending(orderingFunction);
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
        /// Gets list of company
        /// </summary>
        /// <returns>List of company</returns>
        protected List<Company> GetCompanyList()
        {
            List<Company> companyList = new List<Company>();
            CompanyService companyService = new CompanyService();
            List<CompanyVO> companyVOList = companyService.GetCompanyList();

            foreach (CompanyVO companyVO in companyVOList)
            {
                companyList.Add(new Company(companyVO));
            }

            return companyList;
        }

        /// <summary>
        /// Validates multi line text box. Check if enter does not exceeds provided maximum length.
        /// </summary>
        /// <param name="fieldName">The fieldname</param>
        /// <param name="fieldText">The field text</param>
        /// <param name="fieldLength">The field length</param>
        /// <returns>True if valid after replacing /r/n with /n, false otherwise.</returns>
        protected bool IsModelValidForMultilineTextbox(string fieldName, string fieldText, int fieldLength)
        {
            bool ismodelValid = false;

            //Check if the problem with description only
            foreach (var item in ModelState)
            {
                if (item.Key == fieldName && item.Value.Errors.Count > 0)
                {
                    if (fieldText.Length > fieldLength)
                    {
                        string val = fieldText.Replace("\r\n", "\n");
                        if (val.Length <= fieldLength)
                        {
                            ismodelValid = true;
                            break;
                        }
                        else
                        {
                            throw new ApplicationException(String.Format(Constants.FIELD_EXEEDS_LENGTH_FOR_MULTILINE_TEXBOX, fieldName, fieldLength));
                        }
                    }
                }
            }

            return ismodelValid;
        }
    }

    /// <summary>
    /// This class is used to checks if the session is timeout before the actual method gets call.
    /// If yes then redirect to login page.
    /// </summary>
    public class SessionHandler : ActionFilterAttribute
    {
        /// <summary>
        /// This method will be called before any request executes. It checks for session availability.
        /// If session has expired then redirect to login page.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.HttpContext.Session["UserId"] == null)
            {
                //If ajax request return the http status code result
                //else provide redirect to route result
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, Constants.SESSION_TIMEOUT);
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Logout"
                    }));
                }
            }
        }
    }
}