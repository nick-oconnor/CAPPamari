using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class CourseModel
    {
        public string DepartmentCode { get; set; }
        public string CourseNumber { get; set; }
        public double Grade { get; set; }
        public int Credits { get; set; }
        public string Semester { get; set; }

        [JsonConstructor]
        public CourseModel(string DepartmentCode, string CourseNumber, double Grade, int Credits, string Semester)
        {
            this.DepartmentCode = DepartmentCode;
            this.CourseNumber = CourseNumber;
            this.Grade = Grade;
            this.Credits = Credits;
            this.Semester = Semester;
        }

        public CourseModel()
        {
            this.DepartmentCode = "NULL";
            this.CourseNumber = "NULL";
            this.Grade = 0.0;
            this.Credits = 0;
            this.Semester = "N00";
        }
    }
}