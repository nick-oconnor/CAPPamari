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
            if (CourseTaken.Communication == false)
            {
                return false;
            }
            for (int i = 0; i < DepartmentCodes.Count(); i++)
            {
                if (CourseTaken.DepartmentCode == DepartmentCodes[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}