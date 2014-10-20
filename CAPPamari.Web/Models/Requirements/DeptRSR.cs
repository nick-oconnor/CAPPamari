using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    //for requiring a certain number of courses in a department
    public class DeptRSR : RequirementSetRequirement
    {
        public string Department { get; private set; }
        public int NumCourses { get; private set; }
        
        public DeptRSR(string Department, int NumCourses)
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