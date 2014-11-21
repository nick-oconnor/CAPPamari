using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    internal static class EntitiesHelper
    {
        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <param name="username">Username for new user</param>
        /// <param name="password">Password for new user</param>
        /// <param name="major">Major for new user</param>
        public static void CreateNewUser(string username, string password, string major)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                var newUser = new ApplicationUser
                {
                    Username = username,
                    Password = password,
                    Major = major
                };

                entities.ApplicationUsers.Add(newUser);
                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Checks to see if Username is already taken
        /// </summary>
        /// <param name="username">Username to check in the database for existence</param>
        /// <returns>True if Username is taken, false otherwise</returns>
        public static bool UsernameExists(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                return user != null;
            }
        }

        /// <summary>
        ///     Gets the password for the username given.
        /// </summary>
        /// <param name="username">Username of user to get password for</param>
        /// <returns>Password for user with Username or string.Empty if no user is found</returns>
        public static string GetPassword(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? string.Empty : user.Password;
            }
        }

        /// <summary>
        ///     Update user information
        /// </summary>
        /// <param name="username">Username of user to update</param>
        /// <param name="password">Password to update to</param>
        /// <param name="major">Major to update to</param>
        /// <returns>True if user was updated, false otherwise</returns>
        public static bool UpdateUser(string username, string password, string major)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                if (user == null) return false;
                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = password;
                }
                user.Major = major;
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Gets the list of advisors for the user with Username
        /// </summary>
        /// <param name="username">Username for user to look up advisors</param>
        /// <returns>List of advisors for user with Username or empty list if no user is found</returns>
        public static List<Advisor> GetAdvisors(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? new List<Advisor>() : user.Advisors.ToList();
            }
        }

        /// <summary>
        ///     Gets the major for the user
        /// </summary>
        /// <param name="username">Username of user to get major for</param>
        /// <returns>Major or string.Empty if the user is not found.</returns>
        public static string GetMajor(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? string.Empty : user.Major;
            }
        }

        /// <summary>
        ///     Gets active SessionID if one exists
        /// </summary>
        /// <param name="username">Username for user to look up SessionID</param>
        /// <returns>SessionID of valid session or -1 if no such session exists</returns>
        public static int GetSessionId(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);
                if (user == null) return -1;

                UserSession session = user.UserSessions.FirstOrDefault();
                if (session == null) return -1;
                return session.Expiration < DateTime.Now ? -1 : session.SessionID;
            }
        }

        /// <summary>
        ///     Creates a new session for the user, deleting a current session if one exists
        /// </summary>
        /// <param name="username">Username to create session for</param>
        /// <returns>SessionID of newly created session</returns>
        public static int CreateNewSession(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                UserSession session = entities.UserSessions.FirstOrDefault(sess => sess.Username == username);
                if (session != null) entities.UserSessions.Remove(session);

                var newSession = new UserSession
                {
                    Username = username,
                    Expiration = DateTime.Now.AddMinutes(30)
                };
                entities.UserSessions.Add(newSession);
                entities.SaveChanges();

                return newSession.SessionID;
            }
        }

        /// <summary>
        ///     Gets the expiration of the session refered to by SessionID
        /// </summary>
        /// <param name="sessionId">SessionID for session to look up expiration</param>
        /// <returns>Expriation of session or DateTime.MinValue</returns>
        public static DateTime GetSessionExpiration(int sessionId)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                UserSession session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == sessionId);

                return session == null ? DateTime.MinValue : session.Expiration;
            }
        }

        /// <summary>
        ///     Removes session for SessionID and Username
        /// </summary>
        /// <param name="sessionId">SessionID for session to clear</param>
        /// <param name="username">Username for session to clear</param>
        public static void RemoveSession(int sessionId, string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                UserSession session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == sessionId);
                if (session == null) return;
                entities.UserSessions.Remove(session);

                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Updates a session for a user because they have committed an action
        /// </summary>
        /// <param name="username">Username of user to update session for</param>
        /// <returns>True if session is active and refreshed, false otherwise</returns>
        public static bool UpdateSession(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                UserSession session = entities.UserSessions.FirstOrDefault(sess => sess.Username == username);
                if (session == null) return false;

                session.Expiration = DateTime.Now.AddMinutes(30);
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Change a major for a specific user.
        /// </summary>
        /// <param name="username">Username of user to change major</param>
        /// <param name="major">Major to change to.</param>
        /// <returns>Success status of change.</returns>
        public static bool ChangeMajor(string username, string major)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);
                if (user == null) return false;

                user.Major = major;
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Add an advisor to the database
        /// </summary>
        /// <param name="name">Name of the new advisor to add</param>
        /// <param name="email">Email address of the new advisor to add</param>
        /// <returns>The AdvisorID of the new advisor or -1 if the advisor already exists in the database</returns>
        public static int AddAdvisor(string name, string email)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                Advisor existingAdvisor =
                    entities.Advisors.FirstOrDefault(
                        dbadvisor => dbadvisor.Name == name && dbadvisor.EmailAddress == email);
                if (existingAdvisor != null) return -1;

                var newAdvisor = new Advisor
                {
                    EmailAddress = email,
                    Name = name
                };
                entities.Advisors.Add(newAdvisor);
                entities.SaveChanges();

                return newAdvisor.AdvisorID;
            }
        }

        /// <summary>
        ///     Update advisor in database
        /// </summary>
        /// <param name="name">Name of advisor to update</param>
        /// <param name="email">Email to update advisor to</param>
        /// <returns>Success status of the update</returns>
        public static bool UpdateAdvisor(string name, string email)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                Advisor advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.Name == name);
                if (advisor == null) return false;

                advisor.EmailAddress = email;
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Gets the AdvisorID of an advisor
        /// </summary>
        /// <param name="name">Name of the advisor to get the AdvisorID of</param>
        /// <param name="email">EmailAddress of the advisor to get the AdvisorID of</param>
        /// <returns>AdvisorID corresponding to the right advisor or -1 if no such advisor exists</returns>
        public static int GetAdvisorId(string name, string email)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                Advisor advisor =
                    entities.Advisors.FirstOrDefault(
                        dbadvisor => dbadvisor.Name == name && dbadvisor.EmailAddress == email);
                if (advisor == null) return -1;

                return advisor.AdvisorID;
            }
        }

        /// <summary>
        ///     Add an association between a user and an advisor
        /// </summary>
        /// <param name="username">Username of user to create the association with</param>
        /// <param name="advisorId">AdvisorID of the advisor to create the association with</param>
        /// <returns>Success state of the association creation</returns>
        public static bool AssociateAdvisorAndUser(string username, int advisorId)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                Advisor advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == advisorId);
                if (advisor == null) return false;

                user.Advisors.Add(advisor);
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Remove an association between a user and an advisor
        /// </summary>
        /// <param name="username">Username of user to remove the association with</param>
        /// <param name="advisorId">AdvisorID of the advisor to remove the association with</param>
        /// <returns>Success state of the association deletion</returns>
        public static bool DisassociateAdvisorAndUser(string username, int advisorId)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                Advisor advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == advisorId);
                if (advisor == null) return false;

                bool success = user.Advisors.Remove(advisor);
                if (success) entities.SaveChanges();
                return success;
            }
        }

        /// <summary>
        ///     Adds a new course to the Unapplied Courses RequirementSet for a specified user
        /// </summary>
        /// <param name="username">Username for user to add new course for</param>
        /// <param name="newCourse">CourseModel containing information about the new course</param>
        /// <param name="requirementSetName">Name of the requirement set to retrieve</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string username, CourseModel newCourse, string requirementSetName)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                CAPPReport report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                RequirementSet reqSet = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSetName);
                if (reqSet == null) return false;

                reqSet.Courses.Add(new Course
                {
                    Credits = newCourse.Credits,
                    Department = newCourse.DepartmentCode,
                    Grade = newCourse.Grade,
                    Number = newCourse.CourseNumber,
                    PassNC = newCourse.PassNoCredit,
                    Semester = newCourse.Semester,
                    CommunicationIntensive = newCourse.CommIntensive
                });
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Remove a course for a specified user
        /// </summary>
        /// <param name="username">Username of user to remove course for</param>
        /// <param name="oldCourse">CourseModel containing information about course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string username, CourseModel oldCourse)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                CAPPReport report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                RequirementSet reqset =
                    report.RequirementSets.FirstOrDefault(set => set.Name == oldCourse.RequirementSetName);
                if (reqset == null) return false;

                Course courseToRemove =
                    reqset.Courses.FirstOrDefault(course => course.Department == oldCourse.DepartmentCode &&
                                                            course.Number == oldCourse.CourseNumber &&
                                                            course.Grade == oldCourse.Grade &&
                                                            course.Credits == oldCourse.Credits &&
                                                            course.Semester == oldCourse.Semester &&
                                                            course.PassNC == oldCourse.PassNoCredit &&
                                                            course.CommunicationIntensive == oldCourse.CommIntensive);
                if (courseToRemove == null) return false;

                entities.Courses.Remove(courseToRemove);
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Gets a RequirementSet for a user
        /// </summary>
        /// <param name="username">Username of user to get RequirementSet for</param>
        /// <param name="requirementSetName">Name of the requirement set to retrieve</param>
        /// <returns>RequirementSet desired or null if no such RequirementSet exists</returns>
        public static RequirementSetModel GetRequirementSet(string username, string requirementSetName)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                CAPPReport report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                RequirementSet dbset = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSetName);
                return dbset == null ? null : dbset.ToRequirementSetModel();
            }
        }

        /// <summary>
        ///     Apply a course to a requirement set for a user
        /// </summary>
        /// <param name="username">Username of user to move course for</param>
        /// <param name="course">CourseModel for course to move</param>
        /// <param name="requirementSet">RequirementSetModel to move course into</param>
        /// <returns>Success status of move</returns>
        public static bool ApplyCourse(string username, CourseModel course, RequirementSetModel requirementSet)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                CAPPReport report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                RequirementSet dbset = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSet.Name);
                if (dbset == null) return false;

                Course newCourse =
                    entities.Courses.FirstOrDefault(c => c.CommunicationIntensive == course.CommIntensive &&
                                                         c.Credits == course.Credits &&
                                                         c.Department == course.DepartmentCode &&
                                                         c.Grade == course.Grade &&
                                                         c.Number == course.CourseNumber &&
                                                         c.PassNC == course.PassNoCredit &&
                                                         c.Semester == course.Semester &&
                                                         c.RequirementSet.CAPPReport.ApplicationUser
                                                             .Username == username);
                if (newCourse == null) return false;

                newCourse.RequirementSet = dbset;
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Gets all the RequirementSets for a user
        /// </summary>
        /// <param name="username">Username for user to get all the RequirementSets for</param>
        /// <returns>List of all RequirementSets</returns>
        public static CappReportModel GetCappReport(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                CAPPReport report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                return report.ToCappReportModel();
            }
        }

        /// <summary>
        ///     Creates a new capp report for the user
        /// </summary>
        /// <param name="username">UserName for the user to add a capp report for.</param>
        /// <param name="cappReportName">Name of the resulting capp report</param>
        /// <returns>CappReportModel of the resulting capp report or null if the user is not found</returns>
        public static CappReportModel CreateNewCappReport(string username, string cappReportName)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                var newCappReport = new CAPPReport
                {
                    Name = cappReportName,
                };
                user.CAPPReports.Add(newCappReport);
                entities.SaveChanges();

                return newCappReport.ToCappReportModel();
            }
        }

        /// <summary>
        ///     Creates a requirement for the capp report
        /// </summary>
        /// <param name="username">UserName for user to create requirement for</param>
        /// <param name="cappReportName">Name of capp report to apply requirement to </param>
        /// <param name="creditsNeeded">Credits needed to fulfill this requirement</param>
        /// <param name="maxPnc">Maximum number of Pass No Credit credits for the requirement</param>
        /// <param name="commIntensive">Bool indicating whether or not the requrirement is communication intensive</param>
        /// <param name="exclusion">Bool indicating whether or not the requirement is an exclusion</param>
        /// <param name="fulfillments">List of id's the correspond to CourseFulfillments in the database</param>
        public static void CreateCappReportRequirement(string username, string cappReportName,
            int creditsNeeded, int maxPnc, bool commIntensive, bool exclusion, List<int> fulfillments)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return;

                CAPPReport cappreport = user.CAPPReports.FirstOrDefault(capp => capp.Name == cappReportName);
                if (cappreport == null) return;

                var req = new Requirement
                {
                    CommunicationIntensive = commIntensive,
                    CourseFulfillments = GetCourseFulfillmentsFromIds(fulfillments, entities).ToList(),
                    CreditsNeeded = creditsNeeded,
                    Exclusion = exclusion,
                    PassNoCreditCreditsAllowed = maxPnc
                };
                cappreport.Requirements.Add(req);
                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Adds a requirement set to a capp report for a user
        /// </summary>
        /// <param name="username">UserName of user to add requirement set for</param>
        /// <param name="cappReportName">Name of capp report to add requirement set to</param>
        /// <param name="requirementSetName">Name of requirement set to add to capp report</param>
        /// <param name="depthRequirement">Bool indicating whether or not there is a depth requirement in this requirement set</param>
        /// <param name="creditsNeeded">Credits needed to fulfill the requirement set</param>
        /// <param name="maxPnc">Maximum number of Pass No Credit credits allowed in this requirement set</param>
        /// <returns>RequirementSetModel of requirement set added or null if user or capp report could not be found</returns>
        public static RequirementSetModel CreateRequirementSet(string username, string cappReportName,
            string requirementSetName,
            bool depthRequirement, int creditsNeeded, int maxPnc)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                CAPPReport cappreport = user.CAPPReports.FirstOrDefault(capp => capp.Name == cappReportName);
                if (cappreport == null) return null;

                var reqset = new RequirementSet
                {
                    Credits = creditsNeeded,
                    DepthRSR = depthRequirement,
                    Name = requirementSetName,
                    PassNCCredits = maxPnc,
                    Description = string.Empty
                };
                cappreport.RequirementSets.Add(reqset);
                entities.SaveChanges();

                return reqset.ToRequirementSetModel();
            }
        }

        /// <summary>
        ///     Add a Requirement to a requirement set given it's vital information
        /// </summary>
        /// <param name="username">UserName of user to add requirement for</param>
        /// <param name="cappReportName">Name of capp report to add requirement to</param>
        /// <param name="requirementSetName">Name of requirement set to add requirement to</param>
        /// <param name="creditsNeeded">Number of credits needed to fulfill requirement</param>
        /// <param name="maxPnc">Maximum number of Pass No Credit credits allowed in requirement</param>
        /// <param name="commIntensive">Bool indicating whether or not the requirement is communication intensive</param>
        /// <param name="exclusion">Bool idicating whether or not the requirement is an exlusion</param>
        /// <param name="fulfillments">List of id's corresponding to CourseFulfillments from the database for this requirement</param>
        public static void CreateRequirementInRequirementSet(string username, string cappReportName,
            string requirementSetName,
            int creditsNeeded, int maxPnc, bool commIntensive, bool exclusion, List<int> fulfillments)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return;

                CAPPReport cappreport = user.CAPPReports.FirstOrDefault(capp => capp.Name == cappReportName);
                if (cappreport == null) return;

                RequirementSet reqset = cappreport.RequirementSets.FirstOrDefault(rs => rs.Name == requirementSetName);
                if (reqset == null) return;

                var req = new Requirement
                {
                    CommunicationIntensive = commIntensive,
                    CourseFulfillments = GetCourseFulfillmentsFromIds(fulfillments, entities).ToList(),
                    CreditsNeeded = creditsNeeded,
                    Exclusion = exclusion,
                    PassNoCreditCreditsAllowed = maxPnc
                };
                reqset.Requirements.Add(req);
                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Add a Requirement set requirement to a requirement set given it's vital information
        /// </summary>
        /// <param name="username">UserName of user to add requirement for</param>
        /// <param name="cappReportName">Name of capp report to add requirement to</param>
        /// <param name="requirementSetName">Name of requirement set to add requirement to</param>
        /// <param name="creditsNeeded">Number of credits needed to fulfill requirement</param>
        /// <param name="maxPnc">Maximum number of Pass No Credit credits allowed in requirement</param>
        /// <param name="commIntensive">Bool indicating whether or not the requirement is communication intensive</param>
        /// <param name="exclusion">Bool idicating whether or not the requirement is an exlusion</param>
        /// <param name="fulfillments">List of id's corresponding to CourseFulfillments from the database for this requirement</param>
        public static void CreateRequirementSetRequirement(string username, string cappReportName,
            string requirementSetName,
            int creditsNeeded, int maxPnc, bool commIntensive, bool exclusion, List<int> fulfillments)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                ApplicationUser user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return;

                CAPPReport cappreport = user.CAPPReports.FirstOrDefault(capp => capp.Name == cappReportName);
                if (cappreport == null) return;

                RequirementSet reqset = cappreport.RequirementSets.FirstOrDefault(rs => rs.Name == requirementSetName);
                if (reqset == null) return;

                var req = new Requirement
                {
                    CommunicationIntensive = commIntensive,
                    CourseFulfillments = GetCourseFulfillmentsFromIds(fulfillments, entities).ToList(),
                    CreditsNeeded = creditsNeeded,
                    Exclusion = exclusion,
                    PassNoCreditCreditsAllowed = maxPnc
                };
                reqset.RequirementSetRequirements.Add(req);
                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Get the id of the course fulfillment
        /// </summary>
        /// <param name="deptCode">Department code for the course fulfillment</param>
        /// <param name="courseCode">Course code for the course fulfillment</param>
        /// <returns>ID of the pre-existing or newly created course fulfillment</returns>
        public static int GetCourseFulfillmentId(string deptCode, string courseCode)
        {
            var deptRegex = new Regex("^[A-Z]{4}$");
            var numRegex = new Regex("^[1|2|4|6][0-9|x]{3}$");
            if (!deptRegex.IsMatch(deptCode)) return -1;
            if (!numRegex.IsMatch(courseCode)) return -1;
            using (JustinEntities entities = GetEntityModel())
            {
                CourseFulfillment fulfillment =
                    entities.CourseFulfillments.FirstOrDefault(
                        flfll => flfll.DepartmentCode == deptCode && flfll.CourseNumber == courseCode);
                if (fulfillment != null) return fulfillment.CourseFulfillmentID;

                fulfillment = new CourseFulfillment
                {
                    CourseNumber = courseCode,
                    DepartmentCode = deptCode
                };
                entities.CourseFulfillments.Add(fulfillment);
                entities.SaveChanges();

                return fulfillment.CourseFulfillmentID;
            }
        }

        /// <summary>
        ///     Returns new entities object.
        /// </summary>
        /// <returns></returns>
        private static JustinEntities GetEntityModel()
        {
            return new JustinEntities();
        }

        /// <summary>
        ///     Gets CourseFulfillments by their id's given a data context
        /// </summary>
        /// <param name="fulfillmentIds">List of fulfillment id's to find in the databsae</param>
        /// <param name="context">Current context</param>
        /// <returns>List of CourseFulfillments as they were found in the databse</returns>
        private static IEnumerable<CourseFulfillment> GetCourseFulfillmentsFromIds(IEnumerable<int> fulfillmentIds,
            JustinEntities context)
        {
            return
                fulfillmentIds.Select(id => context.CourseFulfillments.First(flfll => flfll.CourseFulfillmentID == id));
        }
    }
}