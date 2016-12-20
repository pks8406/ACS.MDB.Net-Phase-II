using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;


namespace ACS.MDB.Net.App.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// Returns home page view
        /// </summary>
        /// <returns></returns>
        [SessionHandler]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns about page view
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "ReleaseNotes/ReleaseNote.txt";

                ReleaseNoteService releaseNoteService = new ReleaseNoteService();
                List<string> releaseNotes = releaseNoteService.GetReleaseNote(filePath);

                AboutUs aboutUs = new AboutUs();

                // Set property value
                aboutUs.ReleaseNotes = releaseNotes;
                aboutUs.VersionNumber = ApplicationConfiguration.GetARBSVersionNumber();
                aboutUs.LiveDate = ApplicationConfiguration.GetARBSLiveDate();

                //ViewBag.Message = " Advanced Recurring Billing System (ARBS) - Description.";
                return View("About", aboutUs);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }
        }

        /// <summary>
        /// Returns contact us page view
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = " Advanced Recurring Billing System (ARBS) - Contact details.";
            return View();
        }
    }
}