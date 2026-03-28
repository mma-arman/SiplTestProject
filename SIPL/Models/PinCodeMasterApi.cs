using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPL.Models
{
    public class insertPinCodeApi
    {      
        public string PinCodeId { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }    
        public string Active { get; set; }
       
    }

    public class ShowPinCodeResponse
    {
        public string PinCodeId { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
    public class EditPinCodeRequest
    {
        public string PinCodeId { get; set; }
       
       
    }
    public class EditPinCodeResponse
    {
        public string PinCodeId { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Active { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }

    }


    public class DeletePinCodeRequest
    {
        public string PinCodeId { get; set; }

    }
    public class DeletePinCodeResponse
    {
        public string Message { get; set; }

    }
}