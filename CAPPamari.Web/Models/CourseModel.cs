using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class CourseModel
    {
        public string DepartmentCode { get; private set; }
        public string CourseNumber { get; private set; }
        public double Grade { get; private set; }
        public int Credits { get; private set; }
        public string Semester { get; private set; }

        [JsonConstructor]
        public CourseModel(string DepartmentCode, string CourseNumber, double Grade, int Credits, string Semester)
        {
            this.DepartmentCode = DepartmentCode;
            this.CourseNumber = CourseNumber;
            this.Grade = Grade;
            this.Credits = Credits;
            this.Semester = Semester;
        }
    }
}