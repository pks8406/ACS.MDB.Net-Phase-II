using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        /// <summary>
        /// Returns due year detail view
        /// </summary>
        // GET: /Administration/DueYearDetail
        public ActionResult DueYearDetail()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            return View(reportModel);
        }

        /// <summary>
        /// Generate due year detail report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The Dueyeardetail view</returns>
        //[AsyncTimeout(10000000)]
        public ActionResult GenerateDueYearDetailReport(ReportModel reportModel)
        {
            ReportModel dueYearReportModel = null;
            try
            {
                dueYearReportModel = new ReportModel() { OAcompanyList = Session.GetUserAssociatedCompanyList() };

                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }

                if (reportModel.StartDate != null && reportModel.EndDate != null)
                {
                    DateTime startDate = Convert.ToDateTime(reportModel.StartDate).Date;
                    DateTime endDate = Convert.ToDateTime(reportModel.EndDate).Date;
                    if (startDate <= endDate)
                    {
                        dueYearReportModel.UserID = Session.GetUserId();
                        dueYearReportModel.CompanyID = reportModel.CompanyID;
                        //dueYearReportModel.RevenueStartDate = reportModel.RevenueStartDate;
                        //dueYearReportModel.RevenueEndDate = reportModel.RevenueEndDate;
                        dueYearReportModel.StartDate = reportModel.StartDate;
                        dueYearReportModel.EndDate = reportModel.EndDate;
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

            return View("DueYearDetail", dueYearReportModel);
        }
    }
}