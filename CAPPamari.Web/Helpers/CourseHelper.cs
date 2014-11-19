using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    public static class CourseHelper
    {
        /// <summary>
        ///     Add a new course for the user
        /// </summary>
        /// <param name="username">Username for user to add new course for</param>
        /// <param name="newCourse">CourseModel contianing new course information</param>
        /// <param name="requirementSetName">Name of the requirement set to get</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string username, CourseModel newCourse,
            string requirementSetName = "Unapplied Courses")
        {
            return EntitiesHelper.AddNewCourse(username, newCourse, requirementSetName);
        }

        /// <summary>
        ///     Remove course from the database
        /// </summary>
        /// <param name="username">Username for user to remove course for</param>
        /// <param name="oldCourse">CourseModel conataining information about the course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string username, CourseModel oldCourse)
        {
            return EntitiesHelper.RemoveCourse(username, oldCourse);
        }

        /// <summary>
        ///     Gets a RequirementSet for the user
        /// </summary>
        /// <param name="username">Username of user to get the RequirementSet for</param>
        /// <param name="requirementSetName">Name of the requirement set to get</param>
        /// <returns>RequirementSet or null if no such RequirementSet exists for user</returns>
        public static RequirementSetModel GetRequirementSet(string username, string requirementSetName)
        {
            return EntitiesHelper.GetRequirementSet(username, requirementSetName);
        }

        /// <summary>
        ///     Gets CAPPReport for the user
        /// </summary>
        /// <param name="username">Username for user to get CAPP Report for</param>
        /// <returns>CAPPReportModel or null if no CAPP Report exists</returns>
        public static CappReportModel GetCappReport(string username)
        {
            return EntitiesHelper.GetCappReport(username);
        }

        /// <summary>
        ///     Apply a course to a requirement set for a user
        /// </summary>
        /// <param name="username">Username of user to move course for</param>
        /// <param name="course">CourseModel to move</param>
        /// <param name="requirementSet">RequirementSetModel to move course into</param>
        /// <returns>Success status of move</returns>
        public static bool ApplyCourse(string username, CourseModel course, RequirementSetModel requirementSet)
        {
            return requirementSet.CanApplyCourse(course) && EntitiesHelper.ApplyCourse(username, course, requirementSet);
        }
    }
}