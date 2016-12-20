using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Returns Completed Contracts view
        /// </summary>
        // GET: /ReportController.CompletedContracts/
        public ActionResult CompletedContracts()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            return View(reportModel);
        }

        /// <summary>
        /// Generate Completed Contracts report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The CompletedContracts view</returns>
        //[AsyncTimeout(10000000)]
        public ActionResult GenerateCompletedContractsReport(ReportModel reportModel)
        {
            ReportModel completedContractsReportModel = null;

            try
            {
                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }
                completedContractsReportModel = new ReportModel()
                {
                    OAcompanyList = Session.GetUserAssociatedCompanyList(),
                    DivisionList = GetDivisionListById(reportModel.CompanyID)
                };

                if (reportModel.StartDate != null && reportModel.EndDate != null)
                {
                    //Converts the date into mm/dd/yyyy
                    DateTime startDate = Convert.ToDateTime(reportModel.StartDate).Date;
                    DateTime endDate = Convert.ToDateTime(reportModel.EndDate).Date;
                    if (startDate <= endDate)
                    {
                        completedContractsReportModel.UserID = Session.GetUserId();
                        completedContractsReportModel.CompanyID = reportModel.CompanyID;
                        if (reportModel.DivisionID == null || reportModel.DivisionID == -1)
                        {
                            completedContractsReportModel.DivisionID = 0;
                        }
                        else
                        {
                            completedContractsReportModel.DivisionID = reportModel.DivisionID;
                        }
                        completedContractsReportModel.StartDate = reportModel.StartDate;
                        completedContractsReportModel.EndDate = reportModel.EndDate;
                    }
                    else
                    {
                        ModelState.AddModelError("", Constants.START_DATE_SHOULD_NOT_BE_GREATER_THAN_END_DATE);
                    }
                }

                //else
                //{
                //    ModelState.AddModelError("", Constants.START_DATE_AND_END_DATE_ARE_MANDATORY);
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("CompletedContracts", completedContractsReportModel);
        }        
        
    }
}