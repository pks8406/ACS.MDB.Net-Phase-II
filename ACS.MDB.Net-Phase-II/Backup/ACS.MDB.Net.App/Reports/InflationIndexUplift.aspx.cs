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
    public partial class InflationIndexUplift : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string StartDate = Request.QueryString["StartDate"];
                    string EndDate = Request.QueryString["EndDate"];
                    string CompanyID = Request.QueryString["CompanyID"];
                    string UserID = Request.QueryString["UserID"];

                    string ReportName = ConfigurationManager.AppSettings["InflationIndexUpliftReport"];
                    if (ReportName != null && StartDate != null)
                    {
                        string ReportServerURL = ConfigurationManager.AppSettings["ServerUrl"];
                        reportViewer.ShowParameterPrompts = true;
                        reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServerURL);
                        reportViewer.ServerReport.ReportPath = string.Format("/{0}", ReportName);
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("StartDate", StartDate) });
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("EndDate", EndDate) });
                        reportViewer.ServerReport.SetParameters(new ReportParameter[] { new ReportParameter("CompanyID", CompanyID) });
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