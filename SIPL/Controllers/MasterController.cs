using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Converters;
using OfficeOpenXml;
using SIPL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebGrease.Activities;
using static SIPL.Models.Common;


namespace SIPL.Controllers
{
    public class MasterController : Controller
    {
        public ActionResult WelcomePage()
        {
            var UserInfo = Session["UserInfo"] as Dictionary<string, string>;
            if (UserInfo!=null)
            {
                ViewBag.UserCode = UserInfo["UserCode"];
                ViewBag.UserName = UserInfo["UserName"];
                return View();
            }
            else {
                return RedirectToAction("Login","Master");
            }
           
        }

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
                "CountryExport.xlsx"
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
                "StateExport.xlsx"
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
                "CityExport.xlsx"
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
                if (dt.Rows.Count>0)
                {
                    string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                    dic["Grid"] = Grid.ToString();
                }
             

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
            dic["Message"] = "";
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
                dic["Message"] = ex.Message;
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
                "PinodeMaster.xlsx"
            );
        }


        #endregion

        #region PinCodeMasterImport
        public ActionResult PinCodeMasterImport()
        {
            Session.Remove("Error");
            string Format = "";
            DataTable dt = Common.GetCommonFormat("PinCodeImport");
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

        [HttpPost]
        public ActionResult PinCodeMasterImport(HttpPostedFileBase File)
        {
            string Message = "";
            int Success = 0;
            int Failed = 0;
            int TotalRecord = 0;
            string ExceuteStatus = "";
            string ExceuteTimeMsg = "";
            DataTable Error = Common.GetCommonFormat("PinCodeImport");
            Error.Rows.Clear();
            Error.Columns.Add("RowNo");
            Error.Columns.Add("ErrorMsg");
            Error.Columns["RowNo"].SetOrdinal(0);
            try
            {
                if (File == null)
                {
                    Message = "PLease Select File";
                }
                else if (Path.GetExtension(File.FileName) != ".xlsx")
                {
                    Message = "Please Select only .xlsx File";
                }
                else if (File != null && File.ContentLength > 0)
                {
                    ExcelPackage.License.SetNonCommercialOrganization("User");
                    using (var package = new ExcelPackage(File.InputStream))
                    {
                        var ws = package.Workbook.Worksheets[0];
                        DataTable DataBaseHeaderFormat = Common.GetCommonFormat("PinCodeImport");
                        bool FormatIsValid = true;
                        for (int col = 0; col < DataBaseHeaderFormat.Columns.Count; col++)
                        {
                            string DataBaseHeader = DataBaseHeaderFormat.Columns[col].ColumnName.Trim().ToLower();
                            string ImportHeader = ws.Cells[1, col + 1].GetValue<string>()?.Trim().ToLower();
                            if (DataBaseHeader != ImportHeader)
                            {
                                FormatIsValid = false;
                            }
                        }

                        if (FormatIsValid != true)
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
                                string Country = ws.Cells[row, 1].GetValue<string>()?.Trim();
                                string State = ws.Cells[row, 2].GetValue<string>()?.Trim();
                                string City = ws.Cells[row, 3].GetValue<string>()?.Trim();
                                string PinCode = ws.Cells[row, 4].GetValue<string>()?.Trim();
                                string Active = ws.Cells[row, 5].GetValue<string>()?.Trim().ToLower();
                                bool RowHasData = false;
                                for (int col = 1; col <= colcount; col++)
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
                                if (string.IsNullOrWhiteSpace(PinCode))
                                {
                                    ExceuteTimeMsg = "PinCode Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(Country))
                                {
                                    ExceuteTimeMsg = "Country Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(State))
                                {
                                    ExceuteTimeMsg = "State  Can Not be null";
                                    Failed++;
                                }
                                else if (string.IsNullOrWhiteSpace(City))
                                {
                                    ExceuteTimeMsg = "City Code Can Not be null";
                                    Failed++;
                                }

                                else
                                {
                                    string[,] Param = new string[,]
                                {
                                        {"@CountryCode",Country },
                                        {"@StateCode",State },
                                        {"@City",City },
                                        {"@PinCode",PinCode },
                                        {"@Active",Active=(Active == "yes" || Active == "true" || Active == "1")?"true":"false"},
                                };
                                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdatePinCodeMaster", Param);
                                    if (dt.Rows.Count > 0)
                                    {
                                        ExceuteTimeMsg = dt.Rows[0]["Msg"].ToString();
                                        ExceuteStatus = dt.Rows[0]["Status"].ToString();
                                        if (dt.Rows[0]["Status"].ToString() == "1")
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
                                if (!string.IsNullOrEmpty(ExceuteTimeMsg) && ExceuteStatus != "1")
                                {
                                    Error.Rows.Add(row, Country, State, City, PinCode, Active, ExceuteTimeMsg);
                                }
                               
                            }
                            if (Success == 0 && Failed == 0 )
                            {
                                Message = "File contains no data.";
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
            ViewBag.Failed = Failed;
            ViewBag.TotalRecord = TotalRecord;
            return View();
        }
        public ActionResult DownloadPinCodeMasterTemplate()
        {
            DataTable dt = Common.GetCommonFormat("PinCodeImport");
            byte[] PinCodeMAsterTemplate = Common.ExportToExcel(dt, "PinCodeMasterTemplate.xlsx");

            return File(
               PinCodeMAsterTemplate,
               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               "PinCodeMasterTemplate.xlsx"
           );
        }
        public ActionResult DownoadPinCodeImportErrorResult()
        {
            byte[] Error = Common.ExportToExcel(Session["Error"] as DataTable, "PinCodeImport.xlsx");

            return File(
                Error,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PinCodeImportError.xlsx"
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
            int Total = 0;
            int Success = 0;
            int Failed=0;
            string ExecuteStatus = "";
            DataTable Error= Common.GetCommonFormat("Country");
            Error.Rows.Clear();
            Error.Columns.Add("RowNo");
            Error.Columns.Add("ErrorMsg");
            Error.Columns["RowNo"].SetOrdinal(0);
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

                            int colCountt = worksheet.Dimension.End.Column;
                            bool isRowBlank = true;

                            for (int col = 1; col <= colCountt; col++)
                            {
                                string cellValue = worksheet.Cells[row, col].Text?.Trim();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    isRowBlank = false;
                                    break;
                                }
                            }

                            if (isRowBlank)
                            {
                                continue; // Blank row ko skip karo
                            }

                            ExecuteStatus = "";
                            ExecuteMsg = "";
                            string CountryCode = worksheet.Cells[row, 1].GetValue<string>()?.Trim();
                            string CountryName = worksheet.Cells[row, 2].GetValue<string>()?.Trim();
                            string CountryActive = worksheet.Cells[row, 3].GetValue<string>()?.Trim()?.ToLower();
                           
                            //Counry Code Validation
                            if (string.IsNullOrEmpty(CountryCode))
                            {
                                ExecuteMsg = "Please enter Country Code";
                                Failed++;
                            }
                            //Country Name Validation
                            else if (string.IsNullOrEmpty(CountryName))
                            {
                                ExecuteMsg = "Please enter Country Name";
                                Failed++;
                            }
                           
                            if (string.IsNullOrEmpty(ExecuteMsg))
                            {
                                string[,] param = new string[,]
                                {
                                    {"@CountryId","0"},
                                    {"@CountryCode", CountryCode},
                                    {"@CountryName", CountryName},
                                    {"@Active", CountryActive=(CountryActive == "yes"||CountryActive=="true"||CountryActive=="1")?"True":"False"}
                                };

                                DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateCountry", param);

                                if (dt.Rows.Count > 0)
                                {
                                    ExecuteMsg = dt.Rows[0]["Msg"].ToString();
                                    ExecuteStatus = dt.Rows[0]["Status"].ToString();
                                }
                                if (dt.Rows[0]["Status"].ToString()=="1")
                                {
                                    Success++;
                                }
                                else
                                {
                                    Failed++;
                                }
                            }
                            if (ExecuteStatus!="1")
                            {
                                Error.Rows.Add(row, CountryCode, CountryName,CountryActive, ExecuteMsg);
                            }
                            Total++;
                        }
                        if (Success==0 && Failed==0)
                        {
                            Message = "Please Enter data";
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
            ViewBag.Total = Total;
            ViewBag.Success = Success;
            ViewBag.Failed = Failed++;
            return View();
        }
        public ActionResult DownloadImportError()
        {
            DataTable dt = Session["Error"] as DataTable;
           byte[] Error = ExportToExcel(dt,"CountryImportResult.xlsx");
            return File(
                Error,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CountryImportError.xlsx"
            );
        }

        public ActionResult DownloadCountryMasterTemplate()
        {

            DataTable dt = Common.GetCommonFormat("Country");
            byte[] Error = Common.ExportToExcel(dt,"CountryTemplate.xlsx");

         
            return File(
                Error,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CountryMasterTemplate.xlsx"
            );
        }
        #endregion

        #region For UserMAster Import        
        public ActionResult UserMasterImport()
        {
            Session.Remove("Error");
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
            Error.Columns.Add("RowNo");
            Error.Columns.Add("ErrorMsg");
            Error.Columns["RowNo"].SetOrdinal(0);
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
        public ActionResult DownloadUsMasterTemplate()
        {
            DataTable dt = Common.GetCommonFormat("User");
           byte[] UserMasterTemplate =Common.ExportToExcel(dt, "UserMasterTemplate.xlsx");

            return File(
               UserMasterTemplate,
               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               "UserMasterTemplate.xlsx"
           );
        }
        public ActionResult DownoadUserImportErrorResult()
        {
            byte[] Error =Common.ExportToExcel(Session["Error"]as DataTable,"CountryImprtREsult.xlsx");
       
            return File(
                Error,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "DownloadUserImportError.xlsx"
            );
        }
        #endregion        

        #region For Student Master
        public ActionResult StudentMaster()
        {
            Session["CourseList"] = null;
            return View();
        }
        public ActionResult InsertUpdateStudentMaster(string RegistrationNo, string StudentName,string FatherName,string DateOfBirth,string MobileNo, string EmailId,
            string Password, string Gender,string StudentPhoto,string FileName, string FileType, string City,string Address, string StudentId = "0")
            {
          
            List<Course> Courses = Session["CourseList"] as List<Course>;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            var Status = "";
            try
            {
                int sizeInBytes = (StudentPhoto.Length * 3) / 4;
               
                DateTime today = DateTime.Today;
                DateTime dob = Convert.ToDateTime(DateOfBirth);
                if (string.IsNullOrWhiteSpace(StudentName))
                {
                    dic["Message"] = "Please Enter Student Name";
                    dic["Focus"] = "StudentName";
                }
                else if (string.IsNullOrWhiteSpace(FatherName))
                {
                    dic["Message"] = "Please Enter Father Name";
                    dic["Focus"] = "FatherName";
                }
                else  if (string.IsNullOrWhiteSpace(DateOfBirth))
                {
                    dic["Message"] = "Please Enter DateOfBirth";
                    dic["Focus"] = "DateOfBirth";
                }
                else if (dob>today)
                {
                    dic["Message"] = "Date of Birth cannot be in the future";
                    dic["Focus"] = "DateOfBirth";
                }
                else if (string.IsNullOrWhiteSpace(MobileNo))
                {
                    dic["Message"] = "Please Enter MobileNo";
                    dic["Focus"] = "MobileNo";
                }
                else if (MobileNo.Length != 10)
                {
                    dic["Message"] = "Please Enter Valid MobileNo";
                    dic["Focus"] = "MobileNo";
                }
                else if (string.IsNullOrWhiteSpace(EmailId))
                {
                    dic["Message"] = "Please Enter EmailId";
                    dic["Focus"] = "EmailId";
                }
                else if (!Regex.IsMatch(EmailId, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    dic["Message"] = "Please Enter Valid EmailId";
                    dic["Focus"] = "EmailId";
                }
                else if (string.IsNullOrWhiteSpace(Password))
                {
                    dic["Message"] = "Please Enter Password";
                    dic["Focus"] = "Password";
                }
                else if (!Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$"))
                {
                    dic["Message"] = "Password must be 8 characters with uppercase, lowercase, number and special character";
                    dic["Focus"] = "Password";
                }
                else if (string.IsNullOrWhiteSpace(Gender))
                {
                    dic["Message"] = "Please Choose Gender";
                    dic["Focus"] = "Gender";
                }
                else if (string.IsNullOrWhiteSpace(RegistrationNo) && string.IsNullOrWhiteSpace(StudentPhoto))
                {
                    dic["Message"] = "Please Select Photo";
                    dic["Focus"] = "StudentPhoto";
                }
                else if (!string.IsNullOrWhiteSpace(StudentPhoto) &&
                        FileType.ToLower() != "jpg" &&
                        FileType.ToLower() != "jpeg" &&
                        FileType.ToLower() != "png" &&
                        FileType.ToLower() != "webp")
                {
                    dic["Message"] = "Please select a valid image file (jpg, jpeg, png, webp)";
                    dic["Focus"] = "StudentPhoto";
                }

                else if (sizeInBytes > 300000)
                {
                    dic["Message"] = "Image size should be less than 300 KB";
                    dic["Focus"] = "StudentPhoto";
                }

                else if (string.IsNullOrWhiteSpace(City))
                {
                    dic["Message"] = "Please Enter City";
                    dic["Focus"] = "City";
                }
                else if (string.IsNullOrWhiteSpace(Address))
                {
                    dic["Message"] = "Please Enter Address";
                    dic["Focus"] = "Address";
                }
                else if (Courses == null || Courses.Count == 0)
                {
                    dic["Message"] = "Please add at least one qualification before saving.";
                    dic["Focus"] = "Course";
                }
                else
                {

                    string CoursesXml = Common.ConvertToxml(Courses, "Courses", "Course");

                    string[,] Param = new string[,]
                    {

                        {"@RegistrationNo",RegistrationNo},
                        {"@StudentName",StudentName},
                        {"@FatherName",FatherName},
                        {"@MobileNo",MobileNo},
                        {"@DateOfBirth",DateOfBirth},
                        {"@EmailID",EmailId},
                        {"@Password",Password},
                        {"@Gender",Gender},
                        {"@StudentPhoto",StudentPhoto},
                        {"@City",City},
                        {"@Address",Address},
                        {"@FileName",FileName},
                        {"@FileType",FileType},
                        {"@CoursesXml",CoursesXml},
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateStudentMaster", Param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        Status = dt.Rows[0]["Status"].ToString();
                        dic["Status"] = Status;
                        if (Status == "1")
                        {
                            Session["CourseList"] = null;
                        }
                    }

                }
             
                
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            return Json(dic,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCourse(int TempId, string CourseName, string TotalMarks, string ObtainedMarks, string Year)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";
            bool IsCourseAvaliable = false;
            List<Course> Courses = Session["CourseList"] as List<Course> ?? new List<Course>();
            try
            {
                for (int i=0;i<Courses.Count;i++)
                {
                    if (CourseName.ToLower()== Courses[i].CourseName.ToLower() && Courses[i].TempId != TempId)
                    {
                        IsCourseAvaliable = true;
                    }
                }

                if (string.IsNullOrWhiteSpace(CourseName))
                {
                    dic["Focus"] = "Course";
                    dic["Message"] = "Please enter Course";
                }
                else if (string.IsNullOrWhiteSpace(TotalMarks))
                {
                    dic["Focus"] = "TotalMarks";
                    dic["Message"] = "Please enter Total Marks";
                }
                else if (string.IsNullOrWhiteSpace(ObtainedMarks))
                {
                    dic["Focus"] = "ObtainedMarks";
                    dic["Message"] = "Please enter Obtained Marks";
                }
                else if (Convert.ToInt64(ObtainedMarks)>Convert.ToInt64(TotalMarks))
                {
                    dic["Focus"] = "ObtainedMarks";
                    dic["Message"] = "Obtained Marks should be less than Total Marks.";
                }
                else if (string.IsNullOrWhiteSpace(Year))
                {
                    dic["Focus"] = "Year";
                    dic["Message"] = "Please enter Year";
                }
                else if (IsCourseAvaliable )
                {
                    dic["Focus"] = "Course";
                    dic["Message"] = "Course Already Exist";
                }
                else
                {
                    
                    bool isUpdated = false;
                    for (int i = 0; i < Courses.Count; i++)
                    {
                        if (Courses[i].TempId == TempId)
                        {
                            Courses[i].CourseName = CourseName;
                            Courses[i].TotalMarks = TotalMarks;
                            Courses[i].ObtainedMarks = ObtainedMarks;
                            Courses[i].Year = Year;
                            isUpdated = true;
                            dic["Message"] = "Course Updated Successfully";
                            dic["Focus"] = "Course";
                            dic["Status"] = "1";

                        }
                    }
                    if (!isUpdated)
                    {
                        Courses.Add(new Course
                        {
                            TempId = TempId,
                            CourseName = CourseName,
                            TotalMarks = TotalMarks,
                            ObtainedMarks = ObtainedMarks,
                            Year = Year
                        });

                        dic["Message"] = "Course Added Successfully";
                        dic["Focus"] = "Course";
                        dic["Status"] = "1";
                    }

                    Session["CourseList"] = Courses;
                    dic["Courses"] = Courses;
                }
                
                        
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowCourse()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Messages"] = "";
            dic["Grid"] = "";
            StringBuilder sb=new StringBuilder();
            try
            {
                List<Course> Courses = Session["CourseList"] as List<Course> ?? new List<Course>();
                if (Courses.Count == 0)
                {
                    dic["Grid"] = "";
                }
                else
                {
                    sb.Append("<table class='table MT-4 table-sm'>");
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Course</th><th>Total Marks</th><th>Obtained Marks</th><th>Year</th><th>Edit</th><th>Delete</th>");
                    sb.Append("</tr></thead><tbody>");
                    foreach (var c in Courses)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{c.CourseName}</td>");
                        sb.Append($"<td>{c.TotalMarks}</td>");
                        sb.Append($"<td>{c.ObtainedMarks}</td>");
                        sb.Append($"<td>{c.Year}</td>");
                        sb.Append($"<td><button type='button' onclick='EditCourse({c.TempId})'>Edit</button></td>");
                        sb.Append($"<td><button type='button' class='deleteCourse' onclick='DeleteCourse({c.TempId})'>Delete</button></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    dic["Grid"] = sb.ToString();
                }
               
            }
            catch (Exception ex)
            {
                dic["Messages"]=ex.Message;
            }
            return Json(dic,JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCourse(int TempId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            try
            {
                List<Course> courses = Session["CourseList"] as List<Course> ?? new List<Course>();
                for (int i=0;i<courses.Count;i++)
                {
                    if (courses[i].TempId == TempId)
                    {
                        dic["SelectedCourse"] = courses[i];
                    }

                }

            }
            catch(Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCourse(int TempId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messages"] = "";
            try 
            {
                List<Course> Courses = Session["CourseList"] as List<Course>;
                if (Courses!=null)
                {
                    for (int i = 0; i < Courses.Count; i++)
                    {
                        if (Courses[i].TempId == TempId)
                        {
                            Courses.RemoveAt(i);
                            dic["Messages"] = "Delete Succesfull";
                            break;
                        }
                    }
                    Session["CourseList"] = Courses;
                }
                else
                {
                    dic["Messages"] = "No Data Found";
                }


            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditStudentMaster(string RegistrationNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            Session["CourseList"] = null;
            try
            {
                string[,] Param = new string[,]
                {
                    {"@RegistrationNo",RegistrationNo}
                };
                DataSet ds = Common.ExecuteProcedureDataSet("USP_ShowStudentMaster", Param);
                if (ds.Tables.Count>0)
                {
                    int tableCount = ds.Tables.Count;
                    DataTable StudentRecord = ds.Tables[0];
                    dic["StudentName"] = StudentRecord.Rows[0]["StudentName"].ToString();
                    dic["FatherName"] = StudentRecord.Rows[0]["FatherName"].ToString();
                    dic["DateOfBirth"] = Convert.ToDateTime(StudentRecord.Rows[0]["DateOfBirth"]).ToString("yyyy-MM-dd");
                    dic["MobileNo"] = StudentRecord.Rows[0]["MobileNo"].ToString();
                    dic["EmailId"] = StudentRecord.Rows[0]["EmailId"].ToString();
                    dic["Password"] = StudentRecord.Rows[0]["Password"].ToString();
                    dic["Gender"] = StudentRecord.Rows[0]["Gender"].ToString();
                    dic["StudentPhoto"] = StudentRecord.Rows[0]["StudentPhoto"].ToString();
                    dic["City"] = StudentRecord.Rows[0]["City"].ToString();
                    dic["Address"] = StudentRecord.Rows[0]["Address"].ToString();
                    dic["FileName"] = StudentRecord.Rows[0]["FileName"].ToString();
                    dic["FileType"] = StudentRecord.Rows[0]["FileType"].ToString();
                    DataTable QualificationRecords = ds.Tables[1];
                    if (QualificationRecords.Rows.Count > 0 && QualificationRecords.Rows != null)
                    {

                        List<Course> list = Session["CourseList"] as List<Course> ?? new List<Course>();
                        foreach (DataRow dr in QualificationRecords.Rows)
                        {
                            Course course = new Course();
                            course.CourseName = dr["Course"].ToString();
                            course.TotalMarks = dr["TotalMarks"].ToString();
                            course.ObtainedMarks = dr["ObtainedMarks"].ToString();
                            course.Year = dr["Year"].ToString();
                            course.TempId = Convert.ToInt32(dr["QualificationId"]);
                            list.Add(course);
                        }
                        Session["CourseList"] = list;
                    }
                }
               

               }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic,JsonRequestBehavior.AllowGet);
        
        }
        #endregion

        #region For AdmissionMaster
        public ActionResult AdmissionMaster()
        {
            Session["AcademicQualification"] = null;
            return View();
        }
        public ActionResult InsertUpdateAdmissionForm(string AdmissionNo,string RegistrationNo, string StudentName, string ParentsName,string ParentsProfession, string StudentDob, string MobileNo, string EmailId,
             string Gender, string StudentPhoto, string FileName, string FileType, string City, string Address)
        {
            List<Course> Courses = Session["AcademicQualification"] as List<Course>;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            var Status = "";
            try
            {
                int sizeInBytes = (StudentPhoto.Length * 3) / 4;
                if (string.IsNullOrWhiteSpace(RegistrationNo))
                {
                    dic["Message"] = "Please Enter Registration No";
                    dic["Focus"] = "RegistrationNo";
                }
                else if (string.IsNullOrWhiteSpace(StudentName))
                {
                    dic["Message"] = "Please Enter Student Name";
                    dic["Focus"] = "StudentName";
                }
                else if (string.IsNullOrWhiteSpace(ParentsName))
                {
                    dic["Message"] = "Please Enter Parents Name";
                    dic["Focus"] = "ParentsName";
                }
                else if (string.IsNullOrWhiteSpace(StudentDob))
                {
                    dic["Message"] = "Please Enter DateOfBirth";
                    dic["Focus"] = "StudentDob";
                }
                else if (Convert.ToDateTime(StudentDob) >DateTime.Today)
                {
                    dic["Message"] = "Please Enter Valid DateOfBirth";
                    dic["Focus"] = "StudentDob";
                }
                else if (string.IsNullOrWhiteSpace(MobileNo))
                {
                    dic["Message"] = "Please Enter MobileNo";
                    dic["Focus"] = "MobileNo";
                }
                else if (MobileNo.Length != 10)
                {
                    dic["Message"] = "Please Enter Valid MobileNo";
                    dic["Focus"] = "MobileNo";
                }
                else if (string.IsNullOrWhiteSpace(EmailId))
                {
                    dic["Message"] = "Please Enter EmailId";
                    dic["Focus"] = "EmailId";
                }
                else if (!Regex.IsMatch(EmailId, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    dic["Message"] = "Please Enter Valid EmailId";
                    dic["Focus"] = "EmailId";
                }
   
                else if (string.IsNullOrWhiteSpace(Gender))
                {
                    dic["Message"] = "Please Choose Gender";
                    dic["Focus"] = "Gender";
                }
                else if (string.IsNullOrWhiteSpace(RegistrationNo) && string.IsNullOrWhiteSpace(StudentPhoto))
                {
                    dic["Message"] = "Please Select Photo";
                    dic["Focus"] = "StudentPhoto";
                }
                else if (AdmissionNo==""&& string.IsNullOrWhiteSpace(StudentPhoto) )                       
                {
                    dic["Message"] = "Please Choose Image";
                    dic["Focus"] = "StudentPhoto";
                }
                else if (!string.IsNullOrWhiteSpace(StudentPhoto) &&
                        FileType.ToLower() != "jpg" &&
                        FileType.ToLower() != "jpeg" &&
                        FileType.ToLower() != "png" &&
                        FileType.ToLower() != "webp")
                {
                    dic["Message"] = "Please select a valid image file (jpg, jpeg, png, webp)";
                    dic["Focus"] = "StudentPhoto";
                }

                else if (sizeInBytes > 300000)
                {
                    dic["Message"] = "Image size should be less than 300 KB";
                    dic["Focus"] = "StudentPhoto";
                }
                else if (string.IsNullOrWhiteSpace(City))
                {
                    dic["Message"] = "Please Enter City";
                    dic["Focus"] = "City";
                }
                else if (string.IsNullOrWhiteSpace(Address))
                {
                    dic["Message"] = "Please Enter Address";
                    dic["Focus"] = "Address";
                }
                else if (Courses == null || Courses.Count == 0)
                {
                    dic["Message"] = "Please add at least one academic qualification before saving.";
                    dic["Focus"] = "InstituteName";
                }
                else
                {

                    string CoursesXml = Common.ConvertToxml(Courses, "AcademicQualification", "Course");

                    string[,] Param = new string[,]
                    {

                        {"@AdmissionNo",AdmissionNo},
                        {"@RegistrationNo",RegistrationNo},                    
                        {"@StudentName",StudentName},
                        {"@ParentsName",ParentsName},
                        {"@ParentsProfession",ParentsProfession},
                        {"@MobileNo",MobileNo},
                        {"@StudentDob",StudentDob},
                        {"@EmailID",EmailId},                       
                        {"@Gender",Gender},
                        {"@StudentPhoto",StudentPhoto},
                        {"@City",City},
                        {"@Address",Address},
                        {"@FileName",FileName},
                        {"@FileType",FileType},
                        {"@CoursesXml",CoursesXml},
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateAdmissionMaster", Param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        Status = dt.Rows[0]["Status"].ToString();
                        dic["Status"] = Status;
                        if (Status == "1")
                        {
                            Session["AcademicQualification"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAcademicQualification(int TempId,string InstituteName, string CourseName, string TotalMarks, string ObtainedMarks, string Year,string Percentage)
        
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";
            bool IsCourseAvaliable = false;
            List<Course> Courses = Session["AcademicQualification"] as List<Course> ?? new List<Course>();
            try
            {
                for (int i = 0; i < Courses.Count; i++)
                {
                    if (CourseName.ToLower() == Courses[i].CourseName.ToLower() && Courses[i].TempId != TempId)
                    {
                        IsCourseAvaliable = true;
                    }
                }
                if (string.IsNullOrWhiteSpace(InstituteName))
                {
                    dic["Focus"] = "InstituteName";
                    dic["Message"] = "Please enter Institute Name";
                }

                else if (string.IsNullOrWhiteSpace(CourseName))
                {
                    dic["Focus"] = "Course";
                    dic["Message"] = "Please enter Course Name";
                }
                else if (string.IsNullOrWhiteSpace(TotalMarks))
                {
                    dic["Focus"] = "TotalMarks";
                    dic["Message"] = "Please enter Total Marks";
                }
                else if (string.IsNullOrWhiteSpace(ObtainedMarks))
                {
                    dic["Focus"] = "ObtainedMarks";
                    dic["Message"] = "Please enter Obtained Marks";
                }
                else if (Convert.ToInt64(ObtainedMarks) > Convert.ToInt64(TotalMarks))
                {
                    dic["Focus"] = "ObtainedMarks";
                    dic["Message"] = "Obtained Marks should be less than Total Marks.";
                }
                else if (string.IsNullOrWhiteSpace(Year))
                {
                    dic["Focus"] = "Year";
                    dic["Message"] = "Please enter Year";
                }
                else if (string.IsNullOrWhiteSpace(Percentage))
                {
                    dic["Focus"] = "Percentage";
                    dic["Message"] = "Please enter Percentage";
                }
                else if (IsCourseAvaliable)
                {
                    dic["Focus"] = "Course";
                    dic["Message"] = "Course Already Exist";
                }
                else
                {

                    bool isUpdated = false;
                    for (int i = 0; i < Courses.Count; i++)
                    {
                        if (Courses[i].TempId == TempId)
                        {
                            Courses[i].InstituteName = InstituteName;
                            Courses[i].CourseName = CourseName;
                            Courses[i].Year = Year;
                            Courses[i].TotalMarks = TotalMarks;
                            Courses[i].ObtainedMarks = ObtainedMarks;
                            Courses[i].Percentage = Percentage;                        
                            isUpdated = true;
                            dic["Message"] = "Academic Qualfication Updated Successfully";
                            dic["Focus"] = "Course";
                            dic["Status"] = "1";

                        }
                    }
                    if (!isUpdated)
                    {
                        Courses.Add(new Course
                        {
                            InstituteName = InstituteName,
                            TempId = TempId,
                            CourseName = CourseName,
                            Year = Year,
                            TotalMarks = TotalMarks,
                            ObtainedMarks = ObtainedMarks,
                            Percentage= Percentage
                         
                        });

                        dic["Message"] = "Academic Qualification  Added Successfully";
                        dic["Focus"] = "Course";
                        dic["Status"] = "1";
                    }

                    Session["AcademicQualification"] = Courses;
                                  }


            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }


            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAcademicQualification()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Messages"] = "";
            dic["Grid"] = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                List<Course> Courses = Session["AcademicQualification"] as List<Course> ?? new List<Course>();
                if (Courses.Count == 0)
                {
                    dic["Grid"] = "";
                }
                else
                {
                    sb.Append("<table class='table MT-4 table-sm'>");
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Institute Name</th><th>Course Name</th><th>Passing Year</th><th>Total Marks</th><th>Obtained Marks</th> <th>Percentage</th><th>Edit</th><th>Delete</th>");
                    sb.Append("</tr></thead><tbody>");

                    foreach (var c in Courses)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{c.InstituteName}</td>");
                        sb.Append($"<td>{c.CourseName}</td>");
                        sb.Append($"<td>{c.Year}</td>");
                        sb.Append($"<td>{c.TotalMarks}</td>");
                        sb.Append($"<td>{c.ObtainedMarks}</td>");
                        sb.Append($"<td>{c.Percentage}</td>");

                        sb.Append($"<td><button type='button' onclick='EditAcademicQualification({c.TempId})'>Edit</button></td>");
                        sb.Append($"<td><button type='button' class='deleteCourse' onclick='DeleteAcademicQualification({c.TempId})'>Delete</button></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    dic["Grid"] = sb.ToString();
                }

            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditAcademicQualification(int TempId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            try
            {
                List<Course> courses = Session["AcademicQualification"] as List<Course> ?? new List<Course>();
                for (int i = 0; i < courses.Count; i++)
                {
                    if (courses[i].TempId == TempId)
                    {
                        dic["SelectedCourse"] = courses[i];
                    }

                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAcademicQualification(int TempId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Messages"] = "";
            try
            {
                List<Course> Courses = Session["AcademicQualification"] as List<Course>;
                if (Courses != null)
                {
                    for (int i = 0; i < Courses.Count; i++)
                    {
                        if (Courses[i].TempId == TempId)
                        {
                            Courses.RemoveAt(i);
                            dic["Messages"] = "Delete Succesfull";
                            break;
                        }
                    }
                    Session["AcademicQualification"] = Courses;
                }
                else
                {
                    dic["Messages"] = "No Data Found";
                }


            }
            catch (Exception ex)
            {
                dic["Messages"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditAdmssionMaster(string AdmissionNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            Session["AcademicQualification"] = null;
            try
            {
                string[,] Param = new string[,]
                {
                    {"@AdmissionNo",AdmissionNo}
                };
                DataSet ds = Common.ExecuteProcedureDataSet("USP_ShowAdmissionMaster", Param);
                if (ds.Tables.Count > 0)
                {
                    int tableCount = ds.Tables.Count;
                    DataTable StudentRecord = ds.Tables[0];
                    dic["RegistrationNo"] = StudentRecord.Rows[0]["RegistrationNo"].ToString();
                    dic["StudentName"] = StudentRecord.Rows[0]["StudentName"].ToString();
                    dic["StudentPhoto"] = StudentRecord.Rows[0]["StudentPhoto"].ToString();
                    dic["ParentsName"] = StudentRecord.Rows[0]["ParentsName"].ToString();
                    dic["ParentsProfession"] = StudentRecord.Rows[0]["ParentsProfession"].ToString();
                    dic["StudentDob"] = Convert.ToDateTime(StudentRecord.Rows[0]["StudentDob"]).ToString("yyyy-MM-dd");
                    dic["MobileNo"] = StudentRecord.Rows[0]["MobileNo"].ToString();
                    dic["EmailId"] = StudentRecord.Rows[0]["EmailId"].ToString();
                    dic["Gender"] = StudentRecord.Rows[0]["Gender"].ToString();
                    dic["City"] = StudentRecord.Rows[0]["City"].ToString();
                    dic["Address"] = StudentRecord.Rows[0]["Address"].ToString();
                    dic["FileName"] = StudentRecord.Rows[0]["FileName"].ToString();
                    dic["FileType"] = StudentRecord.Rows[0]["FileType"].ToString();
                    DataTable QualificationRecords = ds.Tables[1];
                    if (QualificationRecords.Rows.Count > 0 && QualificationRecords.Rows != null)
                    {

                        List<Course> list = Session["CourseList"] as List<Course> ?? new List<Course>();
                        foreach (DataRow dr in QualificationRecords.Rows)
                        {
                            Course course = new Course();
                            course.InstituteName = dr["Institute"].ToString();
                            course.CourseName = dr["Course"].ToString();
                            course.Year = dr["PassingYear"].ToString();
                            course.TotalMarks = dr["TotalMarks"].ToString();
                            course.ObtainedMarks = dr["ObtainedMarks"].ToString();
                            course.Percentage = dr["Percentage"].ToString();                           
                            course.TempId = Convert.ToInt32(dr["QualificationId"]);
                            list.Add(course);
                        }
                        Session["AcademicQualification"] = list;
                    }
                }


            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowStudentDetailFromRegistrationNo(string RegistrationNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            Session["CourseList"] = null;
            try
            {
                string[,] Param = new string[,]
                {
                    {"@RegistrationNo",RegistrationNo}
                };
                DataTable dt = Common.ExecuteProcedure("USP_ShowAdmissionMasterAndStudentMAster", Param);
                if (dt.Rows.Count > 0)
                {                                   
                    dic["StudentName"] = dt.Rows[0]["StudentName"].ToString();
                    if (dt.Columns.Contains("FatherName"))
                    {
                        dic["FatherName"] = dt.Rows[0]["FatherName"].ToString();
                    }
                    if (dt.Columns.Contains("ParentsName"))
                    {
                        dic["FatherName"] = dt.Rows[0]["ParentsNAme"].ToString();
                    }

                    if (dt.Columns.Contains("DateOfBirth"))
                    {
                        dic["DateOfBirth"] = Convert.ToDateTime(dt.Rows[0]["DateOfBirth"]).ToString("yyyy-MM-dd");
                    }
                    if (dt.Columns.Contains("StudentDob"))
                    {
                        dic["DateOfBirth"] = Convert.ToDateTime(dt.Rows[0]["StudentDob"]).ToString("yyyy-MM-dd");
                    }
                    dic["MobileNo"] = dt.Rows[0]["MobileNo"].ToString();
                    dic["EmailId"] = dt.Rows[0]["EmailId"].ToString();
                    dic["Gender"] = dt.Rows[0]["Gender"].ToString();
                    //dic["StudentPhoto"] = dt.Rows[0]["StudentPhoto"].ToString();
                    dic["City"] = dt.Rows[0]["City"].ToString();
                    dic["Address"] = dt.Rows[0]["Address"].ToString();
                    //dic["FileName"] = dt.Rows[0]["FileName"].ToString();
                    //dic["FileType"] = dt.Rows[0]["FileType"].ToString();
                   
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region For Student Report
        public ActionResult StudentReport()
        {
            Session["searched"] = null;
            return View();
        }

        public ActionResult SearchStudent(string RegistrationNo, string StudentName, string FatherName, string MobileNo,
            string Gender, string RegDateFrom, string RegDateTo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Grid"] = "";
            dic["Message"] = "";
            dic["DataMsg"] = "";
            Session["searched"] = null;
            try
            {
                string[,] Param = new string[,]
            {
                {"@RegistrationNo",RegistrationNo},
                {"@StudentName",StudentName},
                {"@FatherName",FatherName},
                {"@MobileNo",MobileNo},
                {"@Gender",Gender},
                {"@RegDateFrom",RegDateFrom},
                {"@RegDateTo",RegDateTo},
            };
                DataTable dt = Common.ExecuteProcedure("SearchStudent", Param);
                if (dt.Rows.Count > 0)
                {
                    Session["searched"] = dt;
                    string sb = Common.ShowTable(dt,Report:true,PrintReport:"PrintReport");
                    dic["Grid"] = sb;
                }
                else
                {
                    Session["searched"] = null;
                    dic["DataMsg"] = "No Data Found";
                    dic["Grid"] = "";
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportSearchedStudent()
        {

            DataTable dt = Session["searched"] as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                return RedirectToAction("StudentReport");
            }
            byte[] SearchedReport = Common.ExportToExcel(dt, "SearchedReport", true);


            return File(
                SearchedReport,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "StudentReport.xlsx"
            );
        }
        #endregion

        #region For Admission Report

        public ActionResult AdmissionReport()
        {
            Session["searched"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult AdmissionReport(string AdmissionNo,string RegistrationNo, string StudentName, string ParentsName, string MobileNo,
           string Gender, string AdmDateFrom, string AdmDateTo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Grid"] = "";
            dic["Message"] = "";
            dic["DataMsg"] = "";
            Session["searched"] = null;
            try
            {
                string[,] Param = new string[,]
                {
                    {"@AdmissionNo",AdmissionNo},
                    {"@RegistrationNo",RegistrationNo},
                    {"@StudentName",StudentName},
                    {"@ParentsName",ParentsName},
                    {"@MobileNo",MobileNo},
                    {"@Gender",Gender},
                    {"@AdmDateFrom",AdmDateFrom},
                    {"@AdmDateTo",AdmDateTo},
                };
                DataTable dt = Common.ExecuteProcedure("USP_SearchStudentAdmission", Param);
                if (dt.Rows.Count > 0)
                {
                    Session["searched"] = dt;
                    string sb = Common.ShowTable(dt, Report: true, PrintReport: "PrintReport");
                    dic["Grid"] = sb;
                }
                else
                {
                    Session["searched"] = null;
                    dic["DataMsg"] = "No Data Found";
                    dic["Grid"] = "";
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportSearchedStudentOnAdmission()
        {

            DataTable dt = Session["searched"] as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                return RedirectToAction("AdmissionReport");
            }
            byte[] SearchedReport = Common.ExportToExcel(dt, "SearchedReport", true);


            return File(
                SearchedReport,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "AdmissionReport.xlsx"
            );
        }
        #endregion

        #region User Master
        public ActionResult UserMaster()
        {
            return View();
        }
        public ActionResult InsertUpdateUserMaster(string UserId,string UserName,string MobileNo,string EmailId, string Password,string Active,string Address)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Status"] = "";
            dic["Focus"] = "";
            try
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    dic["Message"] = "Please Enter User Name";
                    dic["Focus"] = "UserName";
                }
                else if (string.IsNullOrWhiteSpace(MobileNo))
                {
                    dic["Message"] = "Please Enter MobileNo";
                    dic["Focus"] = "MobileNo";
                }
                else if (MobileNo.Length!=10)
                {
                    dic["Message"] = "Please Enter Valid Mobile No";
                    dic["Focus"] = "MobileNo";
                }
                else if (string.IsNullOrWhiteSpace(EmailId))
                {
                    dic["Message"] = "Please Enter Email Id";
                    dic["Focus"] = "EmailId";
                }
                else if (!Regex.IsMatch(EmailId, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    dic["Message"] = "Please Enter Valid EmailId";
                    dic["Focus"] = "EmailId";
                }
                else if (string.IsNullOrWhiteSpace(UserId) && string.IsNullOrWhiteSpace(Password))
                {
                    dic["Message"] = "Please Enter Password";
                    dic["Focus"] = "Password";
                }
                
                else if (string.IsNullOrWhiteSpace(UserId) && !Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$"))
                {
                    dic["Message"] = "Invalid Password. Must be at least 8 characters, include uppercase, lowercase, number, and special character.";
                    dic["Focus"] = "Password";
                }
                else if (string.IsNullOrWhiteSpace(Address))
                {
                    dic["Message"] = "Please Enter Address";
                    dic["Focus"] = "Address";
                }
                else
                {
                    string[,] Param = new string[,]
                    {
                       {"@UserId",UserId },
                       {"@UserName",UserName },
                       {"@MobileNo",MobileNo },
                       {"@EmailId",EmailId },
                       {"@Password",Password },
                       {"@Address",Address },
                       {"@Active",Active },

                    };
                    DataTable dt=Common.ExecuteProcedure("USP_InsertUpdateUserMaster",Param);
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
        public ActionResult ShowUserMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Grid"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowUserMaster");
                if (dt.Rows.Count > 0)
                {
                    string Grid = Common.ShowTable(dt, dt.Rows[0]["HideColumn"].ToString(), EditFunctionName, DeleteFunctionName);
                    dic["Grid"] = Grid.ToString();
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
 
        public JsonResult EditUserMaster(string UserId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            try
            {

                string[,] Param = new string[,]
                {
                    {"@UserId",UserId }
                };
                DataTable dt = Common.ExecuteProcedure("USP_ShowUserMaster", Param);
                if (dt.Rows.Count > 0)
                {
                    dic["UserId"] = dt.Rows[0]["UserId"]?.ToString();
                    dic["UserCode"] = dt.Rows[0]["User Code"]?.ToString();
                    dic["UserName"] = dt.Rows[0]["User Name"]?.ToString();
                    dic["MobileNo"] = dt.Rows[0]["Mobile No"]?.ToString();
                    dic["EmailId"] = dt.Rows[0]["EmailId"]?.ToString();
                    dic["Address"] = dt.Rows[0]["Address"]?.ToString();
                    dic["Active"] = dt.Rows[0]["Active"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteUserMaster(string UserId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            try
            {
                string[,] Param = new string[,]
                {
                    {"@UserId",UserId }
                };
                DataTable dt = Common.ExecuteProcedure("USP_DeleteUserMaster", Param);
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
        public FileResult ExportToExcelUserMaster()
        {
            string[,] Param = new string[,]
            {
                {"@type","Excel" }
            };

            DataTable dt = Common.ExecuteProcedure("USP_ShowUserMaster", Param);
            byte[] filebytes = Common.ExportToExcel(dt);

            return File(
               filebytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "UserExport.xlsx"
           );
        }


        #endregion

        #region
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserCode,string Password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Session["UserInfo"] = null;
            dic["Message"]="";
            dic["Status"]="";
            try
            {
                string[,] Param = new string[,]
                {
                    {"@UserCode",UserCode },
                    {"@Password",Password }
                };
               DataTable dt= Common.ExecuteProcedure("USP_Login",Param);
                if (dt.Rows.Count>0)
                {                    
                    dic["Message"] = dt.Rows[0]["Msg"].ToString();
                    dic["Status"] = dt.Rows[0]["Status"].ToString();
                    if (dt.Rows[0]["Status"].ToString()=="1")
                    {
                        Dictionary<string, string> UserInfo = new Dictionary<string, string>();
                        UserInfo["UserCode"] = UserCode;
                        UserInfo["UserName"]= dt.Rows[0]["UserName"].ToString();
                        Session["UserInfo"] = UserInfo;
                    }
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }


            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login","Master");
        }
        #endregion



    }
}