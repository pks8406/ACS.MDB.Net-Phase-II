using System;
using System.IO;
using System.Web.Mvc;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Controllers
{
    public class HelpController : BaseController
    {
        /// <summary>
        /// To download ARBS-Help.docx file
        /// </summary>
        public void DownloadHelpFile()
        {
            string helpFilePath = AppDomain.CurrentDomain.BaseDirectory + @"ARBSHelp\ARBS-Help.docx";
            string fileName = Path.GetFileName(helpFilePath);
            
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.TransmitFile((helpFilePath));
            Response.End();
        }
        /// <summary>
        /// To check whether ARBS-Help.docx exists
        /// </summary>
        public ActionResult IsHelpFileAvailable()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"ARBSHelp\ARBS-Help.docx";
            
            // Check file exits
            if (System.IO.File.Exists(filePath))
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeAndErrorResult(500, String.Format(Constants.HELP_FILE_NOT_FOUND));
        }

    }
}
