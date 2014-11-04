using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements.RequirementSetRequirements
{
    /// <summary>
    /// Requires a pair of courses from the set to fulfill the depth requirement
    /// Used in the HASS requirement set only
    /// </summary>
    public class DepthRSR : RequirementSetRequirement
    {
        //no member variables i think

        public DepthRSR()
        {
            
        }

        public override bool Fulfills(List<CourseModel> CoursesTaken)
        {
            //first find higher level course
            for (int i = 0; i < CoursesTaken.Count(); i++)
            {
                if ((CoursesTaken[i].CourseNumber[0] >= '2') && 
                    (!CoursesTaken[i].PassNoCredit) && (CoursesTaken[i].Credits >=4))
                {
                    string dept = CoursesTaken[i].DepartmentCode;
                    //then look for matching second course
                    for (int j = 0; j < CoursesTaken.Count(); j++)
                    {
                        if ((i != j) && (CoursesTaken[j].DepartmentCode == dept)
                            && (!CoursesTaken[j].PassNoCredit) && (CoursesTaken[i].Credits >= 4))
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