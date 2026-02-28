using SIPL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIPL.Controllers
{
    public class AutoCompleteController : Controller
    {
        // GET: AutoComplete
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Autocomplete(string Search,string Type,string typeSearch)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(typeSearch) && typeSearch.Contains(":"))
            {
                string[] CountryParts = typeSearch.Split(':');
                typeSearch = CountryParts[0].Trim();
            }

            string[,] Param = new string[,]
            {
                 {"@search", Search },
                 {"@Type",Type },
                 {"@typeSearch",typeSearch}
            };

            DataTable dt = Common.ExecuteProcedure("USP_GetAutocomplete", Param);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["Selected"].ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}