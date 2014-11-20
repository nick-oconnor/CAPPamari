using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class MoveCourseResponse
    {
        public bool MoveSuccessful { get; set; }
        public bool RequirementSetFulfilled { get; set; }
    }
}