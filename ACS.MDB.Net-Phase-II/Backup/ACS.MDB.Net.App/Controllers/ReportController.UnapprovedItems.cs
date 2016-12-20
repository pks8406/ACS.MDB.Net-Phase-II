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
        /// Returns Unapproved Items view
        /// </summary>
        // GET: /ReportController.UnapprovedItems/
        public ActionResult UnapprovedItems()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            return View(reportModel);
        }

        /// <summary>
        /// Generate  Unapproved Items report
        /// </summary>
        /// <param name="reportModel">The report model</param>
        /// <returns>The  Unapproved Items view</returns>
        //[AsyncTimeout(10000000)]
        public ActionResult GenerateUnapprovedItemsReport(ReportModel reportModel)
        {
            ReportModel UnapprovedItemsReportModel = null;
            try
            {
                if (reportModel.CompanyID == null)
                {
                    reportModel.CompanyID = 0;
                }

                UnapprovedItemsReportModel = new ReportModel()
                {
                    OAcompanyList = Session.GetUserAssociatedCompanyList(),
                    InvoiceCustomerList = GetInvoiceCustomerListByCompanyID(reportModel.CompanyID),
                    DivisionList = GetDivisionListById(reportModel.CompanyID)                    
                };

                if (reportModel.StartDate != null && reportModel.EndDate != null)
                {
                    //Converts the date into mm/dd/yyyy
                    //DateTime startDate = Convert.ToDateTime(reportModel.StartDate).Date;
                    //DateTime endDate = Convert.TDateTime(reportModel.EndDate).Date;
                    if (reportModel.StartDate <= reportModel.EndDate)
                    {
                        UnapprovedItemsReportModel.UserID = Session.GetUserId();
                        UnapprovedItemsReportModel.CompanyID = reportModel.CompanyID;
                        if ((reportModel.InvoiceCustomerID == null || reportModel.InvoiceCustomerID == -1) && 
                            (reportModel.DivisionID == null || reportModel.DivisionID == -1))
                        {
                            UnapprovedItemsReportModel.InvoiceCustomerID = 0;
                            UnapprovedItemsReportModel.DivisionID = 0;
                        }
                        else if ((reportModel.InvoiceCustomerID == null || reportModel.InvoiceCustomerID == -1) &&
                                (reportModel.DivisionID != null || reportModel.DivisionID != -1))
                        {
                            UnapprovedItemsReportModel.InvoiceCustomerID = 0;
                            UnapprovedItemsReportModel.DivisionID = reportModel.DivisionID;
                        }
                        else if ((reportModel.DivisionID == null || reportModel.DivisionID == -1)&&
                                (reportModel.InvoiceCustomerID != null || reportModel.InvoiceCustomerID != -1))
                        {
                            UnapprovedItemsReportModel.InvoiceCustomerID = reportModel.InvoiceCustomerID;
                            UnapprovedItemsReportModel.DivisionID = 0;
                        }
                        else
                        {
                            UnapprovedItemsReportModel.InvoiceCustomerID = reportModel.InvoiceCustomerID;
                            UnapprovedItemsReportModel.DivisionID = reportModel.DivisionID;
                        }
                        //if (reportModel.EndUserID == null || reportModel.EndUserID == "-1" || reportModel.EndUserID == "0")
                        //{
                        //    UnapprovedItemsReportModel.EndUserID = "ALL";
                        //}
                        //else
                        //{
                        //    UnapprovedItemsReportModel.EndUserID = reportModel.EndUserID;
                        //}
                        UnapprovedItemsReportModel.StartDate = reportModel.StartDate;
                        UnapprovedItemsReportModel.EndDate = reportModel.EndDate;
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

            return View("UnapprovedItems", UnapprovedItemsReportModel);
        }

        #endregion Public Methods
    }
}