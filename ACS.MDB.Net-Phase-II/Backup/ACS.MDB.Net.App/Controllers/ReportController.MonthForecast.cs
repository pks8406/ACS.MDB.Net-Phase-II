using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        /// <summary>
        /// Returns month forecast view
        /// </summary>
        // GET: /Administration/MonthForecast
        public ActionResult MonthForecast()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            return View(reportModel);
        }

        /// <summary>
        /// Generate month forecast report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The month forecast report view</returns>
        public ActionResult GenerateMonthForecastReport(ReportModel reportModel)
        {
            ReportModel monthForecastReportModel = null;

            try
            {
                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }
                monthForecastReportModel = new ReportModel()
                {
                    OAcompanyList = Session.GetUserAssociatedCompanyList(),
                    DivisionList = GetDivisionListById(reportModel.CompanyID)
                };

                monthForecastReportModel.UserID = Session.GetUserId();
                monthForecastReportModel.CompanyID = reportModel.CompanyID;
                if (reportModel.DivisionID == null || reportModel.DivisionID == -1)
                {
                    monthForecastReportModel.DivisionID = 0;
                }
                else
                {
                    monthForecastReportModel.DivisionID = reportModel.DivisionID;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("MonthForecast", monthForecastReportModel);
        }
    }
}