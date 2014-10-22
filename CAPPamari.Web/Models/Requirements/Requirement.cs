using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    /// <summary>
    /// Abstract class for a requirement
    /// </summary>
    public abstract class Requirement
    {
        public int CreditsNeeded { get; private set; }
        public abstract bool Fulfills(CourseModel CourseTaken); 
    }
}