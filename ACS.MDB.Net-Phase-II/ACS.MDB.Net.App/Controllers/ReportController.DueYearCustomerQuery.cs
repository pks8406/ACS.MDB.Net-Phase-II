using System;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.Models;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController
    {
        #region Public Methods

        /// <summary>
        /// Returns Due Year Customer Query view
        /// </summary>
        // GET: /ReportController.DueYearCustomerQuery/
        public ActionResult DueYearCustQuery()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            // reportModel.InvoiceCustomerList = GetInvoiceCustomerListForAllCompanies(reportModel.OAcompanyList);
            return View(reportModel);
        }

        /// <summary>
        /// Generate Due Year Customer Query report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The Due Year Customer Query view</returns>
        //[AsyncTimeout(10000000)]
        public ActionResult GenerateDueYearCustQueryReport(ReportModel reportModel)
        {
            ReportModel dueYearCustQueryReportModel = null;

            try
            {
                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }
                dueYearCustQueryReportModel = new ReportModel()
                {
                    OAcompanyList = Session.GetUserAssociatedCompanyList(),
                    InvoiceCustomerList = GetInvoiceCustomerListByCompanyID(reportModel.CompanyID)
                    //EndUserList = GetEndUserListByCompanyID(reportModel.CompanyID)};
                };

                if (reportModel.StartDate != null && reportModel.EndDate != null)
                {
                    if (reportModel.StartDate <= reportModel.EndDate)
                    {
                        dueYearCustQueryReportModel.UserID = Session.GetUserId();
                        dueYearCustQueryReportModel.CompanyID = reportModel.CompanyID;
                        if (reportModel.InvoiceCustomerID == null || reportModel.InvoiceCustomerID == -1)
                        {
                            dueYearCustQueryReportModel.InvoiceCustomerID = 0;
                        }
                        else
                        {
                            dueYearCustQueryReportModel.InvoiceCustomerID = reportModel.InvoiceCustomerID;
                        }
                        //if (reportModel.EndUserID == null || reportModel.EndUserID == "-1" || reportModel.EndUserID == "0")
                        //{
                        //    dueYearCustQueryReportModel.EndUserID = "ALL";
                        //}
                        //else
                        //{
                        //    dueYearCustQueryReportModel.EndUserID = reportModel.EndUserID;
                        //}
                        dueYearCustQueryReportModel.StartDate = reportModel.StartDate;
                        dueYearCustQueryReportModel.EndDate = reportModel.EndDate;
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

            return View("DueYearCustQuery", dueYearCustQueryReportModel);
        }

        #endregion Public Methods
    }
}