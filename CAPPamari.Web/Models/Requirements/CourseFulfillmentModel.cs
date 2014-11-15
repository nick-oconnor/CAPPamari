using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class CourseFulfillmentModel
    {
        public string DepartmentCode { get; set; }
        public string CourseNumber { get; set; }

        public bool Match(CourseModel Course)
        {
            if (DepartmentCode != Course.DepartmentCode) return false;
            for (int i = 0; i < 4; i++)
            {
                if (CourseNumber[i] == 'x') continue;
                if (Convert.ToInt32(CourseNumber[i]) > Convert.ToInt32(Course.CourseNumber[i])) return false;
            }
            return true;
        }
    }
}