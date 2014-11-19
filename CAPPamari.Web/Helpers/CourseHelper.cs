using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    public static class CourseHelper
    {
        /// <summary>
        /// Add a new course for the user
        /// </summary>
        /// <param name="UserName">UserName for user to add new course for</param>
        /// <param name="NewCourse">CourseModel contianing new course information</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string UserName, CourseModel NewCourse, string RequirementSetName = "Unapplied Courses")
        {
            return EntitiesHelper.AddNewCourse(UserName, NewCourse, RequirementSetName);
        }
        /// <summary>
        /// Remove course from the database
        /// </summary>
        /// <param name="Username">UserName for user to remove course for</param>
        /// <param name="OldCourse">CourseModel conataining information about the course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string UserName, CourseModel OldCourse)
        {
            return EntitiesHelper.RemoveCourse(UserName, OldCourse);
        }
        /// <summary>
        /// Gets a RequirementSet for the user
        /// </summary>
        /// <param name="UserName">UserName of user to get the RequirementSet for</param>
        /// <param name="RequirementSetName">Name of the requirement set to get</param>
        /// <returns>RequirementSet or null if no such RequirementSet exists for user</returns>
        public static RequirementSetModel GetRequirementSet(string UserName, string RequirementSetName)
        {
            return EntitiesHelper.GetRequirementSet(UserName, RequirementSetName);
        }
        /// <summary>
        /// Gets CAPPReport for the user
        /// </summary>
        /// <param name="UserName">UserName for user to get CAPP Report for</param>
        /// <returns>CAPPReportModel or null if no CAPP Report exists</returns>
        public static CAPPReportModel GetCAPPReport(string UserName)
        {
            return EntitiesHelper.GetCAPPReport(UserName);
        }
        /// <summary>
        /// Creates a new capp report for the user
        /// </summary>
        /// <param name="UserName">UserName of user to create a capp report for</param>
        /// <param name="Major">Major to create a capp report in for the user</param>
        public static void CreateNewCAPPReport(string UserName, SupportedMajors Major)
        {
            switch (Major)
            {
                case SupportedMajors.CSCI:
                    CreateComputerScienceCAPPReport(UserName);
                    break;
            }
        }
        /// <summary>
        /// Apply a course to a requirement set for a user
        /// </summary>
        /// <param name="UserName">UserName of user to move course for</param>
        /// <param name="Course">CourseModel to move</param>
        /// <param name="RequirementSet">RequirementSetModel to move course into</param>
        /// <returns>Success status of move</returns>
        public static bool ApplyCourse(string UserName, CourseModel Course, RequirementSetModel RequirementSet)
        {
            if (!RequirementSet.CanApplyCourse(Course)) return false;
            return EntitiesHelper.ApplyCourse(UserName, Course, RequirementSet);
        }

        /// <summary>
        /// Creates a new computer science capp report for the user
        /// </summary>
        /// <param name="UserName">UserName of user to create a computer science capp report for</param>
        private static void CreateComputerScienceCAPPReport(string UserName)
        {
            var cappreport = EntitiesHelper.CreateNewCAPPReport(UserName, "Computer Science");
            // put in capp report requirements
            //    add course fulfillments to each requirement
            EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "Unapplied Courses", false, 0, 0);
            EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "Free Electives", false, 33, 33);
            var mathReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "Math", false, 16, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, mathReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("MATH","1010")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, mathReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("MATH","1020")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, mathReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("MATH","2xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, mathReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("MATH","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("MATP","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","2140"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","4140"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","4420")
            });
            //    add Math requirements
            //       add course fulfillments to each requirement
            var scienceReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name,"Science", false, 12, 0);
            //    add Science requirements
            //       add course fulfillments to each requirement
            var cscireqReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "CSCI Required", false, 32, 0);
            //    add CSCI Required requirements
            //       add course fulfillments to each requirement
            var csciOpReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "CSCI Options", false, 11, 0);
            //    add CSCI Options requirements
            //       add course fulfillments to each requirement
            var hassReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "HASS", true, 24, 6);
            //    add HASS requirements
            //    add HASS requirement set requirements
            //       add course fulfillments to each requirement
        }
    }
}