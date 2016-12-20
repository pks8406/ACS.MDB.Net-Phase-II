using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess;

namespace ACS.MDB.Net.App.Services
{
    public class ActualRevenueReportService
    {
        private ActualRevenueReportDAL actualRevenueReportDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActualRevenueReportService()
        {
            actualRevenueReportDAL = new ActualRevenueReportDAL();
        }

        /// <summary>
        /// Get the Datatable of Revenue report
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteActualReport(DateTime startDate, DateTime endDate, int companyId)
        {
            return actualRevenueReportDAL.GetActualRevenueReport(startDate, endDate, companyId);
        }

        /// <summary>
        /// Check whether data is available for reports
        /// </summary>
        /// <param name="startDate">Start date provided by user</param>
        /// <param name="endDate">End date provided by user</param>
        /// <param name="companyId">Company provided by user</param>
        /// <param name="userId">Logged in user id</param>
        /// <returns></returns>
        public bool IsActualRevenueReportDataAvailable(DateTime startDate, DateTime endDate, int companyId)
        {
            return actualRevenueReportDAL.IsActualRevenueReportDataAvailable(startDate, endDate, companyId);
        }
    }
}