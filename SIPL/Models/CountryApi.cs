using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPL.Models
{
    public class CountryApiRequest
    {
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Active { get; set; }
    }
    public class DeleteCountryRequest
    {
        public string CountryId { get; set; }
    }
    public class ShowCountryResponse
    {
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
    public class  EditCountryRequest
    {
        public string CountryId { get; set; }

    }
    public class EditCountryResponse
    {
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}