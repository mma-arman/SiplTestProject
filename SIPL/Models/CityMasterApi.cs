using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPL.Models
{
    public class InsertUpdateCityRequest
    {
        public string CityId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }    
        public string Active { get; set; }    

    }
    public class ShowCityResponse
    {
        public string CityId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
    public class EditCityRequest
    {
        public string CityId { get; set; }

    }
    public class EditCityResponse
    {
        public string CityId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }

    }
    
    public class DeleteCityRequest
    {
        public string CityId { get; set; }
    }
}