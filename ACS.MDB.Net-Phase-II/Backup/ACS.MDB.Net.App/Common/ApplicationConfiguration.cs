using System;
using System.Configuration;

namespace ACS.MDB.Net.App.Common
{
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Get open account database connection for synch service
        /// </summary>
        /// <returns></returns>
        public static string GetOAConnectionString()
        {
            string oaConnectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["OAConnectionString"]);

            return oaConnectionString;
        }

        /// <summary>
        /// Get MDB database connection string
        /// </summary>
        /// <returns>return database connection string</returns>
        public static string GetMDBConnectionString()
        {
            string mdbConnectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["MDBConnectionString"]);
            return mdbConnectionString;
        }

        /// <summary>
        /// Get OA synch service log file location
        /// </summary>
        /// <returns></returns>
        public static string GetLogFilePath()
        {
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            return logFilePath;
        }

        /// <summary>
        /// Get recalculation log file location
        /// </summary>
        /// <returns></returns>
        public static string GetRecalculationLog()
        {
            string recalculationLog = ConfigurationManager.AppSettings["RecalculationLog"];
            return recalculationLog;
        }

        /// <summary>
        /// Get Open Account flat file generation location
        /// </summary>
        /// <returns></returns>
        public static string GetOAFlatFileLocation()
        {
            string oaFlatFilePath = ConfigurationManager.AppSettings["OAFlatFileLocation"];
            return oaFlatFilePath;
        }

        /// <summary>
        /// Get username of the machine 
        /// </summary>
        /// <returns></returns>
        public static string GetUNCUserName()
        {
            string UNCUserName = ConfigurationManager.AppSettings["Username"];
            return UNCUserName;
        }

        public static string GetUNCPassword()
        {
            string UNCPassword = ConfigurationManager.AppSettings["Password"];
            return UNCPassword;
        }

        public static string GetUNCDomainName()
        {
            string UNCDomainName = ConfigurationManager.AppSettings["DomainName"];
            return UNCDomainName;
        }

        /// <summary>
        /// While generating bill to OA file if only specific windows user has rights to write file 
        /// on shared location then use that users credential
        /// </summary>
        /// <returns></returns>
        public static bool UseUNCPath()
        {
            string useUNC = ConfigurationManager.AppSettings["UseUNCPath"];

            bool useUNCPath = false;

            if (!string.IsNullOrEmpty(useUNC))
            {
                if (useUNC.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    useUNCPath = true;
                }
            }

            return useUNCPath;
        }

        /// <summary>
        /// Get the latest version number
        /// </summary>
        /// <returns>Return ARBS version number</returns>
        public static string GetARBSVersionNumber()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["MDBVersion"]);
        }

        /// <summary>
        /// Get the Live Date
        /// </summary>
        /// <returns>Return ARBS live date</returns>
        public static DateTime GetARBSLiveDate()
        {
            DateTime ARBSLiveDate = Convert.ToDateTime(ConfigurationManager.AppSettings["LiveDate"]);
            return ARBSLiveDate;
        }

        public static string GetHelpFilePath()
        {
            string helpFilePath = @System.AppDomain.CurrentDomain.BaseDirectory + @"ARBSHelp\Advanced Recurring Billing System - User Guide.docx";
            //string helpFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            return helpFilePath;
        }

        public static string GetBillToOAErrorLogPath()
        {
            string errorLogPath = ConfigurationManager.AppSettings["BillToOAErrorLogPath"];
            return errorLogPath;
        }
    }
}