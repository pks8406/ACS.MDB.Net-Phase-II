using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        /// <summary>
        /// Returns Outdated Start Date view
        /// </summary>
        /// <returns></returns>
        public ActionResult OutdatedStartDate()
        {
            ReportModel reportModel = new ReportModel();
            return View(reportModel);
        }

        /// <summary>
        /// Generate Outdated Start Date Report
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns>Outdated Start Date report</returns>
        public ActionResult GenerateOutdatedStartDateReport(ReportModel reportModel)
        {
            ReportModel outdatedStartDateModel = null;

            try
            {
                outdatedStartDateModel = new ReportModel();
                if (reportModel.ReportDate != null)
                {
                    outdatedStartDateModel.UserID = Session.GetUserId();
                    outdatedStartDateModel.ReportDate = reportModel.ReportDate;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View("OutdatedStartDate", outdatedStartDateModel);
        }
    }
}