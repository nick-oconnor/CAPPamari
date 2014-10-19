using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public abstract class Requirement
    {
        public int CreditsNeeded { get; private set; }
        public abstract bool Fulfills(CourseModel CourseTaken); 
    }
}