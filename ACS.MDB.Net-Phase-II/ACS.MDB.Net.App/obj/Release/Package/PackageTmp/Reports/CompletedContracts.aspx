<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompletedContracts.aspx.cs" Inherits="ACS.MDB.Net.App.Reports.CompletedContracts" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="100000" />
        <rsweb:ReportViewer ID="reportViewer" runat="server" Style="position: absolute;" AsyncRendering="true" Width="99%" Height="410px" ProcessingMode="Remote">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
