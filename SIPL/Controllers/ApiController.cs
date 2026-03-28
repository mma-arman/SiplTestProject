using Microsoft.Identity.Client;
using SIPL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace SIPL.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api     

        #region For PinCodeMasterApi
        [HttpPost]
        [Route("api/insertupdatepincode")]
        public JsonResult InsertUpdatePinCodeMaster(insertPinCodeApi Req)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";

           
            try
            {
                if (string.IsNullOrWhiteSpace(Req.Country))
                {
                    dic["Message"] = "Please Enter Country";                 

                }
                else if (string.IsNullOrWhiteSpace(Req.State))
                {
                    dic["Message"] = "Please Enter State Code";
                  

                }
                else if (string.IsNullOrWhiteSpace(Req.City))
                {
                    dic["Message"] = "Please Enter City";
              
                }
                else if (string.IsNullOrWhiteSpace(Req.PinCode))
                {
                    dic["Message"] = "Please Enter Pincode";
                 
                }
                else if (Req.PinCode.Length != 6)
                {
                    dic["Message"] = "PinCode Should be 6 Digit";
                    
                }
                else
                {
                    string[] Country = Req.Country.Split(':');
                    string[] State = Req.State.Split(':');

                    string[,] param = new string[,]
                    {
                        {"@PinCodeID",Req.PinCodeId??"0" },
                        {"@PinCode",Req.PinCode?.Trim() },
                        {"@CountryCode",Country[0]?.Trim() },
                        {"@StateCode",State[0] ?.Trim() },
                        {"@City",Req.City ?.Trim() },
                        {"@Active", Req.Active=(Req.Active == "yes"||Req.Active=="true"||Req.Active=="1")?"True":"False" },
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdatePinCodeMaster", param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();
                        dic["Status"]= dt.Rows[0]["Status"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Route("api/showpincode")]
        public JsonResult ShowPinCodeMaster()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                List<ShowPinCodeResponse> list = new List<ShowPinCodeResponse>();

                DataTable dt = Common.ExecuteProcedure("USP_ShowPinCodeMaster");
                if (dt.Rows.Count > 0)
                {
                    
                    foreach (DataRow data in dt.Rows)
                    {
                        ShowPinCodeResponse Ele = new ShowPinCodeResponse();
                        Ele.PinCodeId = data["PincodeId"]?.ToString();
                        Ele.PinCode = data["Pincode"]?.ToString();
                        Ele.Country = data["Country"]?.ToString();
                        Ele.State = data["State"]?.ToString();
                        Ele.City = data["City"]?.ToString();
                        Ele.Active = data["Active"].ToString();
                        Ele.CreatedDate = data["CreatedDate"]?.ToString();
                        Ele.ModifiedDate = data["ModifiedDate"]?.ToString();

                        list.Add(Ele);
                    }
                    dic["Data"] = list;
                    if (list.Count > 0)
                    {
                        dic["Message"] = "Data Fetch Succesfully";
                    }
                    else
                    {
                        dic["Message"] = "No Data Found";
                    }
                }
                else
                {
                    dic["Message"] = "No Data Found";
                }

            }


            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/editpincode")]
        public JsonResult EditPinCodeMaster(EditPinCodeRequest Api)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                if (string.IsNullOrWhiteSpace(Api.PinCodeId))
                {
                    dic["Message"] = "Please Enter PinCodeId";
                }
                else if (!Regex.IsMatch(Api.PinCodeId, @"^[0-9]+$"))
                {
                    dic["Message"] = "Please Enter Valid PinCodeId";
                }
                else
                {
                    string[,] Param = new string[,]
                    {
                        {"@PinCodeID",Api.PinCodeId }
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_ShowPinCodeMaster", Param);
                    if (dt.Rows.Count > 0)
                    {
                        List<EditPinCodeResponse> list = new List<EditPinCodeResponse>();
                        foreach (DataRow data in dt.Rows)
                        {
                            EditPinCodeResponse Ele = new EditPinCodeResponse();
                            Ele.PinCodeId = data["PincodeId"]?.ToString();
                            Ele.PinCode = data["Pincode"]?.ToString();
                            Ele.Country = data["Country"]?.ToString();
                            Ele.State = data["State"]?.ToString();
                            Ele.City = data["City"]?.ToString();


                            list.Add(Ele);
                        }
                        dic["Data"] = list;
                        if (list.Count > 0)
                        {
                            dic["Message"] = "Data Fetch Succesfully";
                        }

                    }
                    else
                    {
                        dic["Message"] = "Invalid PinCodeID";
                    }
                }
            }


            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/deletepincode")]
        public JsonResult DeletePinCodeMaster(DeletePinCodeRequest Api)
        {
            DeletePinCodeResponse Res = new DeletePinCodeResponse();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";

            try
            {
                if (string.IsNullOrWhiteSpace(Api.PinCodeId))
                {
                    dic["Message"] = "Please Enter PinCodeId";
                }
                else if(!Regex.IsMatch(Api.PinCodeId, @"^[0-9]+$"))
                {
                    dic["Message"] = "Please Enter Valid PinCodeId";
                }
                else
                {
                    string[,] Param = new string[,]
                   {
                     {"@PinCodeID",Api.PinCodeId}
                   };
                    DataTable dt = Common.ExecuteProcedure("USP_DeletePinCodeMaster", Param);
                    
                    if (dt.Rows.Count > 0)
                    {
                       Res.Message  = dt.Rows[0]["Msg"].ToString();
                        dic["Message"] = Res.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region For City Master
        [HttpPost]
        [Route("api/insertupdatecity")]
        public ActionResult InsertUpdateCityMaster(InsertUpdateCityRequest Req)
        {
             Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            dic["Focus"] = "";
            dic["Status"] = "0";

            try
            {
                if (string.IsNullOrWhiteSpace(Req.Country))
                {
                    dic["Message"] = "Please Enter Country";
                    dic["Focus"] = "txtCountryCode";
                }
                else if (string.IsNullOrWhiteSpace(Req.State))
                {
                    dic["Message"] = "Please Enter State";
                    dic["Focus"] = "txtState";
                }
                else if (string.IsNullOrWhiteSpace(Req.City))
                {
                    dic["Message"] = "Please Enter City";
                    dic["Focus"] = "txtCityName";
                }
                else
                {
                    string[] Country = Req.Country.Split(':');
                    string[] State = Req.State.Split(':');

                    string[,] Param = new string[,]
                       {
                          { "@CityID",Req.CityId},
                          { "@CountryCode",Country[0]},
                          { "@StateCode",State[0].Trim()},
                          { "@City",Req.City.Trim()},
                          { "@Active",Req.Active=(Req.Active == "yes"||Req.Active=="true"||Req.Active=="1")?"True":"False"},
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

        [HttpPost]
        [Route("api/showcity")]
        public ActionResult ShowCityMaster(string EditFunctionName, string DeleteFunctionName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                DataTable dt = Common.ExecuteProcedure("USP_ShowCityMaster");
                List<ShowCityResponse> res = new List<ShowCityResponse>();
                foreach (DataRow Row in dt.Rows)
                {
                    ShowCityResponse Ele = new ShowCityResponse();
                    Ele.CityId= Row["CityId"].ToString();
                    Ele.Country = Row["Country"].ToString();
                    Ele.State = Row["State"].ToString();
                    Ele.City = Row["City"].ToString();
                    Ele.Active = Row["Active"].ToString();
                    Ele.CreatedDate = Row["CreatedDate"].ToString();
                    Ele.ModifiedDate = Row["ModifiedDate"].ToString();
                    res.Add(Ele);
                }
                if (res.Count>0)
                {
                    dic["Message"] = "Data Fetch Succesfully";
                }               
                dic["Data"] = res;

            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/Editcity")]
        public ActionResult EditCityMaster(EditCityRequest Req)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                if (string.IsNullOrWhiteSpace(Req.CityId))
                {
                    dic["Message"] = "Please Enter CityId";
                }
                else if (!Regex.IsMatch(Req.CityId, @"^[0-9]{6}$"))
                {
                    dic["Message"] = "Invalid CityID";
                }
                else
                {
                    string[,] Param = new string[,]
                {
                    {"@CityID",Req.CityId }
                };
                    DataTable dt = Common.ExecuteProcedure("USP_ShowCityMaster", Param);
                    EditCityResponse res = new EditCityResponse();
                    if (dt.Rows.Count > 0)
                    {
                        res.CityId = dt.Rows[0]["CityID"].ToString();
                        res.Country = dt.Rows[0]["Country"].ToString();
                        res.State = dt.Rows[0]["State"].ToString();
                        res.City = dt.Rows[0]["City"].ToString();
                        res.Active = dt.Rows[0]["Active"].ToString();
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = "Data Fetch succesfully";
                        dic["Data"] = res;
                    }
                    else
                    {
                        dic["Message"] = "invalid CityId";
                    }
                }
            }
                
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/Deletecity")]
        public ActionResult DeleteCityMaster(DeleteCityRequest Req)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";

            try
            {
                if (string.IsNullOrWhiteSpace(Req.CityId))
                {
                    dic["Message"] = "Please Enter CityId";
                }
                else if (!Regex.IsMatch(Req.CityId, @"^[0-9]{6}$"))
                {
                    dic["Message"] = "Invalid CityID";
                }
                else
                {
                    string[,] Param = new string[,]
                    {
                        {"@CityID",Req.CityId?.Trim()}
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_DeleteCityMaster", Param);
                    if (dt.Rows.Count > 0)
                    {
                        dic["Message"] = dt.Rows[0]["Msg"].ToString();

                    }
                }
               
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Country Master
        [HttpPost]
        [Route("api/insertupdatecountry")]
        public ActionResult InsertUpdateCountryMaster(CountryApiRequest Req)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            try
            {
                if (string.IsNullOrWhiteSpace(Req.CountryCode))
                {
                    dic["Message"] = "Please Enter Country Code";
                    dic["Focus"] = "txtCountryCode";
                }
                else if (string.IsNullOrWhiteSpace(Req.CountryName))
                {
                    dic["Message"] = "Please Enter  Country Name";
                    dic["Focus"] = "txtCountryName";
                }
                else
                {
                    string[,] Param = new string[,]
                    {
                        {"@CountryId",Req.CountryId },
                        {"@CountryCode",Req.CountryCode },
                        {"@CountryName",Req.CountryName },
                        {"@Active",Req.Active=(Req.Active == "yes"||Req.Active=="true"||Req.Active=="1")?"True":"False" },
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_InsertUpdateCountry", Param);
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

        [HttpPost]
        [Route("api/DeleteCountry")]
        public JsonResult DeleteCountryMaster(DeleteCountryRequest Req)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Message"] = "";
            try
            {
                string[,] param = new string[,]
                {
                    {"@CountryId",Req.CountryId }
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
        [HttpPost]
        [Route("api/ShowCountry")]
        public ActionResult ShowCountryMaster()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                List<ShowCountryResponse> list = new List<ShowCountryResponse>();
                DataTable dt = Common.ExecuteProcedure("USP_ShowCountryMaster");
                if (dt.Rows.Count>0)
                {
                    foreach (DataRow data in dt.Rows)
                    {
                        ShowCountryResponse Ele = new ShowCountryResponse();
                        Ele.CountryId = data["CountryId"]?.ToString();
                        Ele.CountryCode = data["CountryCode"]?.ToString();
                        Ele.CountryName = data["CountryName"]?.ToString();
                        Ele.Active = data["Active"]?.ToString();
                        Ele.CreatedDate = data["CreatedDate"]?.ToString();
                        Ele.ModifiedDate = data["ModifiedDAte"]?.ToString();
                       

                        list.Add(Ele);
                    }
                }
                if (list.Count > 0)
                {
                    dic["Message"] = "Data Fetch Succesfully";
                }
                else
                {
                    dic["Message"] = "No Data Found";
                }

                dic["Data"] = list;
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/EditCountry")]
        public JsonResult EditCountryMaster(EditCountryRequest Req)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Message"] = "";
            dic["Data"] = "";
            try
            {
                if (string.IsNullOrWhiteSpace(Req.CountryId))
                {
                    dic["Message"] = "Please Enter CountryId";
                }
                else if (!Regex.IsMatch(Req.CountryId, @"^[0-9]+$"))
                {
                    dic["Message"] = "Please Enter Valid CountryId";
                }
                else
                {
                    string[,] Param = new string[,]
                    {
                        {"@CountryId",Req.CountryId }
                    };
                    DataTable dt = Common.ExecuteProcedure("USP_ShowCountryMaster", Param);
                    if (dt.Rows.Count > 0)
                    {
                        List<EditCountryResponse> list = new List<EditCountryResponse>();
                        foreach (DataRow data in dt.Rows)
                        {
                            EditCountryResponse Ele = new EditCountryResponse();
                            Ele.CountryId = data["CountryId"]?.ToString();
                            Ele.CountryCode = data["CountryCode"]?.ToString();
                            Ele.CountryName = data["CountryName"]?.ToString();
                            Ele.Active = data["Active"]?.ToString();
                            Ele.CreatedDate = data["CreatedDate"]?.ToString();
                            Ele.ModifiedDate = data["ModifiedDAte"]?.ToString();
                            list.Add(Ele);
                        }
                        dic["Data"] = list;
                        if (list.Count > 0)
                        {
                            dic["Message"] = "Data Fetch Succesfully";
                        }
                    }
                    else
                    {
                        dic["Message"] = "Invalid CountryId";
                    }
                }
            }
            catch (Exception ex)
            {
                dic["Message"] = ex.Message;
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        #endregion 
    }
}