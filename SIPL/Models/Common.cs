using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;

namespace SIPL.Models
{
    public static class Common
    {
        public static DataTable ExecuteProcedure(string Proc_name, string[,] param)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("DbConnection"));
            SqlCommand command = new SqlCommand(Proc_name, connection);
            command.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < param.Length / 2; i++)
            {
                command.Parameters.AddWithValue(param[i, 0], param[i, 1]);
            }
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public static DataTable ExecuteProcedure(string Proc_name)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("DbConnection"));
            SqlCommand command = new SqlCommand(Proc_name, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }



        public static string ShowTable(DataTable dt, string HideColumns = "", string EditFunctionName = "", string DeleteFunctionName = "",string type="")
        {
            StringBuilder sb = new StringBuilder();
            List<string> HideColumn = new List<string>();

            if (!string.IsNullOrEmpty(HideColumns))
            {
                string[] cols = HideColumns.Split(',');
                foreach (var col in cols)
                {
                    HideColumn.Add(col.Trim());
                }
            }

            sb.Append("<div class='table-responsive' style='max-height:300px; overflow:auto;'>");
            sb.Append("<table class='table table-bordered'><tr>");
            sb.Append("<th>Edit</th><th>Delete</th>");

            foreach (DataColumn col in dt.Columns)
            {
                if (HideColumn.Contains(col.ColumnName)) continue;
                sb.Append("<th>" + col.ColumnName + "</th>");
            }
            sb.Append("</tr>");

            string idColumn = dt.Columns[0].ColumnName;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<td><button class='bg-primary text-white' type='button' onclick='" + EditFunctionName + "(" + dt.Rows[i][idColumn] + ")'><i class='fa-solid fa-pen-to-square'></i></button></td>");
                sb.Append("<td><button class='bg-danger text-white' type='button' onclick='" + DeleteFunctionName + "(" + dt.Rows[i][idColumn] + ")'><i class='fa-solid fa-trash'></i></button></td>");

                foreach (DataColumn col in dt.Columns)
                {
                    if (HideColumn.Contains(col.ColumnName)) continue;
                    sb.Append("<td>" + dt.Rows[i][col.ColumnName] + "</td>");
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("</div>");

            return sb.ToString();
        }

        //public static string ExportToExcel(DataTable dt)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<table border=1>");
        //    sb.Append("<tr>");
        //    sb.Append("<th>Sr.no</th>");
        //    var Sno = 1;
        //    foreach (DataColumn col in dt.Columns)
        //    {
        //        sb.Append("<th>"+ col.ColumnName+"</th>");
        //    }
        //    sb.Append("</tr>");
        //    sb.Append("<tr>");
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        sb.Append("<td>" + Sno +"</td>");
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            sb.Append("<td>" + row[col] +"</td>");
        //        }
        //        sb.Append("</tr>");
        //        Sno++;
        //    }
           
        //    sb.Append("</table>");


        //    return sb.ToString();
        //}

        // For tasteing
       
            public static byte[] ExportToExcel(DataTable dt, string sheetName = "Sheet1")
            {
                ExcelPackage.License.SetNonCommercialOrganization("name");

                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add(sheetName);

                    // Header + Data automatic
                    ws.Cells["A1"].LoadFromDataTable(dt, true);

                    ws.Cells.AutoFitColumns();

                    return package.GetAsByteArray();
                }
            }
        


        public static DataTable GetCommonFormat(string type)
        {
            string[,] param = new string[,]
            {
                {"@type", type}
            };

            return ExecuteProcedure("USP_CommonFormat", param);
        }




       
        
    }
}
