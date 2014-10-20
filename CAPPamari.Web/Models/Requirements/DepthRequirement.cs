using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class DepthRequirement : RequirementSetRequirement
    {
        //no member variables i think

        public DepthRequirement()
        {
            
        }

        public override bool Fulfills(List<CourseModel> CoursesTaken)
        {
            //first find higher level course
            for (int i = 0; i < CoursesTaken.Count(); i++)
            {
                if ((CoursesTaken[i].CourseNumber[0] >= '2') && 
                    (CoursesTaken[i].PNC == false) && (CoursesTaken[i].Credits >=4))
                {
                    string dept = CoursesTaken[i].DepartmentCode;
                    //then look for matching second course
                    for (int j = 0; j < CoursesTaken.Count(); j++)
                    {
                        if ((i != j) && (CoursesTaken[j].DepartmentCode == dept)
                            && (CoursesTaken[j].PNC == false) && (CoursesTaken[i].Credits >= 4))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}