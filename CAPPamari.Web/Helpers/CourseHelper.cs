using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    public static class CourseHelper
    {
        /// <summary>
        ///     Add a new course for the user
        /// </summary>
        /// <param name="UserName">UserName for user to add new course for</param>
        /// <param name="NewCourse">CourseModel contianing new course information</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string UserName, CourseModel NewCourse,
            string RequirementSetName = "Unapplied Courses")
        {
            return EntitiesHelper.AddNewCourse(UserName, NewCourse, RequirementSetName);
        }

        /// <summary>
        ///     Remove course from the database
        /// </summary>
        /// <param name="Username">UserName for user to remove course for</param>
        /// <param name="OldCourse">CourseModel conataining information about the course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string UserName, CourseModel OldCourse)
        {
            return EntitiesHelper.RemoveCourse(UserName, OldCourse);
        }

        /// <summary>
        ///     Gets a RequirementSet for the user
        /// </summary>
        /// <param name="UserName">UserName of user to get the RequirementSet for</param>
        /// <param name="RequirementSetName">Name of the requirement set to get</param>
        /// <returns>RequirementSet or null if no such RequirementSet exists for user</returns>
        public static RequirementSetModel GetRequirementSet(string UserName, string RequirementSetName)
        {
            return EntitiesHelper.GetRequirementSet(UserName, RequirementSetName);
        }

        /// <summary>
        ///     Gets CAPPReport for the user
        /// </summary>
        /// <param name="UserName">UserName for user to get CAPP Report for</param>
        /// <returns>CAPPReportModel or null if no CAPP Report exists</returns>
        public static CAPPReportModel GetCAPPReport(string UserName)
        {
            return EntitiesHelper.GetCAPPReport(UserName);
        }

        /// <summary>
        ///     Apply a course to a requirement set for a user
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
    }
}