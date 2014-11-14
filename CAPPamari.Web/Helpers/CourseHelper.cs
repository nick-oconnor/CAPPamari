using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAPPamari.Web.Models;

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
        public static bool AddNewCourse(string UserName, CourseModel NewCourse)
        {
            return EntitiesHelper.AddNewCourse(UserName, NewCourse);
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
        public static CAPPamari.Web.Models.Requirements.RequirementSetModel GetRequirementSet(string UserName, string RequirementSetName)
        {
            return EntitiesHelper.GetRequirementSet(UserName, RequirementSetName);
        }
        /// <summary>
        /// Gets all RequirementSets for the user
        /// </summary>
        /// <param name="UserName">UserName for user to get all RequirementSets for</param>
        /// <returns>List<RequirementSet> of all RequrementSets</returns>
        public static List<CAPPamari.Web.Models.Requirements.RequirementSetModel> GetAllRequirementSets(string UserName)
        {
            return EntitiesHelper.GetAllRequirementSets(UserName);
        }
    }
}