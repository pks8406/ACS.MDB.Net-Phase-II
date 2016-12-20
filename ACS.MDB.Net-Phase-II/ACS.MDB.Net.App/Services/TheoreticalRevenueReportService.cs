using ACS.MDB.Library.DataAccess;
using System;
using System.Data;

namespace ACS.MDB.Net.App.Services
{
    public class TheoreticalRevenueReportService
    {
        private TheoreticalRevenueReportDAL theoreticalRevenueReportDAL = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TheoreticalRevenueReportService()
        {
            theoreticalRevenueReportDAL = new TheoreticalRevenueReportDAL();
        }

        /// <summary>
        /// Get the Datatable of Theoritical Revenue report
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            return theoreticalRevenueReportDAL.ExecuteRevenueReport(startDate, endDate, companyId);
        }

        /// <summary>
        /// Get the Datatable of Additional Revenue report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable ExecuteAdditionalRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            return theoreticalRevenueReportDAL.ExecuteAdditionalRevenueReport(startDate, endDate, companyId);
        }

        /// <summary>
        /// Get the Datatable of Revenue At Risk report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable ExecuteRevenueAtRiskReport(DateTime startDate, DateTime endDate, int companyId)
        {
            return theoreticalRevenueReportDAL.ExecuteRevenueAtRiskReport(startDate, endDate, companyId);
        }
        /// <summary>
        /// Check whether data is available for reports
        /// </summary>
        /// <param name="startDate">Start date provided by user</param>
        /// <param name="endDate">End date provided by user</param>
        /// <param name="companyId">Company provided by user</param>
        /// <param name="userId">Logged in user id</param>
        /// <returns></returns>
        public bool IsDataAvailable(DateTime startDate, DateTime endDate, int companyId)
        {
            return theoreticalRevenueReportDAL.IsDataAvailable(startDate, endDate, companyId);
        }
    }
}