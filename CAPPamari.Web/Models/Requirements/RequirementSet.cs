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
            return true;
        }

        public void ApplyCourse(CourseModel NewCourse)
        {
            if (!AppliedCourses.Contains(NewCourse)) AppliedCourses.Add(NewCourse);
        }
    }
}