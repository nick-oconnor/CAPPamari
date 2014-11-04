using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models.Requirements
{
    /// <summary>
    /// Requires a certain named course, identified by a 
    /// *DepartmentCode* (ex "CSCI")
    /// and a *CourseNumber* (ex "1200")
    /// </summary>
    public class SingleRequirement : Requirement
    {
        public string DepartmentCode { get; private set; }
        public string CourseNumber { get; private set; }

        public SingleRequirement(string DepartmentCode, string CourseNumber)
        {
            this.DepartmentCode = DepartmentCode;
            this.CourseNumber = CourseNumber;
        }

        public override bool Fulfills(CourseModel CourseTaken)
        {
            return CourseTaken.DepartmentCode == DepartmentCode &&
                   CourseTaken.CourseNumber == CourseNumber &&
                   CourseTaken.Credits >= CreditsNeeded; 
        }
    }
}