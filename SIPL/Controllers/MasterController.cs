using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;
using SIPL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static SIPL.Models.Common;


namespace SIPL.Controllers
{
    public class MasterController : Controller
    {

        #region For Country Master
        public ActionResult CountryMaster()
        {
            return View();
        }

        public JsonResult InsertUpdateCountryMaster(string CountryID, string CountryCode, string CountryName, bool CountryActive)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";
            try
            {
                if (string.IsNullOrWhiteSpace(CountryCode))
                {
                    dic["Message"] = "Please Enter Country Code";
                    dic["Focus"] = "txtCountryCode";
                }
                else if (string.IsNullOrWhiteSpace(CountryName))
                {
                    dic["Message"] = "Please Enter  Country Name";
                    dic["Focus"] = "txtCountryName";
                }
                else
                {
                    string[,] param = new string[,]
                    {
                        {"@CountryID",CountryID },
                        {"@CountryCode",CountryCode },
                        {"@CountryName",CountryName },
                        {"@Active",CountryActive.ToString() },
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateCountry", param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        dic["Status"] = dt.Rows[0]["Status"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);

        }

       
        public ActionResult ShowCountryMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Grid"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowCountryMaster");
                string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                dic["Grid"] = Grid.ToString();
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic,JsonRequestBehavior.AllowGet);
        }
           
        
        public JsonResult EditCountryMaster(string CountryID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["CountryCode"] = "";
            dic["CountryName"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                    {"@CountryID",CountryID }
                };
                DataTable dt = Common.ExecuteProcedure("USP_ShowCountryMaster", Param);
                if (dt.Rows.Count > 0)
                {

                    dic["CountryCode"] = dt.Rows[0]["CountryCode"].ToString();
                    dic["CountryName"] = dt.Rows[0]["CountryName"].ToString();
                    dic["CountryActive"] = dt.Rows[0]["Active"].ToString();
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteCountryMaster(string CountryID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            try
            {
                string[,] param = new string[,]
                {
                    {"@CountryId",CountryID }
                };
                DataTable dt = Common.ExecuteProcedure("USP_DeleteCountryMaster", param);
                if (dt.Rows.Count > 0)
                {
                    dic["Message"] = dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);

        }

        public FileResult ExportToExcelCountryMaster()
        {
            string[,] Param = new string[,]
            {
                {"@type","Excel" }
            };

            DataTable dt = Common.ExecuteProcedure("USP_ShowCountryMaster",Param);
            byte[] filebytes=Common.ExportToExcel(dt);

             return File(
                filebytes,
                 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Export.xlsx"
            );
        }


       
        #endregion


        #region For State Master
        public ActionResult StateMaster()
        {
            return View();
        }

        
        public JsonResult InsertUpdateStateMaster(string StateID, string CountryCode, string StateCode, string StateName, bool Active)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";
            try
            {
                if (string.IsNullOrWhiteSpace(CountryCode))
                {
                    dic["Message"] = "Please Enter Country";
                    dic["Focus"] = "txtCountryCode";

                }
                else if (string.IsNullOrWhiteSpace(StateCode))
                {
                    dic["Message"] = "Please Enter State Code";
                    dic["Focus"] = "txtStateCode";

                }
                else if (string.IsNullOrWhiteSpace(StateName))
                {
                    dic["Message"] = "Please Enter StateName";
                    dic["Focus"] = "txtStateName";
                }
                else
                {
                    string[] CountryCodeParts = CountryCode.Split(':');
                    CountryCode = CountryCodeParts[0].Trim();

                    string[,] param = new string[,]
                    {
                        {"@StateID",StateID },
                        {"@CountryCode",CountryCode },
                        {"@StateCode",StateCode },
                        {"@StateName",StateName },
                        {"@Active",Active.ToString() },
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateState", param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        dic["Status"] = dt.Rows[0]["Status"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowStateMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Grid"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowStateMaster");
                string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                dic["Grid"] = Grid.ToString();
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditStateMaster(string StateID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messages"] = "";
            try {
                string[,] Param = new string[,]
               {
                    {"@StateID",StateID }
               };
                DataTable dt = Common.ExecuteProcedure("USP_ShowStateMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["CountryCode"] = dt.Rows[0]["Country"].ToString();
                    dic["StateCode"] = dt.Rows[0]["StateCode"].ToString();
                    dic["StateName"] = dt.Rows[0]["StateName"].ToString();
                    dic["Active"] = dt.Rows[0]["Active"].ToString();

                }

            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteStateMaster(string StateID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messasge"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                     {"@StateID",StateID}
                };
                DataTable dt = Common.ExecuteProcedure("USP_DeleteStateMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["Message"] = dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Messasge"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);


        }
        public FileResult ExportToExcelStateMaster()
        {
            string[,] Param = new string[,]
            {
                {"@type","Excel" }
            };

            DataTable dt = Common.ExecuteProcedure("USP_ShowStateMaster", Param);
            byte[] filebytes = Common.ExportToExcel(dt);

            return File(
              filebytes,
               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Export.xlsx"
           );
        }

        #endregion

        #region For City Master
        public ActionResult CityMaster()
        {
            return View();
        }

        public JsonResult InsertUpdateCityMaster(string CityID, string Country, string State, string City, string Active)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";

            try
            {
                if (string.IsNullOrWhiteSpace(Country))
                {
                    dic["Message"] = "Please Enter Country";
                    dic["Focus"] = "txtCountryCode";
                }
                else if (string.IsNullOrWhiteSpace(State))
                {
                    dic["Message"] = "Please Enter State";
                    dic["Focus"] = "txtState";
                }
                else if (string.IsNullOrWhiteSpace(City))
                {
                    dic["Message"] = "Please Enter City";
                    dic["Focus"] = "txtCityName";
                }
                else
                {
                    string[] CountryCodeParts = Country.Split(':');
                    Country = CountryCodeParts[0].Trim();
                    string[] StateCodeParts = State.Split(':');
                    State = StateCodeParts[0].Trim();


                    string[,] Param = new string[,]
                       {
                          { "@CityID",CityID},
                          { "@CountryCode",Country},
                          { "@StateCode",State},
                          { "@City",City},
                          { "@Active",Active},
                       };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateCity", Param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        dic["Status"] = dt.Rows[0]["Status"].ToString();
                        dic["Focus"] = dt.Rows[0]["Focus"].ToString();
                    }
                }
            }

            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteCityMaster(string CityID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messasge"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                     {"@CityID",CityID}
                };
                DataTable dt = Common.ExecuteProcedure("USP_DeleteCityMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["Message"] = dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Messasge"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowCityMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Grid"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowCityMaster");
                string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                dic["Grid"] = Grid.ToString();
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditCityMaster(string CityID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messages"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                    {"@CityID",CityID }
                };
                DataTable dt = Common.ExecuteProcedure("USP_ShowCityMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["CityID"] = dt.Rows[0]["CityID"].ToString();
                    dic["Country"] = dt.Rows[0]["Country"].ToString();
                    dic["State"] = dt.Rows[0]["State"].ToString();
                    dic["City"] = dt.Rows[0]["City"].ToString();
                    dic["Active"] = dt.Rows[0]["Active"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public FileResult ExportToExcelCityMaster()
        {
            string[,] Param = new string[,]
            {
                {"@type","Excel" }
            };

            DataTable dt = Common.ExecuteProcedure("USP_ShowCityMaster", Param);
            byte[] filebytes = Common.ExportToExcel(dt);

            return File(
               filebytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Export.xlsx"
           );
        }
        #endregion

        #region For PinCodeMaster

        public ActionResult PinCodeMaster()
        {
            return View();
        }

        public JsonResult InsertUpdatePinCodeMaster(string PinCodeID, string CountryCode, string StateCode, string City, string PinCode, bool Active)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";
            try
            {
                if (string.IsNullOrWhiteSpace(CountryCode))
                {
                    dic["Message"] = "Please Enter Country";
                    dic["Focus"] = "txtCountryCode";

                }
                else if (string.IsNullOrWhiteSpace(StateCode))
                {
                    dic["Message"] = "Please Enter State Code";
                    dic["Focus"] = "txtState";

                }
                else if (string.IsNullOrWhiteSpace(City))
                {
                    dic["Message"] = "Please Enter City";
                    dic["Focus"] = "txtCityName";
                }
                else if (string.IsNullOrWhiteSpace(PinCode))
                {
                    dic["Message"] = "Please Enter Pincode";
                    dic["Focus"] = "txtPinCode";
                }
                else if (PinCode.Length!=6)
                {
                    dic["Message"] = "PinCode Should be 6 Digit";
                    dic["Focus"] = "txtPinCode";
                }
                else
                {

                    string[] CountryCodeParts = CountryCode.Split(':');
                    CountryCode = CountryCodeParts[0].Trim();
                    string[] StateCodeParts = StateCode.Split(':');
                    StateCode = StateCodeParts[0].Trim();



                    string[,] param = new string[,]
                    {
                        {"@PinCodeID",PinCodeID },        
                        {"@PinCode",PinCode },
                        {"@CountryCode",CountryCode },
                        {"@StateCode",StateCode },
                        {"@City",City },
                        {"@Active",Active.ToString() },
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdatePinCodeMaster", param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        dic["Status"] = dt.Rows[0]["Status"].ToString();
                        dic["Focus"] = dt.Rows[0]["Focus"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPinCodeMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Grid"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowPinCodeMaster");
                string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                dic["Grid"] = Grid.ToString();
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditPinCodeMaster(string PinCodeID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messages"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                    {"@PinCodeID",PinCodeID }
                };
                DataTable dt = Common.ExecuteProcedure("USP_ShowPinCodeMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["PinCodeID"] = dt.Rows[0]["PinCodeID"].ToString();
                    dic["Country"] = dt.Rows[0]["Country"].ToString();
                    dic["State"] = dt.Rows[0]["State"].ToString();
                    dic["City"] = dt.Rows[0]["City"].ToString();
                    dic["PinCode"] = dt.Rows[0]["PinCode"].ToString();
                    dic["Active"] = dt.Rows[0]["Active"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePinCodeMaster(string PinCodeID)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messasge"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                     {"@PinCodeID",PinCodeID}
                };
                DataTable dt = Common.ExecuteProcedure("USP_DeletePinCodeMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["Message"] = dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Messasge"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportToExcelPinCodeMaster()
        {
            string[,] Param = new string[,]
            {
                {"@type","Excel" }
            };

            DataTable dt = Common.ExecuteProcedure("USP_ShowPinCodeMaster", Param);

            byte[] fileBytes = Common.ExportToExcel(dt, "PinCodeMaster");

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Export.xlsx"
            );
        }


        #endregion

        #region For Country Import
        public ActionResult CountryImport( )
        {
            string Format = "";
            Session.Remove("Error");
            DataTable dt = Common.GetCommonFormat("Country");
            if (dt.Columns.Count>0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Format += col.ColumnName + ",";
                }
            }
            Session["Format"] = Format.TrimEnd(',');
            return View();
        }

        [HttpPost]
        public ActionResult CountryImport(HttpPostedFileBase File)
        {           
            string Message = "";
            string ExecuteMsg = "";
            DataTable Error = new DataTable();
            Error.Columns.Add("Row");
            Error.Columns.Add("CountryCode");
            Error.Columns.Add("CountryName");
            Error.Columns.Add("Msg");


            try {
                if (File == null)
                {
                    Message = "Please Select File ";                    
                }
                else if (Path.GetExtension(File.FileName)!=".xlsx")
                {
                    Message = "file Should be .xlsx  ";
                }

                else if (File != null && File.ContentLength > 0)
                {
                    ExcelPackage.License.SetNonCommercialPersonal("Your Name Here");

                    using (var package = new ExcelPackage(File.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        //header Validation
                        string header1 = worksheet.Cells[1, 1].GetValue<string>()?.Trim();
                        string header2 = worksheet.Cells[1, 2].GetValue<string>()?.Trim();
                        string header3 = worksheet.Cells[1, 3].GetValue<string>()?.Trim();

                        if (header1 != "CountryCode" ||
                            header2 != "CountryName" ||
                            header3 != "Active")
                        {
                            Message = "Invalid file format. Please upload correct format file.";
                            TempData["Message"] = Message;
                            return View();
                        }

                        
                        int rowCount = worksheet.Dimension.End.Row;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ExecuteMsg = "";
                            string CountryCode = worksheet.Cells[row, 1].GetValue<string>()?.Trim();
                            string CountryName = worksheet.Cells[row, 2].GetValue<string>()?.Trim();
                            string CountryActive = worksheet.Cells[row, 3].GetValue<string>()?.Trim();
                            CountryActive= CountryActive?.Trim().ToLower();
                            //Counry Code Validation
                            if (string.IsNullOrEmpty(CountryCode))
                            {
                                ExecuteMsg = "Please enter Country Code";
                            }
                            //Country Name Validation
                            else if (string.IsNullOrEmpty(CountryName))
                            {
                                ExecuteMsg = "Please enter Country Name";
                            }
                            //Active Status Validation 
                            else if (CountryActive == "yes"|| CountryActive== "true"|| CountryActive=="1")
                            {
                                CountryActive = "true";
                            }
                            else if (CountryActive == "no" || CountryActive == "false" || CountryActive == "0")
                            {
                                CountryActive = "false";
                            }
                            else
                            {
                                ExecuteMsg = "Active must be Yes or No";
                            }

                            if (string.IsNullOrEmpty(ExecuteMsg))
                            {
                                string[,] param = new string[,]
                                {
                                    {"@CountryId","0"},
                                    {"@CountryCode", CountryCode},
                                    {"@CountryName", CountryName},
                                    {"@Active", CountryActive}
                                };

                                DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateCountry", param);

                                if (dt.Rows.Count > 0)
                                    ExecuteMsg = dt.Rows[0]["Msg"].ToString();
                            }
                            
                            Error.Rows.Add(row,CountryCode,CountryName,ExecuteMsg);
                        }
                    }
                    Session["Error"] = Error;
                }


            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
       
            TempData["Message"] = Message;
            return View();
        }
        public ActionResult DownloadImportError()
        {
            DataTable dt = Session["Error"] as DataTable;
            FileResult Error = ExportToExcel(dt);
            return Error;
        }

        public ActionResult DownloadCountryMasterTemplate()
        {

            DataTable dt = Common.GetCommonFormat("Country");
             FileResult Result= ExportToExcel(dt);

            return Result;
        }
        #endregion

        #region For UserMAster Import

        public ActionResult UserMasterImport()
        {
            Session["Format"] = null;
            string Format = "";
            DataTable dt = Common.GetCommonFormat("user");
            if (dt.Columns.Count > 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Format += col.ColumnName + ",";
                }
            }
            Session["Format"] = Format.TrimEnd(',');
            return View();
        }


        public ActionResult DownloadUserMasterTemplate()
        {
            DataTable dt = Common.GetCommonFormat("User");
            FileResult UserMasterTemplate = ExportToExcel(dt,"UserMasterTemplate.xlsx");

            return UserMasterTemplate;
        }

        [HttpPost]
        public ActionResult UserMasterImport(HttpPostedFileBase File)
        {   
            string Message = "";
            int Success = 0;
            int Failed= 0;
            int TotalRecord = 0;
            string ExceuteStatus = "";
            string ExceuteTimeMsg = "";
            DataTable Error = Common.GetCommonFormat("User");
            Error.Rows.Clear();
            Error.Columns.Add("Row");
            Error.Columns.Add("Msg");
            Error.Columns["Row"].SetOrdinal(0);
            try
            {
                if (File==null)
                {
                    Message = "PLease Select File";
                }
                else if (Path.GetExtension(File.FileName)!=".xlsx")
                {
                    Message = "Please Select only .xlsx File";
                }
                else if(File!= null && File.ContentLength>0)
                {
                    ExcelPackage.License.SetNonCommercialOrganization("User");
                    using (var package = new ExcelPackage(File.InputStream))
                    {
                        var ws=package.Workbook.Worksheets[0];
                        DataTable DataBaseHeaderFormat = Common.GetCommonFormat("User");
                        bool FormatIsValid = true;

                        for (int col=0; col< DataBaseHeaderFormat.Columns.Count;col++)
                        {
                            string DataBaseHeader = DataBaseHeaderFormat.Columns[col].ColumnName.Trim().ToLower();
                            string ImportHeader = ws.Cells[1, col + 1].GetValue<string>()?.Trim().ToLower();
                            if (DataBaseHeader!=ImportHeader)
                            {
                                FormatIsValid = false;
                                break;
                            }
                        }

                        if (FormatIsValid != true  )
                        {
                            Message = "Invalid file format. Please upload correct format file.";
                        }
                        else
                        {
                            int rowcount = ws.Dimension.End.Row;
                            int colcount = ws.Dimension.End.Column;                                               
                            for (int row = 2; row <= rowcount; row++)
                            {
                                ExceuteStatus = "";
                                ExceuteTimeMsg = "";
                                string UserCode = ws.Cells[row, 1].GetValue<string>()?.Trim();
                                string UserName = ws.Cells[row, 2].GetValue<string>()?.Trim();
                                string MobileNo = ws.Cells[row, 3].GetValue<string>()?.Trim();
                                string Password = ws.Cells[row, 4].GetValue<string>()?.Trim();
                                string EmailID = ws.Cells[row, 5].GetValue<string>()?.Trim();
                                string Address = ws.Cells[row, 6].GetValue<string>()?.Trim();
                                string Active = ws.Cells[row, 7].GetValue<string>()?.Trim().ToLower();
                                bool RowHasData = false;                                  
                                for (int col=1;col<=colcount;col++)
                                {
                                    string CellValue = ws.Cells[row, col].GetValue<string>()?.Trim();
                                    if (!string.IsNullOrWhiteSpace(CellValue))
                                    {
                                        RowHasData = true;
                                        break;
                                    }
                                }
                                if (!RowHasData)
                                    continue;
                                if (string.IsNullOrWhiteSpace(UserCode))
                                {
                                    ExceuteTimeMsg = "User Code Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(UserName))
                                {
                                    ExceuteTimeMsg = "User Name Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(MobileNo))
                                {
                                    ExceuteTimeMsg = "MobileNo Code Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(Password))
                                {
                                    ExceuteTimeMsg = "Password Code Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(EmailID))
                                {
                                    ExceuteTimeMsg = "EmailID Code Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(Address))
                                {
                                    ExceuteTimeMsg = "Address Code Can Not be null";
                                    Failed++;
                                }                                
                                else
                                {
                                        string[,] Param = new string[,]
                                    {                                        
                                        {"@UserCode",UserCode },
                                        {"@UserName",UserName },
                                        {"@MobileNo",MobileNo },
                                        {"@Password",Password },
                                        {"@EmailId",EmailID },
                                        {"@Address",Address },
                                        {"@Active",Active=(Active == "yes" || Active == "true" || Active == "1")?"true":"false"},
                                    };
                                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateUserMaster", Param);
                                    if (dt.Rows.Count > 0)
                                    {
                                        ExceuteTimeMsg = dt.Rows[0]["Msg"].ToString();
                                        ExceuteStatus = dt.Rows[0]["Status"].ToString();
                                        if (dt.Rows[0]["Status"].ToString()=="1")
                                        {
                                            Success++;
                                        }
                                        else
                                        {
                                            Failed++;
                                        }
                                        
                                    }
                                   
                                }
                                TotalRecord++;
                                if (!string.IsNullOrEmpty(ExceuteTimeMsg)&& ExceuteStatus!="1")
                                {
                                    Error.Rows.Add(row, UserCode, UserName, MobileNo, Password, EmailID, Address, Active, ExceuteTimeMsg);
                                }
                            }
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            TempData["Message"] = Message;
            Session["Error"] = Error;
            ViewBag.Success = Success;
            ViewBag.Failed= Failed;
            ViewBag.TotalRecord= TotalRecord;
            return View();
        }
        public ActionResult DownoadUserImportErrorResult()
        {
            FileResult Error = ExportToExcel(Session["Error"]as DataTable,"CountryImprtREsult.xlsx");
            return Error;
        }
        #endregion
        // Common Function For File Convert dt to Excel
        public FileResult ExportToExcel(DataTable dt,string fileName = "Export.xlsx", string sheetName = "Sheet1")
        {
            ExcelPackage.License.SetNonCommercialOrganization("name");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add(sheetName);               
                int colIndex = 1;
                foreach (DataColumn col in dt.Columns)
                {
                    ws.Cells[1, colIndex].Value = col.ColumnName;
                    colIndex++;
                }
                // Data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    colIndex = 1;
                    foreach (DataColumn col in dt.Columns)
                    {
                        ws.Cells[row + 2, colIndex].Value = dt.Rows[row][col];
                        colIndex++;
                    }
                }
                ws.Cells.AutoFitColumns();
                return File(
                    package.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
        }
    }
}