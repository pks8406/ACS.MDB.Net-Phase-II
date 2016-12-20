using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Net.App.FileUtility;
using ACS.MDB.Net.App.Models;
using ACS.MDB.Net.App.Services;
using OfficeOpenXml;

namespace ACS.MDB.Net.App.Controllers
{
    public partial class ReportController 
    {
        //
        // GET: /ReportController.ActualRevenueForecast/

        /// <summary>
        /// Theoretical Revenue Forecast
        /// </summary>
        /// <returns></returns>
        public ActionResult ActualRevenueForecast()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            //reportModel.CompanyID = Session.GetDefaultCompanyId();
            return View(reportModel);
        }

        /// <summary>
        /// Return Datatable
        /// </summary>
        /// <param name="startDate">start Date</param>
        /// <param name="endDate">end Date</param>
        /// <param name="companyId">company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteActualReport(DateTime startDate, DateTime endDate, int companyId)
        {
            ActualRevenueReportService actualRevenueReportService = new ActualRevenueReportService();

            var result = actualRevenueReportService.ExecuteActualReport(startDate, endDate, companyId);
            return result;
        }

        /// <summary>
        /// Generate & download Theoreticle report in excel format
        /// </summary>
        /// <param name="sDate">Start date</param>
        /// <param name="eDate">End date</param>
        /// <param name="companyId">Selected company id</param>
        public void GetActualRevenueReport(string sDate, string eDate, int companyId)
        {
            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);

            //Set File Name in .xlsx format
            string fileName = "Actual_Revenue_Forecast" + " _ " + companyId + "(" +
                              startDate.ToString("dd'-'MM'-'yyyy") + " To " +
                              endDate.ToString("dd'-'MM'-'yyyy") + ")" + ".xlsx";

            //Added data in datatable
            DataTable dataTable = ExecuteActualReport(startDate, endDate, companyId);

            // Remove null columns for data table
            // 26th is the index column number from where dyanamic month wise column start generating
            RemoveExtraColumns(dataTable, startDate, endDate, 26);

            ExcelUtility excelUtility = new ExcelUtility();

            // Generate excel package
            ExcelPackage excelPackage = excelUtility.GenerateExcelReport(dataTable, "Actual Revenue Forecast");

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();
        }

        /// <summary>
        /// Theoretical revenue report
        /// Check whether data is available for specified parameter 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult IsActualRevenueDataAvailable(string sDate, string eDate, int companyId)
        {
            ActualRevenueReportService actualRevenueReportService = new ActualRevenueReportService();

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);

            DateTime afterFiveYearDate = startDate.AddYears(5);

            afterFiveYearDate = afterFiveYearDate.AddDays(-1);

            if (endDate > afterFiveYearDate)
            {
                return new HttpStatusCodeAndErrorResult(500, String.Format("Please enter valid 'End date', It should not  be more than 5 years from Start Date"));
            }

            bool isDataFound = actualRevenueReportService.IsActualRevenueReportDataAvailable(startDate, endDate, companyId);

            if (isDataFound)
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.DATA_NOT_FOUND));
        }
    }
}
