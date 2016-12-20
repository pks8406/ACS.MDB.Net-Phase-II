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
    public partial class MonthForecast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string companyID = Request.QueryString["CompanyID"];
                    string divisionID = Request.QueryString["DivisionID"];
                    string userID = Request.QueryString["UserID"];

                    string ReportName = ConfigurationManager.AppSettings["MonthForecastReport"];
                    if (ReportName != null && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(companyID))
                    {
                        string ReportServerURL = ConfigurationManager.AppSettings["ServerUrl"];
                        reportViewer.ShowParameterPrompts = true;
                        reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerURL);
                        reportViewer.ServerReport.ReportPath = string.Format("/{0}", ReportName);                        
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("CompanyID", companyID) });
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("DivisionID", divisionID) });
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("UserID", userID) });
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