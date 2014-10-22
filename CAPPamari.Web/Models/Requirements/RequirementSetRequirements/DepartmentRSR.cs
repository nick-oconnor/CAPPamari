using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements.RequirementSetRequirements
{
    /// <summary>
    /// Requires *NumCourses* in the course set to be from *Department*
    /// </summary>
    public class DepartmentRSR : RequirementSetRequirement
    {
        public string Department { get; private set; }
        public int NumCourses { get; private set; }
        
        public DepartmentRSR(string Department, int NumCourses)
        {
            this.Department = Department;
            this.NumCourses = NumCourses;
        }
        
        public override bool Fulfills(List<CourseModel> CoursesTaken)
        {
            int n=0;
            for (int i = 0; i < CoursesTaken.Count(); i++)
            {
                if (CoursesTaken[i].DepartmentCode == Department)
                {
                    n++;
                }
            }
            if(n >= NumCourses){
                return true;
            }
            return false;
        }
    }
}