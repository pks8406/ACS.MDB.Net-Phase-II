using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.MDB.Sync.Library
{
    public static class SynchOAData
    {
        public static bool IsError = false;

        public static void Start(string OAConnectionString, string MDBConnectionString, string LogFilePath, int? userId)
        {
            try
            {
                OdbcConnection oaConnection = OADatabaseConnection(OAConnectionString);
                SqlConnection mdbConnection = MDBDatabaseConnection(MDBConnectionString);

                if (oaConnection.State == System.Data.ConnectionState.Open && mdbConnection.State == System.Data.ConnectionState.Open)
                {
                    CompareData compareData = new CompareData(oaConnection, mdbConnection, LogFilePath,userId);
                    compareData.Run();
                    IsError = compareData.IsError;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public static bool Start(string OAConnectionString, string MDBConnectionString, string LogFilePath)
        //{
        //    bool isError = false;
        //    try
        //    {
        //        OdbcConnection oaConnection = OADatabaseConnection(OAConnectionString);
        //        SqlConnection mdbConnection = MDBDatabaseConnection(MDBConnectionString);

        //        if (oaConnection.State == System.Data.ConnectionState.Open && mdbConnection.State == System.Data.ConnectionState.Open)
        //        {
        //            CompareData compareData = new CompareData(oaConnection, mdbConnection, LogFilePath);
        //            isError = compareData.Run();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        isError = true;
        //        throw e;
        //    }
        //    return isError;
        //}

        /// <summary>
        /// Open OpenAccount database connection
        /// </summary>
        /// <param name="oaConnectionString">OA Connection string</param>
        /// <returns>return connection object</returns>
        private static OdbcConnection OADatabaseConnection(string oaConnectionString)
        {
            OdbcConnection oaConnection = new OdbcConnection(oaConnectionString);
            oaConnection.Open();

            return oaConnection;
        }

        /// <summary>
        /// Open MDB database connection
        /// </summary>
        /// <param name="mdbConnectionString">MDB database connection string</param>
        /// <returns>return MDB connection object</returns>
        private static SqlConnection MDBDatabaseConnection(string mdbConnectionString)
        {
            SqlConnection mdbConnection = new SqlConnection(mdbConnectionString);
            mdbConnection.Open();

            return mdbConnection;
        }
    }
}
