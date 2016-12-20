
using System;
using System.Configuration;
using ACS.MDB.Library;
using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Net.App.Services
{
    public class BaseService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseService()
        {
            Library.Common.DatabaseConnection.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["MDBConnectionString"].ConnectionString;

           //Library.DatabaseConnection.mdbDataContext = new MDBDataContext(ConfigurationManager.ConnectionStrings["MDBConnectionString"].ConnectionString);

           ////To avoid Error : Timeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding.
           //DatabaseConnection.mdbDataContext.CommandTimeout = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataContextCommandTimeout"]) ?
           //    Convert.ToInt32(ConfigurationManager.AppSettings["DataContextCommandTimeout"]) :
           //    DatabaseConnection.mdbDataContext.CommandTimeout;
        }
    }
}