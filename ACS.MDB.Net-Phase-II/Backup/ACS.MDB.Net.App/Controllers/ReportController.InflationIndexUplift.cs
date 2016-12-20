using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        //
        // GET: /ReportController.InflationIndexUplift/

        /// <summary>
        /// Returns Inflation Index view
        /// </summary>
        // GET: /ReportController.InflationIndexUplift
        public ActionResult InflationIndex()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            return View(reportModel);
        }

        /// <summary>
        /// Generate Inflation report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The InflationIndex view</returns>
        //[AsyncTimeout(10000000)]
        public ActionResult GenerateInflationReport(ReportModel reportModel)
        {
            ReportModel inflationReportModel = null;
            try
            {
                inflationReportModel = new ReportModel() { OAcompanyList = Session.GetUserAssociatedCompanyList() };
                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }

                if (reportModel.StartDate != null && reportModel.EndDate != null)
                {
                    //CompanyService companyService = new CompanyService();
                    //string companyName = companyService.GetCompanyNameByID(reportModel.CompanyID);

                    DateTime startDate = Convert.ToDateTime(reportModel.StartDate).Date;
                    DateTime endDate = Convert.ToDateTime(reportModel.EndDate).Date;
                    if (startDate <= endDate)
                    {
                        inflationReportModel.UserID = Session.GetUserId();
                        inflationReportModel.CompanyID = reportModel.CompanyID;
                        inflationReportModel.StartDate = reportModel.StartDate;
                        inflationReportModel.EndDate = reportModel.EndDate;
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

            return View("InflationIndex", inflationReportModel);
        }
    }
}