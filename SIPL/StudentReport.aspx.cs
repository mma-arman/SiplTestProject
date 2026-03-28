using SIPL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace SIPL
{
    public partial class StudentReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                string RegistrationNo = Request.QueryString["RegistrationNo"];

                if (!string.IsNullOrEmpty(RegistrationNo))
                {
                    LoadReport(RegistrationNo);
                  

                }
            }
        }
        public void LoadReport(string RegistrationNo)
        {
            string[,] Param = new string[,]
            {
        {"@RegistrationNo", RegistrationNo }
            };

            DataTable dt = Common.ExecuteProcedure("USP_StudentReport", Param);

            if (dt != null && dt.Rows.Count > 0)
            {
                ReportViewer rv = new ReportViewer();

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = Server.MapPath("~/RDLC/StudentReport.rdlc");

                rv.LocalReport.DataSources.Clear();

                ReportDataSource rds1 = new ReportDataSource("DataSet1", dt);
                rv.LocalReport.DataSources.Add(rds1);

                rv.LocalReport.Refresh();

                string mimeType;
                string encoding;
                string fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] bytes = rv.LocalReport.Render(
                    "PDF",
                    null,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings
                );

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }
    }
}