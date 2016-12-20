using System;
using System.Data;
using System.Data.SqlClient;
using ACS.MDB.Library.Common;

namespace ACS.MDB.Library.DataAccess
{
    public class ActualRevenueReportDAL : BaseDAL
    {
        /// <summary>
        /// Get the Datatable of Actual Revenue report
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public DataTable GetActualRevenueReport(DateTime startDate, DateTime endDate, int companyId)
        {
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            //connection.DataContextCommandTimeout =
              //  Convert.ToInt32(ConfigurationManager.AppSettings["DataContextCommandTimeout"]);
            
            SqlCommand command = new SqlCommand(Constants.ActualRevenueReport, connection);
            command.CommandTimeout = 0;
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
        /// Check whether Actual Revenue report has data for specific parameter
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Datatable</returns>
        public bool IsActualRevenueReportDataAvailable(DateTime startDate, DateTime endDate, int companyId)
        {
            // Create sql connection              
            SqlConnection connection = new SqlConnection(DatabaseConnection.DatabaseConnectionString);
            SqlCommand command = new SqlCommand(Constants.TheoreticalRevenueReport, connection);

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
