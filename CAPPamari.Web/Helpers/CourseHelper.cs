using System.Collections.Generic;
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
        /// <param name="username">Username for user to get CAPP report for</param>
        /// <returns>CAPPReportModel or null if no CAPP report exists</returns>
        public static CappReportModel GetCappReport(string username)
        {
            return EntitiesHelper.GetCappReport(username);
        }

        /// <summary>
        ///     Creates a new capp report for the user
        /// </summary>
        /// <param name="username">UserName of user to create a capp report for</param>
        /// <param name="major">Major to create a capp report in for the user</param>
        public static void CreateNewCappReport(string username, SupportedMajors major)
        {
            switch (major)
            {
                case SupportedMajors.CSCI:
                    CreateComputerScienceCappReport(username);
                    break;
            }
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

        /// <summary>
        ///     Creates a new computer science capp report for the user
        /// </summary>
        /// <param name="username">UserName of user to create a computer science capp report for</param>
        private static void CreateComputerScienceCappReport(string username)
        {
            var cappreport = EntitiesHelper.CreateNewCappReport(username, "Computer Science");
            EntitiesHelper.CreateCappReportRequirement(username, cappreport.Name, 4, 0, true, false, new List<int>
            {
                EntitiesHelper.GetCourseFulfillmentId("CSCI", "4440")
            });
            EntitiesHelper.CreateCappReportRequirement(username, cappreport.Name, 4, 0, true, false, new List<int>
            {
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ARTS", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LANG", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LITR", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COMM", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("WRIT", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSH", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PHIL", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ARTS", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LANG", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LITR", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COMM", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("WRIT", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSH", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PHIL", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ARTS", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LANG", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("LITR", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COMM", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("WRIT", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSH", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PHIL", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COGS", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSS", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ECON", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PSYC", "1xxx"),
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COGS", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSS", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ECON", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PSYC", "2xxx"),
                EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("COGS", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("STSS", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("ECON", "4xxx"),
                EntitiesHelper.GetCourseFulfillmentId("PSYC", "4xxx")
            });
            EntitiesHelper.CreateRequirementSet(username, cappreport.Name, "Unapplied Courses", false, 0, 0);
            EntitiesHelper.CreateRequirementSet(username, cappreport.Name, "Free Electives", false, 33, 33);
            var mathReqset = EntitiesHelper.CreateRequirementSet(username, cappreport.Name, "Math",
                false, 16, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, mathReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("MATH", "1010")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, mathReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("MATH", "1020")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, mathReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("MATH", "2xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, mathReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("MATH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("MATP", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "2140"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4140"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4420")
                });
            var scienceReqset = EntitiesHelper.CreateRequirementSet(username, cappreport.Name, "Science",
                false, 12, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, scienceReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("BIOL", "1010"),
                    EntitiesHelper.GetCourseFulfillmentId("BIOL", "1015")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, scienceReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("PHYS", "1100")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, scienceReqset.Name, 4, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("ASTR", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("BIOL", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ERTH", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CHEM", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHYS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ASTR", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("BIOL", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ERTH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CHEM", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHYS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ASTR", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("BIOL", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ERTH", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CHEM", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHYS", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, scienceReqset.Name, 0, 10, false,
                true, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("ERTH", "1030"),
                });
            var cscireqReqset = EntitiesHelper.CreateRequirementSet(username, cappreport.Name,
                "CSCI Required", false, 32, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "1100"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "1200")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "2500"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "2610")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "2200"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "2400")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4430")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4440")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4210")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, cscireqReqset.Name, 3, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "2300")
                });
            var csciOpReqset = EntitiesHelper.CreateRequirementSet(username, cappreport.Name,
                "CSCI Options", false, 11, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 1, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "6xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "46xx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "47xx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 1, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "6xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "46xx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "47xx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 1, 0, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("CSCI", "6xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "46xx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "47xx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 0, 10, false,
                true, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4630"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4640"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4720")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 0, 10, false,
                true, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4630"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4640"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4720")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, csciOpReqset.Name, 0, 10, false,
                true, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4630"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4640"),
                    EntitiesHelper.GetCourseFulfillmentId("ECSE", "4720")
                });
            var hassReqset = EntitiesHelper.CreateRequirementSet(username, cappreport.Name, "HASS", true,
                24, 6);
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4xxx")
                });
            EntitiesHelper.CreateRequirementInRequirementSet(username, cappreport.Name, hassReqset.Name, 4, 4, false,
                false, new List<int>
                {
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COGS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ECON", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PSYC", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "1xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "2xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("IHSS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("ARTS", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LANG", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("LITR", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("COMM", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("WRIT", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("STSH", "4xxx"),
                    EntitiesHelper.GetCourseFulfillmentId("PHIL", "4xxx")
                });
            EntitiesHelper.CreateRequirementSetRequirement(username, cappreport.Name, hassReqset.Name, 0, 9, false, true,
                new List<int>());
        }
    }
}