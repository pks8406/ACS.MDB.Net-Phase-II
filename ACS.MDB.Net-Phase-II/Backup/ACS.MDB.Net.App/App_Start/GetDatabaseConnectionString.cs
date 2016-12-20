using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ACS.MDB.Net.App.App_Start
{
    public class GetDatabaseConnectionString
    {
        public static void GetARBSConnectionString()
        {
            Library.Common.DatabaseConnection.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["MDBConnectionString"].ConnectionString;

            Library.Common.DatabaseConnection.DataContextCommandTimeout =
                Convert.ToInt32(ConfigurationManager.AppSettings["DataContextCommandTimeout"]);

        }
    }
}