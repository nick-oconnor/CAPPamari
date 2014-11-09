using CAPPamari.Web.Models.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Helpers
{
    public class AutopopulationHelper
    {
        public void fillNamedRequirements(List<RequirementSet> allSets, List<CAPPamari.Web.Models.CourseModel> courses)
        {

            foreach (RequirementSet reqset in allSets){
                foreach (Requirement req in reqset.Requirements)
                {
                    if (req is SingleRequirement)
                    {
                        foreach (CAPPamari.Web.Models.CourseModel course in courses)
                        {
                            if (req.Fulfills(course))
                            {
                                reqset.AppliedCourses.Add(course);
                                courses.Remove(course);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void fillLevelRequirements(List<RequirementSet> allSets, List<CAPPamari.Web.Models.CourseModel> courses){
            foreach (RequirementSet reqset in allSets){
                foreach (Requirement req in reqset.Requirements)
                {
                    if (req is LevelRequirement)
                    {
                        foreach (CAPPamari.Web.Models.CourseModel course in courses)
                        {
                            if (req.Fulfills(course))
                            {
                                reqset.AppliedCourses.Add(course);
                                courses.Remove(course);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void autopopulate(List<RequirementSet> requirements, List<CAPPamari.Web.Models.CourseModel> courses)
        {
            fillNamedRequirements(requirements, courses);
            fillLevelRequirements(requirements, courses);
        }
    }
}