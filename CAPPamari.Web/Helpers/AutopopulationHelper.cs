using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAPPamari.Web.Models;

namespace CAPPamari.Web.Helpers
{
    public static class AutopopulationHelper
    {
        //function to autopopulate the HASS requirement set
        public static void FillHASS(CAPPamari.Web.Models.Requirements.RequirementSetModel HASSreqset, List<CourseModel> CoursesTaken)

        {
            List<string> humDepts = new List<string>(){
                "IHSS",
                "ARTS",
                "LANG",
                "LITR",
                "COMM",
                "WRIT",
                "STSH",
                "PHIL"
            };
            List<string> ssciDepts = new List<string>(){
                "COGS",
                "STSS",
                "ECON",
                "PSYC",
                "IHSS",
            };

            //create sorted lists of humanities and ssci courses
            //sorted with highest code first
            SortedDictionary<int, List<CourseModel>> HumCourses = new SortedDictionary<int, List<CourseModel>>();
            SortedDictionary<int, List<CourseModel>> SsciCourses = new SortedDictionary<int, List<CourseModel>>();

            for (int i = 0; i < CoursesTaken.Count; i++)
            {
                if (humDepts.Contains(CoursesTaken[i].DepartmentCode))
                {
                    int courseNum = Convert.ToInt32(CoursesTaken[i].CourseNumber);
                    if (HumCourses.ContainsKey(courseNum))
                    {
                        HumCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else
                    {
                        List<CourseModel> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        HumCourses[courseNum] = tempCourse;
                    }
                }
                else if (ssciDepts.Contains(CoursesTaken[i].DepartmentCode))
                {
                    int courseNum = Convert.ToInt32(CoursesTaken[i].CourseNumber);
                    if (SsciCourses.ContainsKey(courseNum))
                    {
                        SsciCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else
                    {
                        List<CourseModel> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        SsciCourses[courseNum] = tempCourse;
                    }
                }
            }

            //alternate between adding hum and ssci to HASS 
            //until full (or run out of courses, caught below)
            while (!HASSreqset.IsFulfilled())
            {
                if (HumCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = HumCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        HASSreqset.CanApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        HumCourses.Remove(HumCourses.Last().Key);
                    }
                    else
                    {
                        //apply the first course in the list
                        HASSreqset.CanApplyCourse(maxCourseList[0]);
                        //remove the course from list of remaining courses
                        maxCourseList.RemoveAt(0);
                        HumCourses[HumCourses.Count - 1] = maxCourseList;
                    }
                }
                if (SsciCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = SsciCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        HASSreqset.CanApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        SsciCourses.Remove(SsciCourses.Last().Key);
                    }
                    else
                    {
                        //apply the first course in the list
                        HASSreqset.CanApplyCourse(maxCourseList[0]);
                        //remove the course from list of remaining courses
                        maxCourseList.RemoveAt(0);
                        SsciCourses[HumCourses.Count - 1] = maxCourseList;
                    }
                }

                //no more courses to add, you're done
                if (HumCourses.Count == 0 && SsciCourses.Count == 0)
                {
                    return;
                }
            }
            return;
        }

        public static void fillNamedRequirements(List<CAPPamari.Web.Models.Requirements.RequirementSetModel> allSets, List<CAPPamari.Web.Models.CourseModel> courses)
        {
            // We need to find another way to do this because we don't have SingleRequirements anymore
            
            foreach (var reqset in allSets){
                foreach (CAPPamari.Web.Models.CourseModel course in courses)
                {
                    if (reqset.CanApplyCourse(course))
                    {
                        reqset.ApplyCourse(course);
                        courses.Remove(course);
                        break;
                    }
                }
            }
        }

        public static void fillLevelRequirements(List<CAPPamari.Web.Models.Requirements.RequirementSetModel> allSets, List<CAPPamari.Web.Models.CourseModel> courses){
            // We need to find another way to do this because we don't have LevelRequirement anymore
            /*
            foreach (var reqset in allSets)
		{
                foreach (var req in reqset.Requirements)
                {
                    if (req.GetType() == typeof(CAPPamari.Web.Models.Requirements.LevelRequirement))
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
            */
        }

        /// <summary>
        /// Fills in classes automatically.
        /// Note that the HASS RequirementSet must be named "HASS"
        /// </summary>
        /// <param name="requirementSets"></param>
        /// <param name="courses"></param>
        
        public static void autopopulate(List<CAPPamari.Web.Models.Requirements.RequirementSetModel> requirementSets, List<CAPPamari.Web.Models.CourseModel> courses)

        {
            fillNamedRequirements(requirementSets, courses);
            fillLevelRequirements(requirementSets, courses);

            //find and check HASS
            foreach (CAPPamari.Web.Models.Requirements.RequirementSetModel reqset in requirementSets)
            {
                if (reqset.Name == "HASS")
                {
                    FillHASS(reqset, courses);
                }
            }
        }
    }
}