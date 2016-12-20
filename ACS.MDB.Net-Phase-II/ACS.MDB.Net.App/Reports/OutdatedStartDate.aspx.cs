using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace ACS.MDB.Net.App.Reports
{
    public partial class OutdatedStartDate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string ReportDate = Request.QueryString["ReportDate"];
                    string UserID = Request.QueryString["UserID"];
                    string ReportName = ConfigurationManager.AppSettings["OutdatedStartDateReport"];
                    if (ReportName != null && ReportDate != null)
                    {
                        string ReportServerURL = ConfigurationManager.AppSettings["ServerUrl"];
                        reportViewer.ShowParameterPrompts = true;
                        reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerURL);
                        reportViewer.ServerReport.ReportPath = string.Format("/{0}", ReportName);
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("ReportDate", ReportDate) });
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("UserID", UserID) });
                        reportViewer.ProcessingMode = ProcessingMode.Remote;
                        reportViewer.ShowZoomControl = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}