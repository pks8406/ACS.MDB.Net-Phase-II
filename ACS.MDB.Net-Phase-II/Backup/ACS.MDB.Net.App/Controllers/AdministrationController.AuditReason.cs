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
        /// Returns audit reason index view
        /// </summary>
        /// <returns>audit reason index view</returns>
        // GET: /AuditReason/
        public ActionResult AuditReasonIndex()
        {
            return IndexViewForAuthorizeUser();
        }

        /// <summary>
        ///  Returns list of audit Reasons
        /// </summary>
        /// <param name="param">The filter an other parameters</param>
        /// <returns>List of audit reasons</returns>
        // GET: /Administration/AuditReasonList
        public ActionResult AuditReasonList(MODEL.jQueryDataTableParamModel param)
        {
            try
            {
                AuditReasonService auditReasonService = new AuditReasonService();
                List<MODEL.AuditReason> auditReasonList = new List<AuditReason>();


                //Get the list of Audit Reasons VO's
                List<AuditReasonVO> auditReasonVOList = auditReasonService.GetAuditReasonCodeVOList();

                foreach (var auditReasonVO in auditReasonVOList)
                {
                    MODEL.AuditReason auditReason = new MODEL.AuditReason(auditReasonVO);
                    auditReasonList.Add(auditReason);
                }


                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetAuditReasonOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjectsOrderByAscending(param, auditReasonList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Create new audit reason
        /// </summary>
        /// <returns></returns>
        // GET: /Administration/AuditReasonCreate
        public ActionResult AuditReasonCreate()
        {
            try
            {
                MODEL.AuditReason auditReason = new MODEL.AuditReason();
                return PartialView("AuditReasonDetails", auditReason);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Edit existing audit reason
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult AuditReasonEdit(int id)
        {
            MODEL.AuditReason auditReason = null;
            try
            {
                AuditReasonService auditReasonService = new AuditReasonService();

                //Get audit reason details
                AuditReasonVO auditReasonVO = auditReasonService.GetAuditReasonById(id);

                if (auditReasonVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.AUDIT_REASON));
                }
                else
                {
                    auditReason = new AuditReason(auditReasonVO);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("AuditReasonDetails", auditReason);
        }

        /// <summary>
        /// Save the audit reason
        /// </summary>
        /// <param name="model">The Audit Reason Details</param>
        /// <returns></returns>
        // POST: /Administration/AuditReasonSave
        [ValidateInput(false)]
        public ActionResult AuditReasonSave(MODEL.AuditReason model)
        {
            try
            {
                bool ismodelValid = ModelState.IsValid;
                if (ismodelValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    AuditReasonService auditReasonService = new AuditReasonService();
                    //AuditReasonVO auditReasonVO = new AuditReasonVO(model, userId);

                    AuditReasonVO auditReasonVO = model.Transpose(userId);

                    auditReasonService.SaveAuditReason(auditReasonVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.AUDIT_REASON));
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete the audit reasons
        /// </summary>
        /// <param name="Ids">The list of audit reason id's</param>
        /// <returns></returns>
        public ActionResult AuditReasonDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                AuditReasonService auditReasonService = new AuditReasonService();
                auditReasonService.DeleteAuditReason(Ids, userId);
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
        /// <returns></returns>
        public Func<BaseModel, object> GetAuditReasonOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction = null;
            switch (sortCol)
            {
                case 1:
                    sortFunction = obj => ((AuditReason)obj).ReasonCode;
                    break;

                default:
                    sortFunction = obj => ((AuditReason)obj).ReasonDescription;
                    break;
            }
            return sortFunction;
        }
    }
}
