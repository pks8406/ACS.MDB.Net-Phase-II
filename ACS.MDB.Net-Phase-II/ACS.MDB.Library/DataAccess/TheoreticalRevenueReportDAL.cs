using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using System.Data;
using System.Data.SqlClient;
using ACS.MDB.Library.Common;

namespace ACS.MDB.Library.DataAccess
{
    public class TheoreticalRevenueReportDAL : BaseDAL
    {
        /// <summary>
        /// Get the Datatable of Theoretical Revenue report
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public DataTable ExecuteRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            SqlCommand command = new SqlCommand
            (Constants.TheoreticalRevenueReport, connection);

            // Datatable object
            DataTable table = new DataTable();

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter RetVal = command.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            //Start date parameter for stored procedure
            SqlParameter startDateForSP = command.Parameters.Add("@StartDate", SqlDbType.DateTime);
            startDateForSP.Direction = ParameterDirection.Input;
            
            //End date parameter for stored procedure
            SqlParameter endDateForSP = command.Parameters.Add("@EndDate", SqlDbType.DateTime);
            endDateForSP.Direction = ParameterDirection.Input;

            //Selected company id
            SqlParameter companyIdForSP = command.Parameters.Add("@CompanyID", SqlDbType.Int);
            companyIdForSP.Direction = ParameterDirection.Input;

            //Set parameter value for SP
            startDateForSP.Value = startDate;
            endDateForSP.Value = endDate;
            companyIdForSP.Value = companyId;

            using (var dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(table);
            }

            //Close ADO.net connection
            connection.Close();
            connection.Dispose();

            return table;
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
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            SqlCommand command = new SqlCommand
            ("usp_Additional_Revenue_Forecast", connection);

            // Datatable object
            DataTable table = new DataTable();

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter RetVal = command.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            //Start date parameter for stored procedure
            SqlParameter startDateForSP = command.Parameters.Add("@StartDate", SqlDbType.DateTime);
            startDateForSP.Direction = ParameterDirection.Input;

            //End date parameter for stored procedure
            SqlParameter endDateForSP = command.Parameters.Add("@EndDate", SqlDbType.DateTime);
            endDateForSP.Direction = ParameterDirection.Input;

            //Selected company id
            SqlParameter companyIdForSP = command.Parameters.Add("@CompanyID", SqlDbType.Int);
            companyIdForSP.Direction = ParameterDirection.Input;

            //Set parameter value for SP
            startDateForSP.Value = startDate;
            endDateForSP.Value = endDate;
            companyIdForSP.Value = companyId;

            using (var dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(table);
            }

            //Close ADO.net connection
            connection.Close();
            connection.Dispose();

            return table;
        }

        /// <summary>
        /// Get the Datatable of Revenue At risk report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable ExecuteRevenueAtRiskReport(DateTime startDate, DateTime endDate, int companyId)
        {
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            SqlCommand command = new SqlCommand
            ("usp_Revenue_AtRisk_Forecast", connection);

            // Datatable object
            DataTable table = new DataTable();

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter RetVal = command.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            //Start date parameter for stored procedure
            SqlParameter startDateForSP = command.Parameters.Add("@StartDate", SqlDbType.DateTime);
            startDateForSP.Direction = ParameterDirection.Input;

            //End date parameter for stored procedure
            SqlParameter endDateForSP = command.Parameters.Add("@EndDate", SqlDbType.DateTime);
            endDateForSP.Direction = ParameterDirection.Input;

            //Selected company id
            SqlParameter companyIdForSP = command.Parameters.Add("@CompanyID", SqlDbType.Int);
            companyIdForSP.Direction = ParameterDirection.Input;

            //Set parameter value for SP
            startDateForSP.Value = startDate;
            endDateForSP.Value = endDate;
            companyIdForSP.Value = companyId;

            using (var dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(table);
            }

            //Close ADO.net connection
            connection.Close();
            connection.Dispose();

            return table;
        }

        /// <summary>
        /// Check whetherTheoretical Revenue report has data for specific parameter
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public bool IsDataAvailable(DateTime startDate, DateTime endDate, int companyId)
        {
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            SqlCommand command = new SqlCommand
            ("usp_Theoretical_Revenue_Forecast", connection);

            // Datatable object
            DataTable table = new DataTable();

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter RetVal = command.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            //Start date parameter for stored procedure
            SqlParameter startDateForSP = command.Parameters.Add("@StartDate", SqlDbType.DateTime);
            startDateForSP.Direction = ParameterDirection.Input;

            //End date parameter for stored procedure
            SqlParameter endDateForSP = command.Parameters.Add("@EndDate", SqlDbType.DateTime);
            endDateForSP.Direction = ParameterDirection.Input;

            //Selected company id
            SqlParameter companyIdForSP = command.Parameters.Add("@CompanyID", SqlDbType.Int);
            companyIdForSP.Direction = ParameterDirection.Input;

            //Set parameter value for SP
            startDateForSP.Value = startDate;
            endDateForSP.Value = endDate;
            companyIdForSP.Value = companyId;

            using (var dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(table);
            }

            //Close ADO.net connection
            connection.Close();
            connection.Dispose();

            return table.Rows.Count > 0;
        }
    }
}
