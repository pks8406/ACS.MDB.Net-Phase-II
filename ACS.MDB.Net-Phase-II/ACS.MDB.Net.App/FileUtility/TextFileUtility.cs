using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ACS.MDB.Net.App.FileUtility
{
    public class TextFileUtility
    {
        public TextFileUtility()
        {
                
        }

        /// <summary>
        /// Logs the error message
        /// </summary>
        /// <param name="errorMessage">The error message to be logged</param>
        public void WriteLog(string path, string filename, List<string> errorMessage)
        {
            StreamWriter writer = null;
            try
            {

                if (!String.IsNullOrWhiteSpace(path))
                {
                    bool IsExists = Directory.Exists((path));

                    if (!IsExists)
                        Directory.CreateDirectory(path);
                }

                writer = new StreamWriter(path + "//" + filename);
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

        /// <summary>
        /// Returns the current date and time
        /// with proper format
        /// </summary>
        /// <returns>Current date and time</returns>
        private string GetCurrentDateTime()
        {
            return String.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now);
        }
    }
}