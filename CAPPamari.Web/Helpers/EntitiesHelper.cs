using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    internal static class EntitiesHelper
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="UserName">UserName for new user</param>
        /// <param name="Password">Password for new user</param>
        /// <param name="Major">Major for new user</param>
        public static void CreateNewUser(string UserName, string Password, string Major)
        {
            using (var entities = GetEntityModel())
            {
                var newUser = new ApplicationUser()
                {
                    UserName = UserName,
                    Password = Password,
                    Major = Major
                };

                entities.ApplicationUsers.Add(newUser);
                entities.SaveChanges();
            }
        }
        /// <summary>
        /// Checks to see if UserName is already taken
        /// </summary>
        /// <param name="UserName">UserName to check in the database for existence</param>
        /// <returns>True if UserName is taken, false otherwise</returns>
        public static bool UserNameExists(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                return user != null;
            }
        }
        /// <summary>
        /// Gets the password for the user name given.
        /// </summary>
        /// <param name="UserName">UserName of user to get password for</param>
        /// <returns>Password for user with UserName or string.Empty if no user is found</returns>
        public static string GetPassword(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);

                if (user == null) return string.Empty;
                return user.Password;
            }
        }
        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="UserName">UserName of user to update</param>
        /// <param name="Password">Password to update to</param>
        /// <param name="Major">Major to update to</param>
        /// <returns>True if user was updated, false otherwise</returns>
        public static bool UpdateUser(string UserName, string Password, string Major)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);

                if (user == null) return false;

                user.Password = Password;
                user.Major = Major;
                entities.SaveChanges();

                return true;
            }
        }
        /// <summary>
        /// Gets the list of advisors for the user with UserName
        /// </summary>
        /// <param name="UserName">UserName for user to look up advisors</param>
        /// <returns>List of advisors for user with UserName or empty list if no user is found</returns>
        public static List<Advisor> GetAdvisors(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);

                if (user == null) return new List<Advisor>();
                return user.Advisors.ToList();
            }
        }
        public static string GetMajor(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);

                if (user == null) return string.Empty; 
                return user.Major;
            }
        }
        /// <summary>
        /// Gets active SessionID if one exists
        /// </summary>
        /// <param name="UserName">UserName for user to look up SessionID</param>
        /// <returns>SessionID of valid session or -1 if no such session exists</returns>
        public static int GetSessionID(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);
                if (user == null) return -1;

                var session = user.UserSessions.FirstOrDefault();
                if (session == null) return -1;
                return session.Expiration < DateTime.Now ? -1 : session.SessionID;
            }
        }
        /// <summary>
        /// Creates a new session for the user, deleting a current session if one exists 
        /// </summary>
        /// <param name="UserName">UserName to create session for</param>
        /// <returns>SessionID of newly created session</returns>
        public static int CreateNewSession(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.UserName == UserName);
                if (session != null) entities.UserSessions.Remove(session);

                var newSession = new UserSession()
                {
                    UserName = UserName,
                    Expiration = DateTime.Now.AddMinutes(30)
                };
                entities.UserSessions.Add(newSession);
                entities.SaveChanges();

                return newSession.SessionID;
            }
        }
        /// <summary>
        /// Gets the expiration of the session refered to by SessionID
        /// </summary>
        /// <param name="SessionID">SessionID for session to look up expiration</param>
        /// <returns>Expriation of session or DateTime.MinValue</returns>
        public static DateTime GetSessionExpiration(int SessionID)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == SessionID);

                if (session == null) return DateTime.MinValue;
                return session.Expiration;
            }
        }
        /// <summary>
        /// Removes session for SessionID and UserName
        /// </summary>
        /// <param name="SessionID">SessionID for session to clear</param>
        /// <param name="UserName">UserName for session to clear</param>
        public static void RemoveSession(int SessionID, string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == SessionID);
                if (session == null) return;
                entities.UserSessions.Remove(session);

                entities.SaveChanges();
            }
        }
        /// <summary>
        /// Change a major for a specific user.
        /// </summary>
        /// <param name="UserName">UserName of user to change major</param>
        /// <param name="Major">Major to change to.</param>
        /// <returns>Success status of change.</returns>
        public static bool ChangeMajor(string UserName, string Major)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.UserName == UserName);
                if (user == null) return false;

                user.Major = Major;
                entities.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// Add an advisor to the database
        /// </summary>
        /// <param name="Name">Name of the new advisor to add</param>
        /// <param name="Email">Email address of the new advisor to add</param>
        /// <returns>The AdvisorID of the new advisor or -1 if the advisor already exists in the database</returns>
        public static int AddAdvisor(string Name, string Email)
        {
            using (var entities = GetEntityModel())
            {
                var existingAdvisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.Name == Name && dbadvisor.EMailAddress == Email);
                if (existingAdvisor != null) return -1;

                var newAdvisor = new Advisor()
                {
                    EMailAddress = Email,
                    Name = Name
                };
                entities.Advisors.Add(newAdvisor);
                entities.SaveChanges();

                return newAdvisor.AdvisorID;
            }
        }
        /// <summary>
        /// Update advisor in database
        /// </summary>
        /// <param name="Name">Name of advisor to update</param>
        /// <param name="EMail">EMail to update advisor to</param>
        /// <returns>Success status of the update</returns>
        public static bool UpdateAdvisor(string Name, string EMail)
        {
            using (var entities = GetEntityModel())
            {
                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.Name == Name);
                if (advisor == null) return false;

                advisor.EMailAddress = EMail;
                entities.SaveChanges();

                return true;
            }
        }
        /// <summary>
        /// Gets the AdvisorID of an advisor
        /// </summary>
        /// <param name="Name">Name of the advisor to get the AdvisorID of</param>
        /// <param name="Email">EMailAddress of the advisor to get the AdvisorID of</param>
        /// <returns>AdvisorID corresponding to the right advisor or -1 if no such advisor exists</returns>
        public static int GetAdvisorID(string Name, string Email)
        {
            using (var entities = GetEntityModel())
            {
                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.Name == Name && dbadvisor.EMailAddress == Email);
                if (advisor == null) return -1;

                return advisor.AdvisorID;
            }
        }
        /// <summary>
        /// Add an association between a user and an advisor
        /// </summary>
        /// <param name="UserName">UserName of user to create the association with</param>
        /// <param name="AdvisorID">AdvisorID of the advisor to create the association with</param>
        /// <returns>Success state of the association creation</returns>
        public static bool AssociateAdvisorAndUser(string UserName, int AdvisorID)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return false;

                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == AdvisorID);
                if (advisor == null) return false;

                user.Advisors.Add(advisor);
                entities.SaveChanges();

                return true;
            }
        }
        /// <summary>
        /// Remove an association between a user and an advisor 
        /// </summary>
        /// <param name="UserName">UserName of user to remove the association with</param>
        /// <param name="AdvisorID">AdvisorID of the advisor to remove the association with</param>
        /// <returns>Success state of the association deletion</returns>
        public static bool DisassociateAdvisorAndUser(string UserName, int AdvisorID)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return false;

                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == AdvisorID);
                if (advisor == null) return false;

                var success = user.Advisors.Remove(advisor);
                if (success) entities.SaveChanges();
                return success;
            }
        }
        /// <summary>
        /// Adds a new course to the Unapplied Courses RequirementSet for a specified user
        /// </summary>
        /// <param name="UserName">UserName for user to add new course for</param>
        /// <param name="NewCourse">CourseModel containing information about the new course</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string UserName, CourseModel NewCourse)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return false;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                var unassignedCourses = report.RequirementSets.FirstOrDefault(set => set.Name == "Unapplied Courses");
                if (unassignedCourses == null) return false;

                unassignedCourses.Courses.Add(new Course()
                {
                    Credits = NewCourse.Credits,
                    Department = NewCourse.DepartmentCode,
                    Grade = NewCourse.Grade,
                    Number = NewCourse.CourseNumber,
                    PassNC = NewCourse.PassNoCredit,
                    Semester = NewCourse.Semester
                });
                entities.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// Remove a course for a specified user
        /// </summary>
        /// <param name="UserName">UserName of user to remove course for</param>
        /// <param name="OldCourse">CourseModel containing information about course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string UserName, CourseModel OldCourse)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return false;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                foreach (var reqSet in report.RequirementSets)
                {
                    var remover = reqSet.Courses.FirstOrDefault(course => course.Department == OldCourse.DepartmentCode && course.Number == OldCourse.CourseNumber);
                    if (remover != null)
                    {
                        reqSet.Courses.Remove(remover);
                        entities.SaveChanges();
                        return true;
                    }
                }

                return false;
            }
        }
        /// <summary>
        /// Gets a RequirementSet for a user
        /// </summary>
        /// <param name="UserName">UserName of user to get RequirementSet for</param>
        /// <param name="RequirementSetName">Name of the requirement set to retrieve</param>
        /// <returns>RequirementSet desired or null if no such RequirementSet exists</returns>
        public static RequirementSetModel GetRequirementSet(string UserName, string RequirementSetName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return null;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                var dbset = report.RequirementSets.FirstOrDefault(set => set.Name == RequirementSetName);
                if (dbset == null) return null;

                return dbset.ToRequirementSetModel();
            }
        }
        /// <summary>
        /// Gets all the RequirementSets for a user
        /// </summary>
        /// <param name="UserName">UserName for user to get all the RequirementSets for</param>
        /// <returns>List<RequirementSet> of all RequirementSets</returns>
        public static CAPPReportModel GetCAPPReport(string UserName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.UserName == UserName);
                if (user == null) return null;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                return report.ToCAPPReportModel();
            }
        }

        /// <summary>
        /// Returns new entities object.
        /// </summary>
        /// <returns></returns>
        private static JustinEntities GetEntityModel()
        {
            return new JustinEntities();
        }
    }
}