using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Helpers
{
    public class AutopopulationHelper
    {

        //function to autopopulate the HASS requirement set
        public void FillHASS(List<CourseModel> CoursesTaken)
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
            SortedList<int, List<CourseModel>> HumCourses;
            SortedList<int, List<CourseModel>> SsciCourses;

            for (int i = 0; i < CoursesTaken.Count; i++)
            {
                if (humDepts.Contains(CoursesTaken[i].DepartmentCode))
                {
                    //NOTE what do i include to get ToInt32 to work?
                    int courseNum = ToInt32(CoursesTaken[i].CourseNumber);
                    if(HumCourses.ContainsKey(courseNum)){
                        HumCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else{
                        List<string> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        HumCourses[courseNum] = tempCourse;
                    }
                }
                if (ssciDepts.Contains(CoursesTaken[i].Department))
                {
                    int courseNum = ToInt32(CoursesTaken[i].CourseNumber);
                    if(SsciCourses.ContainsKey(courseNum)){
                        SsciCourses[courseNum].Add(CoursesTaken[i]);
                    }
                    else{
                        List<string> tempCourse = new List<CourseModel>(){
                            CoursesTaken[i],
                        };
                        SsciCourses[courseNum] = tempCourse;
                    }
                }
            }

            //alternate between adding hum and ssci to HASS until run out or full
            //I'm calling the HASS req set HASSreqset, not sure how to address it
            while (!HASSreqset.Full())
            {
                if (HumCourses.Count > 0)
                {
                    //NOTE does this return the value for the max key???
                    List<CourseModel> maxCourseList = HumCourses[HumCourses.Count-1];
                    if(maxCourseList.Count == 1){
                        HASSreqset.ApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        HumCourses.RemoveAt(HumCourses.Count-1);
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
                    //NOTE does this return the value for the max key???
                    List<CourseModel> maxCourseList = SsciCourses[HumCourses.Count - 1];
                    if (maxCourseList.Count == 1)
                    {
                        HASSreqset.ApplyCourse(maxCourseList[0]);
                        //remove the key from the list of remaining courses
                        SsciCourses.RemoveAt(HumCourses.Count - 1);
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