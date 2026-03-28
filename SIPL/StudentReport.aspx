<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentReport.aspx.cs" Inherits="SIPL.StudentReport"Debug="true" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"Namespace="Microsoft.Reporting.WebForms"TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
             <rsweb:ReportViewer ID="ReportViewer1"
                runat="server"
                Width="100%"
                Height="600px"
                ProcessingMode="Local">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
