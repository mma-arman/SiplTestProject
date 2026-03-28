using Microsoft.Reporting.WebForms;
using SIPL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPL
{
    public partial class AdmissionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string AdmissionNo = Request.QueryString["AdmissionNo"];

                if (!string.IsNullOrEmpty(AdmissionNo))
                {
                    LoadReport(AdmissionNo);


                }
            }
        }


        public void LoadReport(string AdmissionNo)
        {
            string[,] Param = new string[,]
            {
                {"@AdmissionNo", AdmissionNo }
            };

            DataTable dt = Common.ExecuteProcedure("USP_AdmissionReport", Param);

            if (dt != null && dt.Rows.Count > 0)
            {
                LocalReport report = new LocalReport();

                report.ReportPath = Server.MapPath("~/RDLC/AdmissionReportRdlc.rdlc");

                ReportDataSource rds = new ReportDataSource("DataSet1", dt);

                report.DataSources.Clear();
                report.DataSources.Add(rds);

                string mimeType;
                string encoding;
                string fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] bytes = report.Render(
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