using System;
using System.Data;
using System.Globalization;
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
        // GET: /ReportController.TheoreticalRevenueForecast/

        /// <summary>
        /// Theoretical Revenue Forecast
        /// </summary>
        /// <returns></returns>
        public ActionResult TheoreticalRevenueForecast()
        {
            ReportModel reportModel = new ReportModel();
            reportModel.OAcompanyList = Session.GetUserAssociatedCompanyList();
            //reportModel.CompanyID = Session.GetDefaultCompanyId();
            return View(reportModel);
        }

        /// <summary>
        /// Return data table of Theoritical Revenue section
        /// </summary>
        /// <param name="startDate">start Date</param>
        /// <param name="endDate">end Date</param>
        /// <param name="companyId">company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            TheoreticalRevenueReportService revenueReportService = new TheoreticalRevenueReportService();

            var result = revenueReportService.ExecuteRevenueReport(startDate, endDate, companyId);
            return result;
        }

        /// <summary>
        /// Return data table of Additional Revenue section
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable ExecuteAdditionalRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            TheoreticalRevenueReportService revenueReportService = new TheoreticalRevenueReportService();

            var result = revenueReportService.ExecuteAdditionalRevenueReport(startDate, endDate, companyId);
            return result;
        }

        /// <summary>
        /// Return data table of Revenue At Risk section
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable ExecuteRevenueAtRiskReport(DateTime startDate, DateTime endDate, int companyId)
        {
            TheoreticalRevenueReportService revenueReportService = new TheoreticalRevenueReportService();

            var result = revenueReportService.ExecuteRevenueAtRiskReport(startDate, endDate, companyId);
            return result;
        }

        /// <summary>
        /// Generate & download Theoreticle report in excel format
        /// </summary>
        /// <param name="sDate">Start date</param>
        /// <param name="eDate">End date</param>
        /// <param name="companyId">Selected company id</param>
        public void GetExcel(string sDate, string eDate, int companyId)
        {
            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);

            //Set File Name in .xlsx format
            string fileName = "Theoretical_Revenue_Forecast" + " _ " + companyId + "(" +
                              startDate.ToString("dd'-'MM'-'yyyy") + " To " +
                              endDate.ToString("dd'-'MM'-'yyyy") + ")" + ".xlsx";
            ExcelPackage excelPackage = null;
            ExcelUtility excelUtility = new ExcelUtility();

            //Added data in datatable
            DataTable theoriticalRevenueDatatable = ExecuteRevenueReport(startDate, endDate, companyId);

            // Remove null columns for data table
            // 25th is the index column number from where dyanamic month wise column start generating
            RemoveExtraColumns(theoriticalRevenueDatatable, startDate, endDate, 25);

            //Added data in datatable
            DataTable additionalRevenueDatatable = ExecuteAdditionalRevenueReport(startDate, endDate, companyId);

            // Remove null columns for data table
            // 26th is the index column number from where dyanamic month wise column start generating as new field "ForecastBillingStartDate" field is added
            RemoveExtraColumns(additionalRevenueDatatable, startDate, endDate, 26);

            //Added data in datatable
            DataTable revenueAtRiskDatatable = ExecuteRevenueAtRiskReport(startDate, endDate, companyId);

            // Remove null columns for data table
            // 26th is the index column number from where dyanamic month wise column start generating as mew field "EarlyTerminationDate" field is added
            RemoveExtraColumns(revenueAtRiskDatatable, startDate, endDate, 26);

           
            //ExcelPackage excelPackage = excelUtility.GenerateExcelReport(dataTable, "Theoretical Revenue Forecast");
            // Generate excel package
            using (excelPackage = new ExcelPackage())
            {
                excelPackage = excelUtility.GenerateExcelReport(theoriticalRevenueDatatable, "Theoretical Revenue Forecast");
                excelPackage = excelUtility.GenerateExcelReport(additionalRevenueDatatable, "Additional Revenue Forecast");
                excelPackage = excelUtility.GenerateExcelReport(revenueAtRiskDatatable, "Revenues At Risk");
            }

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
        public ActionResult IsDataAvailable(string sDate, string eDate, int companyId)
        {
            TheoreticalRevenueReportService revenueReportService = new TheoreticalRevenueReportService();

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);

            DateTime afterFiveYearDate = startDate.AddYears(5);

            afterFiveYearDate = afterFiveYearDate.AddDays(-1);

            if (endDate > afterFiveYearDate)
            {
                return new HttpStatusCodeAndErrorResult(500, String.Format("Please enter valid 'End date', It should not  be more than 5 years from Start Date"));
            }
            
            bool isDataFound = revenueReportService.IsDataAvailable(startDate, endDate, companyId);

            if (isDataFound)
            {
                // If data found then send 200 OK
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.DATA_NOT_FOUND));
        }

        ///// <summary>
        ///// Remove columns whose date is not between start date and end date
        ///// </summary>
        ///// <param name="dataTable">Data table</param>
        ///// <param name="startDate">Start date provided by user</param>
        ///// <param name="endDate">end date provided by user</param>
        //private void RemoveExtraColumns(DataTable dataTable, DateTime startDate, DateTime endDate)
        //{
        //    //Remove extra columns from datatable onwards 21 column (column no 21 = 'No of Days')
        //    for (int col = dataTable.Columns.Count - 1; col >= 21; col--)
        //    {
        //        string colunmName = Convert.ToString(dataTable.Columns[col]);

        //        string[] dates = colunmName.Split('_');

        //        if (dates.Length != 2) continue;
        //        int year = Convert.ToInt32(dates[1]);

        //        int monthNumber = DateTime.ParseExact(dates[0], "MMM", CultureInfo.CurrentCulture).Month;

        //        DateTime currentDate = new DateTime(year, monthNumber, 1);

        //        if (currentDate < startDate || currentDate > endDate)
        //        {
        //            // Do not remove start date month & end date month
        //            if ((currentDate.Year != startDate.Year || currentDate.Month != startDate.Month) &&
        //                (currentDate.Year != endDate.Year || currentDate.Month != endDate.Month))
        //            {
        //                dataTable.Columns.RemoveAt(col);
        //            }
        //        }
        //    }
        //}
    }
}