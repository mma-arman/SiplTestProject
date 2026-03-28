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


namespace SIPL.Models
{
    public static class Common
    {
        public static DataSet ExecuteProcedureDataSet(string Proc_name, string[,] param)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("DbConnection"));
            SqlCommand command = new SqlCommand(Proc_name, connection);
            command.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < param.Length / 2; i++)
            {
                command.Parameters.AddWithValue(param[i, 0], param[i, 1]);
            }
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

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



        public static string ShowTable(DataTable dt, string HideColumns = "", string EditFunctionName = "", string DeleteFunctionName = "",string type="",bool Report=false,string PrintReport="")
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
            sb.Append("<div class='table-responsive' style='max-width:100%; overflow-x:auto; overflow-y:auto; max-height:300px;'>");
            sb.Append("<table class='table table-bordered table-striped' style='white-space:nowrap; min-width:100%;'>");
            if (!string.IsNullOrEmpty(EditFunctionName))
            {
                sb.Append("<th>Edit</th>");
            }

            if (!string.IsNullOrEmpty(DeleteFunctionName))
            {
                sb.Append("<th>Delete</th>");
            }
            if (Report == true)
            {
                sb.Append("<th>Report</th>");
            }
            foreach (DataColumn col in dt.Columns)
            {
                if (HideColumn.Contains(col.ColumnName)) continue;
                sb.Append("<th>" + col.ColumnName + "</th>");
            }
            

            string idColumn = dt.Columns[0].ColumnName;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr>");
                if (!string.IsNullOrEmpty(EditFunctionName))
                {
                    sb.Append("<td><button class='btn btn-primary btn-sm' onclick='" + EditFunctionName + "(" + dt.Rows[i][idColumn] + ")'><i class='fa-solid fa-pen-to-square'></i></button></td>");
                }

                if (!string.IsNullOrEmpty(DeleteFunctionName))
                {
                    sb.Append("<td><button class='btn btn-danger btn-sm' onclick='" + DeleteFunctionName + "(" + dt.Rows[i][idColumn] + ")'><i class='fa-solid fa-trash'></i></button></td>");
                }
                if (Report==true)
                {
                    sb.Append("<td><button class='btn btn-danger btn-sm' onclick='" + PrintReport + "(\""  + dt.Rows[i][idColumn] + "\")'><i class='fa fa-print '></i></button></td>");
                }
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

       
        public static byte[] ExportToExcel(DataTable dt, string sheetName = "Sheet1", bool Sno = false)
            {
                ExcelPackage.License.SetNonCommercialOrganization("name");
            if (Sno is true)
            {
                dt.Columns.Add("S.No", typeof(int)).SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["S.No"] = i + 1;
                }
            }           

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

        public static string ConvertToxml<T>(List<T> list,String RootName="RootName",string ChildName = "ChildName")
        {
            StringBuilder Xml=new StringBuilder();
            Xml.Append($"<{RootName}>");
            foreach (var l in list)
            {
                Xml.Append($"<{ChildName}>");
                foreach (var prop in typeof(T).GetProperties())
                {
                    var value = prop.GetValue(l, null);
                    Xml.Append($"<{prop.Name}>{value}</{prop.Name}>");
                }
                Xml.Append($"</{ChildName}>");
            }
            Xml.Append($"</{RootName}>");
            return Xml.ToString();
        }


       
        
    }
}
