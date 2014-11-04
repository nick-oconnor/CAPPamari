using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    /// <summary>
    /// Requires a course from among *DepartmentCodes* that is of level
    /// greater than or equal to *MinLevel*
    /// </summary>
    public class LevelRequirement : Requirement
    {
        public List<string> DepartmentCodes { get; private set; }
        public string MinLevel { get; private set; }

        public LevelRequirement(List<string> DepartmentCodes, string MinLevel, int CreditsNeeded)
        {
            this.DepartmentCodes = DepartmentCodes;
            this.MinLevel = MinLevel;
            this.CreditsNeeded = CreditsNeeded;
        }

        public override bool Fulfills(CourseModel CourseTaken)
        {
            //I think this compares the first digits
            //does it work? does an atof conversion need to happen?
            if (CourseTaken.CourseNumber[0] < MinLevel[0])
            {
                return false;
            }
            if (CourseTaken.Credits < CreditsNeeded)
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