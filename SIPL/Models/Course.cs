using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPL.Models
{
    public class Course
    {
        public int TempId { get; set; } 
        public string InstituteName { get; set; } 
        public string CourseName { get; set; }
        public string TotalMarks { get; set; }
        public string ObtainedMarks { get; set; }
        public string Percentage { get; set; }
        public string Year { get; set; }
    }
}