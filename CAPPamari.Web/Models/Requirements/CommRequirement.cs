using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    /// <summary>
    /// Requires that a course is included that is from among *DepartmentCodes* and
    /// is communication intensive
    /// </summary>
    public class CommRequirement : Requirement
    {
        public List<string> DepartmentCodes { get; private set; }

        public CommRequirement(List<string> DepartmentCodes)
        {
            this.DepartmentCodes = DepartmentCodes;
        }

        public override bool Fulfills(CourseModel CourseTaken)
        {
            if (!CourseTaken.CommIntensive)
            {
                return false;
            }
            foreach(var deptCode in DepartmentCodes)
            {
                if (CourseTaken.DepartmentCode == deptCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}