using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController
    {

        /// GET: /ContractController.ApproveMaintenance/
        /// <summary>
        /// Gets approve maintenance view
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveMaintenanceIndex()
        {
            var approveMaintenance = new ApproveMaintenance();
            try
            {
                approveMaintenance.OAcompanyList = Session.GetUserAssociatedCompanyList();
                
                approveMaintenance.CompanyId = Session.GetDefaultCompanyId();

                //Get all active divisions associated with the company
                var divisionService = new DivisionService();
                List<DivisionVO> divisionListVO = divisionService.GetDivisionListByCompany(approveMaintenance.CompanyId);

                foreach (var division in divisionListVO)
                {
                    approveMaintenance.DivisionList.Add(new Division(division));
                }

                //Get all invoice customers associated with the company
                var invoiceCustomerService = new InvoiceCustomerService();
                List<InvoiceCustomerVO> invoiceCustomerVOList = invoiceCustomerService.GetInvoiceCustomerList(approveMaintenance.CompanyId.Value);
                foreach (var item in invoiceCustomerVOList)
                {
                    approveMaintenance.InvoiceCustomerList.Add(new InvoiceCustomer(item));
                }

                var milestoneStatusService = new MilestoneStatusService();
                List<MilestoneStatusVO> milestoneStatusVOList = milestoneStatusService.GetMilestoneStatusList();
                foreach (var item in milestoneStatusVOList)
                {
                    approveMaintenance.MilestoneStatusList.Add(new MilestoneStatus(item));
                }
                approveMaintenance.MilestoneStatusId = 9;
            }
            catch (Exception)
            {
                //throw;
            }
            return View(approveMaintenance);
        }

        /// <summary>
        /// Get Division List
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>List of Division</returns>
        public JsonResult GetDivisionList(int companyId)
        {
            try
            {
                List<DivisionVO> divisionVOList = null;
               
                if (companyId != -1)
                {
                    //Get division list associated with single company
                    var divisionService = new DivisionService();
                    divisionVOList = divisionService.GetDivisionListByCompany(companyId);
                }
                return Json(divisionVOList);
            }
            catch (Exception e)
            {
                return Json(new ApplicationException(e.Message));
            }
        }

        /// <summary>
        /// Get list of milestones for approve maintenance
        /// </summary>
        /// <param name="param">Datatable parameter</param>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">customer id</param>
        /// <param name="divisionId">division id</param>
        /// <param name="milestoneStatusId">milestone status id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>return json for datatable</returns>
        public ActionResult GetApproveMaintenanceMilestonesList(jQueryDataTableParamModel param,
                                                                int? companyId, int? invoiceCustomerId, int? divisionId,
                                                                int? milestoneStatusId,
                                                                string startDate, string endDate)
        {
            try
            {
                var approveMaintenanceService = new ApproveMaintenanceService();
                var approveMaintenanceList = new List<ApproveMaintenance>();
                
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    DateTime fromDate = DateTime.Parse(startDate);
                    DateTime toDate = DateTime.Parse(endDate);

                    //Get user id
                    int? userId = Session.GetUserId();
                    List<MilestoneVO> milestoneVOList = approveMaintenanceService.GetApproveMaintenance(companyId,
                                                                                                        invoiceCustomerId,
                                                                                                        divisionId,
                                                                                                        milestoneStatusId,
                                                                                                        fromDate, toDate,
                                                                                                        userId);

                    approveMaintenanceList.AddRange(milestoneVOList.Select(item => new ApproveMaintenance(item)));
                }

                //get the field on with sorting needs to happen and set the
                //ordering function/delegate accordingly.
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var orderingFunction = GetApproveMaintenanceOrderingFunction(sortColumnIndex);

                var result = GetFilteredObjects(param, approveMaintenanceList, orderingFunction);
                return result;
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Generate invoice of milestone which has status "Approved for Payment" and approved
        /// in text file as per Open Account format
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateInvoice(int companyId, int divisionId, int invoiceCustomerId, 
            string startDate, string endDate, string invoiceDate)
        {
            try
            {

                var approveMaintenanceService = new ApproveMaintenanceService();

                DateTime before14DaysDate = DateTime.Now.AddDays(-14);

                if (Convert.ToDateTime(invoiceDate) < before14DaysDate)
                {
                    return new HttpStatusCodeAndErrorResult(500,
                                                            "Invoice date cannot be less than 14 days from today's date");
                }
                else
                {
                    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                    {
                        //Get user id
                        int? userId = Session.GetUserId();
                        DateTime fromDate = DateTime.Parse(startDate);
                        DateTime toDate = DateTime.Parse(endDate);
                        DateTime invDate = DateTime.Parse(invoiceDate);

                        try
                        {
                            approveMaintenanceService.GenerateInvoice(companyId, divisionId, invoiceCustomerId,
                                                                      fromDate, toDate, userId, invDate);

                            // Set milestone status as link loaded after generation of flat file
                            approveMaintenanceService.UpdateMilestoneStatus(companyId, divisionId, invoiceCustomerId,
                                                                            fromDate, toDate, userId,
                                                                            Convert.ToInt32(
                                                                                Constants.MilestoneStatus.LINK_LOADED), invDate);

                            return new HttpStatusCodeResult(200);
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Equals(Constants.NO_MILESTONE_FOUND_FOR_AP))
                            {
                                // Revert back milestone
                                approveMaintenanceService.UpdateMilestoneStatus(companyId, divisionId, invoiceCustomerId,
                                                                                fromDate, toDate, userId,
                                                                                Convert.ToInt32(
                                                                                    Constants.MilestoneStatus
                                                                                             .APPROVED_FOR_PAYMENT), DateTime.Now);
                            }

                            throw ex;
                        }
                    }
                    else
                    {
                        return new HttpStatusCodeAndErrorResult(500, "Date cannot be null.");
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Download approve maintenance log File 
        /// </summary>
        public void DownloadErrorLog()
        {
            string filePath = ApplicationConfiguration.GetBillToOAErrorLogPath();
            string fileName = Constants.BILL_TO_OA_ERROR_LOG_FILENAME;
            int? userId = Session.GetUserId();
            
            // Add user id to generate unique file for user
            fileName += userId + ".txt";

            filePath += "/" + fileName ;

            if (System.IO.File.Exists(filePath))
            {
                //fileName = Path.GetFileName(filePath);
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.TransmitFile((filePath));
                Response.End();
            }
        }

        /// <summary>
        /// Edit the Milestone
        /// </summary>
        /// <param name="id">Milestone Id</param>
        /// <returns>Milestone details view</returns>
        public ActionResult ApproveMaintenanceEdit(int id)
        {
            var milestone = new Milestone();

            try
            {
                //Get milestone details
                var milestoneService = new MilestoneService();
                MilestoneVO milestoneVO = milestoneService.GetMilestoneById(id);

                if (milestoneVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.MILESTONE));
                }
                else
                {
                    milestone = new Milestone(milestoneVO) { MilestoneStatusList = GetMilestoneStatusList() };
                    //FillMilestoneBillingLines(milestone, milestoneVO.MilestoneBillingLines);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("ApproveMaintenanceDetail", milestone);
        }

        /// <summary>
        /// Get Milestone billing lines
        /// </summary>
        /// <param name="id">Milestone Id</param>
        /// <returns>Milestone billing lines view</returns>
        public ActionResult GetMilestoneBillingLines(int id)
        {
            var milestone = new Milestone();

            try
            {
                //Get milestone details
                var milestoneService = new MilestoneService();
                MilestoneVO milestoneVO = milestoneService.GetMilestoneById(id);

                if (milestoneVO == null)
                {
                    ModelState.AddModelError("", String.Format(Constants.ITEM_NOT_FOUND, Constants.MILESTONE));
                }
                else
                {
                    milestone = new Milestone(milestoneVO) { MilestoneStatusList = GetMilestoneStatusList() };
                    FillMilestoneBillingLines(milestone, milestone.MilestoneBillingLines);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView("_BillingLines", milestone);
        }

        /// <summary>
        /// Save billing line from approve maintenance page - Inline billing line saving
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult SaveBillingLines(Milestone model)
        {
            try
            {
                var milestoneService = new MilestoneService();

                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    //ApproveMaintenanceVO approveMaintenanceVO = new ApproveMaintenanceVO(model, userId);
                    //var milestoneVO = new MilestoneVO(model, userId);

                    var milestoneVO = model.Transpose(userId);

                    milestoneService.SaveBillingLines(milestoneVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.MILESTONE));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Save the milestone
        /// </summary>
        /// <param name="model">The Milestone model</param>
        [ValidateInput(false)]
        public ActionResult ApproveMaintenanceSave(Milestone model)
        {
            try
            {
                var milestoneService = new MilestoneService();

                if (ModelState.IsValid)
                {
                    //Get user id
                    int? userId = Session.GetUserId();
                    //ApproveMaintenanceVO approveMaintenanceVO = new ApproveMaintenanceVO(model, userId);
                    //var milestoneVO = new MilestoneVO(model, userId);

                    var milestoneVO = model.Transpose(userId);

                    milestoneService.UpdateMilestone(milestoneVO);
                    return new HttpStatusCodeResult(200);
                }
                else
                {
                    throw new ApplicationException(String.Format(Constants.CANNOT_SAVE, Constants.MILESTONE));
                }
            }
            catch (ApplicationException e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Delete approve maintenance
        /// </summary>
        /// <param name="Ids">Ids of milestones to be deleted</param>
        public ActionResult ApproveMaintenanceDelete(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                var approveMaintenanceService = new ApproveMaintenanceService();
                approveMaintenanceService.DeleteApproveMaintenance(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Approve all selected milestones.
        /// </summary>
        /// <param name="Ids">The milestone id list</param>
        /// <returns></returns>
        public ActionResult ApproveAll(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                var approveMaintenanceService = new ApproveMaintenanceService();
                approveMaintenanceService.ApproveAllMaintenance(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// UnApprove all selected milestones.
        /// </summary>
        /// <param name="Ids">The milestone id list</param>
        /// <returns></returns>
        public ActionResult UnApproveAll(List<int> Ids)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();

                var approveMaintenanceService = new ApproveMaintenanceService();
                approveMaintenanceService.UnApproveAllMaintenance(Ids, userId);
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// The function used to return field used for sorting
        /// <param name="sortCol">The column number on which sorting needs to happen</param>
        /// <returns>returns the field to sorted</returns>
        public Func<BaseModel, object> GetApproveMaintenanceOrderingFunction(int sortCol)
        {
            Func<BaseModel, object> sortFunction;
            switch (sortCol)
            {
                case 2:
                    sortFunction = obj => ((ApproveMaintenance)obj).DivisionName;
                    break;

                case 3:
                    sortFunction = obj => ((ApproveMaintenance)obj).InvoiceCustomer;
                    break;

                case 4:
                    sortFunction = obj => ((ApproveMaintenance)obj).ContractNumber;
                    break;

                case 5:
                    sortFunction = obj => ((ApproveMaintenance)obj).InvoiceDate;
                    break;


                case 6:
                    sortFunction = obj => ((ApproveMaintenance)obj).Amount;
                    break;

                case 7:
                    sortFunction = obj => ((ApproveMaintenance)obj).IsApproved;
                    break;

                case 8:
                    sortFunction = obj => ((ApproveMaintenance)obj).MilestoneDescription;
                    break;

                default:
                    sortFunction = obj => ((ApproveMaintenance)obj).ID;
                    break;
            }

            return sortFunction;
        }
    }
}