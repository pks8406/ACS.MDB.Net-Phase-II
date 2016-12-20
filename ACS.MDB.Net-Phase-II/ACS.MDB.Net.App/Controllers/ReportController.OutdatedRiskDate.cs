using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        /// <summary>
        /// Returns Outdated Ris Date view
        /// </summary>
        /// <returns></returns>
        public ActionResult OutdatedRiskDate()
        {
            ReportModel reportModel = new ReportModel();
            return View(reportModel);
        }

        /// <summary>
        /// Generate Outdated Risk Date Report
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns>Outdated Risk Date report</returns>
        public ActionResult GenerateOutdatedRiskDateReport(ReportModel reportModel)
        {
            ReportModel outdatedRiskDateModel = null;

            try
            {
                outdatedRiskDateModel = new ReportModel();
                if (reportModel.ReportDate != null)
                {
                    outdatedRiskDateModel.UserID = Session.GetUserId();
                    outdatedRiskDateModel.ReportDate = reportModel.ReportDate;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View("OutdatedRiskDate", outdatedRiskDateModel);
        }
    }
}