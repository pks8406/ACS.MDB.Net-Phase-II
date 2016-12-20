using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ContractController : BaseController
    {
        //
        // GET: /ContractController.OpenAccount/

        public ActionResult OpenAccountIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OpenAccountIndex(OpenAccount openAccount)
        {
            try
            {
                //Get user id
                int? userId = Session.GetUserId();
                OpenAccountService openAccountService = new OpenAccountService();
                openAccountService.Start(userId);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeAndErrorResult(500, e.Message);
            }

            return View();
        }
    }
}
