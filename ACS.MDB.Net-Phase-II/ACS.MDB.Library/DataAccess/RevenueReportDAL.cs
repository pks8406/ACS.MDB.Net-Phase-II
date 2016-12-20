using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ACS.MDB.Library.DataAccess.LINQ;
using System.Configuration;
using ACS.MDB.Library.DataAccess;

namespace ACS.MDB.Net.App.DataAccess.LINQ
{
    public class RevenueReportDAL : BaseDAL
    {
        /// <summary>
        /// Get the Datatable of Revenue report
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteRevenueReport(DateTime startDate, DateTime endDate, int companyId, int? userId)
        {
            //int intRowAffected;                        
            SqlConnection con = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["MDBConnectionString"]));
            SqlCommand testCMD = new SqlCommand
            ("usp_Theoretical_Revenue_Forecast_New", con);

            DataTable table = new DataTable();

            testCMD.CommandType = CommandType.StoredProcedure;

            SqlParameter RetVal = testCMD.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter startDateForSP = testCMD.Parameters.Add
            ("@StartDate", SqlDbType.DateTime);
            startDateForSP.Direction = ParameterDirection.Input;

            SqlParameter endDateForSP = testCMD.Parameters.Add
               ("@EndDate", SqlDbType.DateTime);
            endDateForSP.Direction = ParameterDirection.Input;

            SqlParameter companyIdForSP = testCMD.Parameters.Add
               ("@CompanyID", SqlDbType.Int);
            companyIdForSP.Direction = ParameterDirection.Input;

            SqlParameter userIdForSP = testCMD.Parameters.Add
               ("@UserID", SqlDbType.Int);
            userIdForSP.Direction = ParameterDirection.Input;

            startDateForSP.Value = startDate;
            endDateForSP.Value = endDate;
            companyIdForSP.Value = companyId;
            userIdForSP.Value = userId;

            //con.Open();

            using (var da = new SqlDataAdapter(testCMD))
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }

            return table;
        }
    }
}