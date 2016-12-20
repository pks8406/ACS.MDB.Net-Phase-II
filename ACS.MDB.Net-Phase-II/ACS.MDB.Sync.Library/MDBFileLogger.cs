using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.MDB.Sync.Library
{
    public class MDBFileLogger
    {
        private const string COMPANY_LOG_FILE_NAME = "OACompanyLog.txt";
        private const string CUSTOMER_LOG_FILE_NAME = "OACustomerLog.txt";
        private const string COSTCENTRE_LOG_FILE_NAME = "OACostCentreLog.txt";
        private const string ACCOUNTCODE_LOG_FILE_NAME = "OAAccountCodeLog.txt";
        private const string ACTIVITYCODE_LOG_FILE_NAME = "OAActivityCodeLog.txt";
        private const string JOBCODE_LOG_FILE_NAME = "OAJobCodeLog.txt";
        private const string OAPERIOD_LOG_FILE_NAME = "OAPeriodLog.txt";

        /// <summary>
        /// path of the log file
        /// </summary>
        private string logFilePath;

        /// <summary>
        /// Local constructor
        /// </summary>
        public MDBFileLogger(string lfp)
        {
            logFilePath = lfp;
        }

        /// <summary>
        /// Returns the current date and time
        /// with proper format
        /// </summary>
        /// <returns>Current date and time</returns>
        private string GetCurrentDateTime()
        {
            return String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now);
        }

        /// <summary>
        /// Gets fully qualified path of the log file
        /// </summary>
        private string GetLogPath(int entityID)
        {
            if (!String.IsNullOrWhiteSpace(logFilePath))
            {
                bool IsExists = Directory.Exists((logFilePath));

                if (!IsExists)
                    Directory.CreateDirectory(logFilePath);
            }
            else
            {
                logFilePath = "C:\\MDB\\MDBOASyncLog";
                Directory.CreateDirectory(logFilePath);
            }

            switch (entityID)
            {
                case 1:
                    logFilePath += "\\" + COMPANY_LOG_FILE_NAME;
                    break;
                case 2:
                    logFilePath += "\\" + CUSTOMER_LOG_FILE_NAME;
                    break;
                case 3:
                    logFilePath += "\\" + COSTCENTRE_LOG_FILE_NAME;
                    break;
                case 4:
                    logFilePath += "\\" + ACCOUNTCODE_LOG_FILE_NAME;
                    break;
                case 5:
                    logFilePath += "\\" + ACTIVITYCODE_LOG_FILE_NAME;
                    break;
                case 6:
                    logFilePath += "\\" + JOBCODE_LOG_FILE_NAME;
                    break;
                case 7:
                    logFilePath += "\\" + OAPERIOD_LOG_FILE_NAME;
                    break;
            }

            return logFilePath;
        }

        /// <summary>
        /// Writes error message to the log file
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public void Write(List<string> errorMessage, int entityID)
        {
            LogError(errorMessage, entityID);
        }

        /// <summary>
        /// Writes error message along with the
        /// configuration file name to the log file
        /// </summary>
        /// <param name="configFile">Name of the configuration file</param>
        /// <param name="errorMessage">The error message</param>
        public void Write(string configFile, string errorMessage)
        {
            if (!String.IsNullOrEmpty(configFile) && !String.IsNullOrEmpty(errorMessage))
            {
                string message = GetCurrentDateTime() + " : "
                                 + "Configuration file : \"" + configFile + "\" - " + errorMessage;
                //LogError(message);
            }

        }

        /// <summary>
        /// Logs the error message
        /// </summary>
        /// <param name="errorMessage">The error message to be logged</param>
        private void LogError(List<string> errorMessage, int entityID)
        {
            StreamWriter writer = null;
            try
            {
                string logPath = GetLogPath(entityID);

                writer = File.Exists(logPath) ? new StreamWriter(logPath, true) : new StreamWriter(logPath);
                writer.WriteLine("----------------------------" + GetCurrentDateTime() + "----------------------------------------");

                foreach (var item in errorMessage)
                {
                    if (!string.IsNullOrEmpty(item.Trim()))
                    {
                        string message = GetCurrentDateTime() + " : " + item; // +Environment.NewLine;
                        writer.WriteLine(message);
                    }
                    else
                    {
                        writer.WriteLine(Environment.NewLine);
                    }
                }
            }
            finally
            {
                //Cleanup buffers and
                //close the writer
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

    }
}
