using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements.RequirementSetRequirements
{
    /// <summary>
    /// Requires that a specific course is not applied to a requirement set
    /// </summary>
    public class ExclusionRSR
    {
        public string DepartmentCode { get; private set; }
        public string CourseNumber { get; private set; }

        public ExclusionRSR(string DepartmentCode, string CourseNumber)
        {
            this.DepartmentCode = DepartmentCode;
            this.CourseNumber = CourseNumber;
        }

        public override bool Fulfills(List<CourseModel> CoursesTaken)
        {
            //make sure this course does not exisit anywhere in the whole world!
            for (int i = 0; i < CoursesTaken.Count(); i++)
            {
                if ((CoursesTaken[i].CourseNumber == this.CourseNumber) && 
                    (CoursesTaken[i].DepartmentCode == this.DepartmentCode))
                {
                    return false;
                }
            }
            return true;
        }
    }
}