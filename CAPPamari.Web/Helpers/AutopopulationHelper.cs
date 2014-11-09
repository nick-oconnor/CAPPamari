using CAPPamari.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Helpers
{
    public class AutopopulationHelper
    {

        //function to autopopulate the HASS requirement set
        public void FillHASS(RequirementSet HASSreqset, List<CourseModel> CoursesTaken)
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
            SortedDictionary<int, List<CourseModel>> HumCourses;
            SortedDictionary<int, List<CourseModel>> SsciCourses;

            for (int i = 0; i < CoursesTaken.Count; i++)
            {
                if (humDepts.Contains(CoursesTaken[i].DepartmentCode))
                {
]                    int courseNum = Convert.ToInt32(CoursesTaken[i].CourseNumber);
                    if(HumCourses.ContainsKey(courseNum)){
                        HumCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else{
                        List<CourseModel> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        HumCourses[courseNum] = tempCourse;
                    }
                }
                else if (ssciDepts.Contains(CoursesTaken[i].DepartmentCode))
                {
                    int courseNum = Convert.ToInt32(CoursesTaken[i].CourseNumber);
                    if(SsciCourses.ContainsKey(courseNum)){
                        SsciCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else{
                        List<CourseModel> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        SsciCourses[courseNum] = tempCourse;
                    }
                }
            }

            //alternate between adding hum and ssci to HASS 
            //until full (or run out of courses, caught below)
            while (!HASSreqset.Full())
            {
                if (HumCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = HumCourses.Last().Value;
                    if(maxCourseList.Count == 1){
                        HASSreqset.ApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        HumCourses.Remove(HumCourses.Last().Key);
                    }
                    else{
                        //apply the first course in the list
                        HASSreqset.ApplyCourse(maxCourseList[0]);
                        //remove the course from list of remaining courses
                        maxCourseList.RemoveAt(0);
                        HumCourses[HumCourses.Count-1] = maxCourseList;
                    }
                }
                if (SsciCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = SsciCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        HASSreqset.ApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        SsciCourses.Remove(SsciCourses.Last().Key);
                    }
                    else
                    {
                        //apply the first course in the list
                        HASSreqset.ApplyCourse(maxCourseList[0]);
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




    }
}