using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Sync.Library;


namespace ACS.MDB.Net.App.Services
{
    public class OpenAccountService
    {
        private string oaConnectionString = string.Empty;
        private string mdbConnectionString = string.Empty;
        private string logFilePath = string.Empty;

        /// <summary>
        /// Default constructor 
        /// </summary>
        public OpenAccountService()
        {
            oaConnectionString = ApplicationConfiguration.GetOAConnectionString();
            mdbConnectionString = ApplicationConfiguration.GetMDBConnectionString();
            logFilePath = ApplicationConfiguration.GetLogFilePath();
        }

        /// <summary>
        /// Start synchronised data between OA & MDB
        /// </summary>
        public void Start(int? userId)
        {
            if (!string.IsNullOrEmpty(oaConnectionString) && !string.IsNullOrEmpty(mdbConnectionString) && !string.IsNullOrEmpty(logFilePath))
            {
                SynchOAData.Start(oaConnectionString, mdbConnectionString, logFilePath, userId);

                if (SynchOAData.IsError)
                {
                    throw new ApplicationException("Synchronisation completed with error. Please check log file");
                }
            }
            else
            {
                throw new ApplicationException("Parameter can not be null..");
            }
        }

    }
}