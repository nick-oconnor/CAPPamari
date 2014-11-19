using System;
using System.Collections.Generic;
using System.Linq;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    public static class AutopopulationHelper
    {
        //function to autopopulate the HASS requirement set
        public static void FillHass(RequirementSetModel hassReqSet, List<CourseModel> coursesTaken)

        {
            var humDepts = new List<string>
            {
                "IHSS",
                "ARTS",
                "LANG",
                "LITR",
                "COMM",
                "WRIT",
                "STSH",
                "PHIL"
            };
            var ssciDepts = new List<string>
            {
                "COGS",
                "STSS",
                "ECON",
                "PSYC",
                "IHSS",
            };

            //create sorted lists of humanities and ssci courses
            //sorted with highest code first
            var humCourses = new SortedDictionary<int, List<CourseModel>>();
            var ssciCourses = new SortedDictionary<int, List<CourseModel>>();

            foreach (var courseTaken in coursesTaken)
            {
                if (humDepts.Contains(courseTaken.DepartmentCode))
                {
                    var courseNum = Convert.ToInt32(courseTaken.CourseNumber);
                    if (humCourses.ContainsKey(courseNum))
                    {
                        humCourses[courseNum].Add(courseTaken);
                    }
                    else
                    {
                        var tempCourse = new List<CourseModel>
                        {
                            courseTaken,
                        };
                        humCourses[courseNum] = tempCourse;
                    }
                }
                else if (ssciDepts.Contains(courseTaken.DepartmentCode))
                {
                    var courseNum = Convert.ToInt32(courseTaken.CourseNumber);
                    if (ssciCourses.ContainsKey(courseNum))
                    {
                        ssciCourses[courseNum].Add(courseTaken);
                    }
                    else
                    {
                        var tempCourse = new List<CourseModel>
                        {
                            courseTaken,
                        };
                        ssciCourses[courseNum] = tempCourse;
                    }
                }
            }

            //alternate between adding hum and ssci to HASS 
            //until full (or run out of courses, caught below)
            while (!hassReqSet.IsFulfilled())
            {
                if (humCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    var maxCourseList = humCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        hassReqSet.CanApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        humCourses.Remove(humCourses.Last().Key);
                    }
                    else
                    {
                        //apply the first course in the list
                        hassReqSet.CanApplyCourse(maxCourseList[0]);
                        //remove the course from list of remaining courses
                        maxCourseList.RemoveAt(0);
                        humCourses[humCourses.Count - 1] = maxCourseList;
                    }
                }
                if (ssciCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    var maxCourseList = ssciCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        hassReqSet.CanApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        ssciCourses.Remove(ssciCourses.Last().Key);
                    }
                    else
                    {
                        //apply the first course in the list
                        hassReqSet.CanApplyCourse(maxCourseList[0]);
                        //remove the course from list of remaining courses
                        maxCourseList.RemoveAt(0);
                        ssciCourses[humCourses.Count - 1] = maxCourseList;
                    }
                }

                //no more courses to add, you're done
                if (humCourses.Count == 0 && ssciCourses.Count == 0)
                {
                    return;
                }
            }
        }

        public static void FillNamedRequirements(List<RequirementSetModel> allSets, List<CourseModel> courses)
        {
            // We need to find another way to do this because we don't have SingleRequirements anymore

            foreach (var reqset in allSets)
            {
                RequirementSetModel reqset1 = reqset;
                foreach (var course in courses.Where(reqset1.CanApplyCourse))
                {
                    reqset.ApplyCourse(course);
                    courses.Remove(course);
                    course.RequirementSetName = reqset.Name;
                    break;
                }
            }
        }

        /// <summary>
        ///     Fills in classes automatically.
        ///     Note that the HASS RequirementSet must be named "HASS"
        /// </summary>
        /// <param name="requirementSets"></param>
        /// <param name="courses"></param>
        public static void AutoPopulate(List<RequirementSetModel> requirementSets, List<CourseModel> courses)
        {
            FillNamedRequirements(requirementSets, courses);

            //find and check HASS
            foreach (var reqset in requirementSets.Where(reqset => reqset.Name == "HASS"))
            {
                FillHass(reqset, courses);
            }
        }
    }
}