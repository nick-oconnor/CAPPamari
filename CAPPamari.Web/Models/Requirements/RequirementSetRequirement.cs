using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementSetRequirement
    {
        public int TotalCreditsNeeded { get; private set; }
        public bool PNCAllowed { get; private set; }
        public abstract bool Fulfills(List<CourseModel> CoursesTaken); 
    }
}