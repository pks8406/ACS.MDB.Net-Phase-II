using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Timers;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Sync.Library;


namespace ACS.MDB.OA.Windows.Service
{
    public class OAService
    {
        Timer timer = new Timer();

        Timer recalculationTimer = new Timer();

        string oaConnectionString = string.Empty;
        string mdbConnectionString = string.Empty;
        string logFilePath = string.Empty;
        string recalculationLogFilePath = string.Empty;
        int timeInterval = 3600000;
        private int recalculationTimeInterval = 100000;

        public OAService()
        {
            oaConnectionString = GetOAConnectionString();
            mdbConnectionString = GetMDBConnectionString();
            logFilePath = GetLogFilePath();
            recalculationLogFilePath = GetRecalculationLogFilePath();
            timeInterval = GetTimeInterval();
            recalculationTimeInterval = GetRecalculationTimePool();
        }


        /// <summary>
        /// Start windows service
        /// </summary>
        public void Start()
        {
            try
            {

                Library.Common.DatabaseConnection.DatabaseConnectionString = mdbConnectionString;

                Library.Common.DatabaseConnection.DataContextCommandTimeout =
                    Convert.ToInt32(ConfigurationManager.AppSettings["DataContextCommandTimeout"]);

                //handle Elapsed event
                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

                //This statement is used to set interval to 1 minute (= 60,000 milliseconds)
                timer.Interval = timeInterval;

                //for testing
                //timer.Interval = 60000;

                //enabling the timer
                timer.Enabled = true;

                timer.Start();
                //WindowsServiceScheduler(timer);
            }
            catch (Exception e)
            {
                WriteLog(e.Message, logFilePath);
            }

            //try catch for recalculaton process - log file is different 
            // thats why we have kept two try catch block
            try
            {
                recalculationTimer.Elapsed += new ElapsedEventHandler(OnElapsedTimeForRecalculation);

                recalculationTimer.Interval = recalculationTimeInterval;

                recalculationTimer.Enabled = true;

                recalculationTimer.Start();
                WindowsServiceSchedulerForRecalculation(recalculationTimer);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, recalculationLogFilePath);
            }
          
        }

        /// <summary>
        /// Stop window service
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Check timer on regular interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnElapsedTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                WindowsServiceScheduler(timer);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, logFilePath);
            }
        }

        /// <summary>
        /// Check timer on regular interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnElapsedTimeForRecalculation(object sender, ElapsedEventArgs e)
        {
            try
            {
                WindowsServiceSchedulerForRecalculation(timer);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, logFilePath);
            }
        }

        /// <summary>
        /// Check windows service scheduler 
        /// </summary>
        /// <param name="timer"></param>
        private void WindowsServiceScheduler(Timer timer)
        {

            string runweekly = Convert.ToString(ConfigurationManager.AppSettings["Weekly"]);
            string weeklyEventTriggerTime = Convert.ToString(ConfigurationManager.AppSettings["WeeklyeventTriggerTime"]);
            string dayOfWeek = Convert.ToString(ConfigurationManager.AppSettings["DayOfWeek"]);

            string DailyEventTriggerTime = Convert.ToString(ConfigurationManager.AppSettings["DailyEventTriggerTime"]);
            int hour;
            if (string.Equals(runweekly, "true", StringComparison.OrdinalIgnoreCase))
            {
                string weekDay = Convert.ToString(DateTime.Now.DayOfWeek);

                if (string.Equals(weekDay, dayOfWeek, StringComparison.OrdinalIgnoreCase))
                {
                    bool result = Int32.TryParse(weeklyEventTriggerTime, out hour);
                    if (result)
                    {
                        Run(oaConnectionString, mdbConnectionString, logFilePath, hour);
                    }
                }
            }
            else
            {
                bool result = Int32.TryParse(DailyEventTriggerTime, out hour);
                if (result)
                {
                    Run(oaConnectionString, mdbConnectionString, logFilePath, hour);
                }

            }
        }

        /// <summary>
        /// Check windows service scheduler for recalculation process
        /// </summary>
        /// <param name="timer"></param>
        private void WindowsServiceSchedulerForRecalculation(Timer timer)
        {
            Run(mdbConnectionString, logFilePath);
        }

        /// <summary>
        /// Run recalculation process
        /// </summary>
        /// <param name="mdbConnectionString"></param>
        /// <param name="logFilePath"></param>
        private void Run(string mdbConnectionString, string logFilePath)
        {

            Recalculation recalculation = new Recalculation();

            //recalculation.RecalculateMilestones();

            MDBDataContext arbsDataContext = new MDBDataContext(mdbConnectionString);

            List<Library.DataAccess.LINQ.Recalculation> recalculations = arbsDataContext.Recalculations.Where(r => !r.IsDeleted &&
                    r.Status == Convert.ToInt32(Library.Common.Constants.RecalculationStatus.PENDING)
                    || r.Status == Convert.ToInt32(Library.Common.Constants.RecalculationStatus.IN_PROGRESS)).ToList();

            if (recalculations.Count > 0)
            {
                bool isRecalculationInProgress = recalculations.Any(r => r.Status == Convert.ToInt32(Library.Common.Constants.RecalculationStatus.IN_PROGRESS));

                // IF recalculation not in running
                if (!isRecalculationInProgress)
                {
                    RecalculationVO recalculationVO = new RecalculationVO(recalculations.FirstOrDefault(r => r.Status == Convert.ToInt32(Library.Common.Constants.RecalculationStatus.PENDING)));
                    //if (recalculationVO != null)
                    //{
                    //    List<int> indexIds = new List<int>();

                    //    if (recalculationVO.IndexIds.Contains(";"))
                    //    {
                    //        string[] ids = recalculationVO.IndexIds.Split(';');
                    //        indexIds.AddRange(ids.Select(id => Convert.ToInt32(id)));
                    //    }
                    //    else
                    //    {
                    //        indexIds.Add(Convert.ToInt32(recalculationVO.IndexIds));
                    //    }
                    //}
                    recalculation.RecalculateMilestone(recalculationVO, 1);
                }
            }
        }

        /// <summary>
        /// Start synchronisation of OA & MDB data
        /// </summary>
        /// <param name="oaConnectionString"></param>
        /// <param name="mdbConnectionString"></param>
        /// <param name="logFilePath"></param>
        /// <param name="hour"></param>
        private static void Run(string oaConnectionString, string mdbConnectionString, string logFilePath, int hour)
        {
            try
            {
                if (hour < 24 && hour > 0)
                {
                    if (!string.IsNullOrEmpty(oaConnectionString) && !string.IsNullOrEmpty(mdbConnectionString))
                    {
                        if (DateTime.Now.Hour == hour)
                        {
                            SynchOAData.Start(oaConnectionString, mdbConnectionString, logFilePath, -1);
                        }
                    }
                }
                else
                {
                    WriteLog("Please enter valid Hour. Hour should be in 24 hour datetime format.", logFilePath);
                }
            }
            catch (Exception e)
            {
                WriteLog(e.Message, logFilePath);
            }
        }

        /// <summary>
        /// Write error to log file
        /// </summary>
        /// <param name="error"></param>
        /// <param name="logFilePath"></param>
        private static void WriteLog(string error, string logFilePath)
        {
            StreamWriter writer = null;
            try
            {
                if (string.IsNullOrEmpty(logFilePath))
                {
                    logFilePath = @"C:\ARBS\OASynchLog\";
                }

                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                logFilePath += "\\OASynchErrorLog.txt";

                writer = File.Exists(logFilePath) ? new StreamWriter(logFilePath, true) : new StreamWriter(logFilePath);
                string message = GetCurrentDateTime() + " : " + error; // +Environment.NewLine;

                writer.WriteLine(message);
                writer.WriteLine("-----------------------------------------------------------------------------------------");
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


        /// <summary>
        /// Returns the current date and time
        /// with proper format
        /// </summary>
        /// <returns>Current date and time</returns>
        private static string GetCurrentDateTime()
        {
            return String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now);
        }

        /// <summary>
        /// Read Open Account connection string
        /// </summary>
        /// <returns>return open account connection string</returns>
        private static string GetOAConnectionString()
        {
            string oaConnectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["OAConnectionString"]);

            return oaConnectionString;
        }

        /// <summary>
        /// Read MDB database connection string
        /// </summary>
        /// <returns></returns>
        private static string GetMDBConnectionString()
        {
            string mdbConnectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["MDBConnectionString"]);
            return mdbConnectionString;
        }

        /// <summary>
        /// Read log file path location
        /// </summary>
        /// <returns></returns>
        private static string GetLogFilePath()
        {
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            return logFilePath;

        }

        /// <summary>
        /// Real log file path for recalculation process
        /// </summary>
        /// <returns></returns>
        private static string GetRecalculationLogFilePath()
        {
            string logFilePath = ConfigurationManager.AppSettings["RecalculationLogFilePath"];
            return logFilePath;
        }

        /// <summary>
        /// Read pool time interval
        /// </summary>
        /// <returns></returns>
        private int GetTimeInterval()
        {
            int timeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimeInterval"]);

            return timeInterval;
        }

        /// <summary>
        /// Gets recalculatime timer pool
        /// </summary>
        /// <returns></returns>
        private int GetRecalculationTimePool()
        {
            int timeInterval = Convert.ToInt32(ConfigurationManager.AppSettings["RecalculationTimerPool"]);

            return timeInterval;
        }

    }
}
