using CAPPamari.Web.Models.Requirements.RequirementSetRequirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    /// <summary>
    /// A requirement set is made up of both "Requirements" and "Requirement Set Requirements"
    /// Example of "Requirement": you must take CSCI 1200
    /// Example of "Requirement Set Requirement": no more than 3 1000 level courses may be 
    ///     applied to the HASS requirement set
    /// </summary>
    public class RequirementSet
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Requirement> Requirements { get; private set; }
        public List<RequirementSetRequirement> RSRs { get; private set; }
        public List<CourseModel> AppliedCourses { get; private set; }

        public RequirementSet(string Name, string Description, List<Requirement> Requirements, List<RequirementSetRequirement> RSRs, List<CourseModel> AppliedCourses)
        {
            this.Name = Name;
            this.Requirements = Requirements;
            this.RSRs = RSRs;
            this.AppliedCourses = AppliedCourses;
        }

        public bool Fulfilled()
        {
            // change to return enum so that we know more about the fulfillment of the set
            // go through each requirement and make sure they are all met
            // by some course 
            for (int i = 0; i < Requirements.Count(); i++)
            {
               bool foundCourse = false;
               for(int j=0; j<AppliedCourses.Count(); j++){
                   if(Requirements[i].Fulfills(AppliedCourses[j])){
                       foundCourse = true;
                       break;
                   }
               }
                if(foundCourse == false){
                    return false;
                }
                    
            }
            // go through each requirement set requirement and make sure 
            // they are all met by the set of courses
            for (int i = 0; i < RSRs.Count(); i++)
            {
                if (RSRs[i].Fulfills(AppliedCourses) == false)
                {
                    return false;
                }
            }
            return true;
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