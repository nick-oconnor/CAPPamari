using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementSet
    {
        public string Name { get; private set; }
        public List<Requirement> Requirements { get; private set; }
        public List<CourseModel> AppliedCourses { get; private set; }

        public RequirementSet(string Name, List<Requirement> Requirements, List<CourseModel> AppliedCourses)
        {
            this.Name = Name;
            this.Requirements = Requirements;
            this.AppliedCourses = AppliedCourses;
        }

        public bool Fulfilled()
        {
            // change to return enum so that we know more about the fulfillment of the set
            // go through each requirement and make sure they are all met
            return false;
        }

        public bool CanApplyCourse(CourseModel NewCourse)
        {
            if (!AppliedCourses.Contains(NewCourse)) AppliedCourses.Add(NewCourse);
            var success = Fulfilled();
            if (!success) AppliedCourses.Remove(NewCourse);
            return success;
        }
    }
}