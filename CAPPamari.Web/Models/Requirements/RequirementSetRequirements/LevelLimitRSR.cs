using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements.RequirementSetRequirements
{
    /// <summary>
    /// Requires that no more than *UpperLimit* courses are less equals level *Level*
    /// </summary>
    public class LevelLimitRSR : RequirementSetRequirement
    {
        public string Level { get; private set; }
        public int UpperLimit { get; private set; }
        
        public LevelLimitRSR(string Level, int UpperLimit)
        {
            this.Level = Level;
            this.UpperLimit = UpperLimit;
        }
        
        public override bool Fulfills(List<CourseModel> CoursesTaken)
        {
            int n=0;
            for (int i = 0; i < CoursesTaken.Count(); i++)
            {
                if (CoursesTaken[i].CourseNumber[0] <= Level[0])
                {
                    n++;
                    if (n > UpperLimit)
                    {
                        return false;
                    }
                }
            }
                return true;
        }
    }
}