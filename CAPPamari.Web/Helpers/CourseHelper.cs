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
            EntitiesHelper.CreateCAPPReportRequirement(UserName, cappreport.Name, 4, 0, true, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4440")
            });
            EntitiesHelper.CreateCAPPReportRequirement(UserName, cappreport.Name, 4, 0, true, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","4xxx")
            });
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
            var scienceReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name,"Science", false, 12, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, scienceReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("BIOL","1010"),
                EntitiesHelper.GetCourseFulfillmentID("BIOL","1015")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, scienceReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("PHYS","1100")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, scienceReqset.Name, 4, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("ASTR","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("BIOL","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ERTH","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CHEM","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHYS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ASTR","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("BIOL","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ERTH","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CHEM","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHYS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ASTR","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("BIOL","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ERTH","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CHEM","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHYS","4xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, scienceReqset.Name, 0, 10, false, true, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("ERTH","1030"),
            });
            var cscireqReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "CSCI Required", false, 32, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","1100"), 
                EntitiesHelper.GetCourseFulfillmentID("CSCI","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","1200") 
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","2500"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","2610")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","2200"),
                EntitiesHelper.GetCourseFulfillmentID("CSCI","2400")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4430")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4440")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4210")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, cscireqReqset.Name, 3, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","2300")
            });
            var csciOpReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "CSCI Options", false, 11, 0);
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 1, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CSCI","6xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","46xx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","47xx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 1, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CSCI","6xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","46xx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","47xx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 1, 0, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("CSCI","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("CSCI","6xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","46xx"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","47xx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 0, 10, false, true, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4630"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4640"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4720")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 0, 10, false, true, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4630"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4640"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4720")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, csciOpReqset.Name, 0, 10, false, true, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4630"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4640"),
                EntitiesHelper.GetCourseFulfillmentID("ECSE","4720")
            });
            var hassReqset = EntitiesHelper.CreateRequirementSet(UserName, cappreport.Name, "HASS", true, 24, 6);
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, hassReqset.Name, 4, 4, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","4xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, hassReqset.Name, 4, 4, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ARTS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LANG","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("LITR","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COMM","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("WRIT","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSH","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PHIL","4xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, hassReqset.Name, 4, 4, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","4xxx")
            });
            EntitiesHelper.CreateRequirementInRequirementSet(UserName, cappreport.Name, hassReqset.Name, 4, 4, false, false, new List<int>()
            {
                EntitiesHelper.GetCourseFulfillmentID("IHSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","1xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","2xxx"),
                EntitiesHelper.GetCourseFulfillmentID("IHSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("COGS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("STSS","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("ECON","4xxx"),
                EntitiesHelper.GetCourseFulfillmentID("PSYC","4xxx")
            });
            EntitiesHelper.CreateRequirementSetRequirement(UserName, cappreport.Name, hassReqset.Name, 0, 9, false, true, new List<int>());
        }
    }
}