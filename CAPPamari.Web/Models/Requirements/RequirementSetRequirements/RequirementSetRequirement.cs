using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements.RequirementSetRequirements
{
    /// <summary>
    /// abstract class for Requirement Set Requirements
    /// </summary>
    public abstract class RequirementSetRequirement
    {
        public abstract bool Fulfills(List<CourseModel> CoursesTaken); 
    }
}