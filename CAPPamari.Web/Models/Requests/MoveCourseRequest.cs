using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class MoveCourseRequest
    {
        public string UserName { get; set; }
        public CourseModel CourseToMove { get; set; }
        public string RequirementSetName { get; set; }
    }
}