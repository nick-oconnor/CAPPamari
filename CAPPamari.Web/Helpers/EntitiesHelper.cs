using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using CAPPamari.Web.Models;

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
        /// Gets the major for the user with UserName
        /// </summary>
        /// <param name="UserName">UserName of user to lookup major</param>
        /// <returns>Major for user with UserName or string.Empty if no user is found</returns>
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
                return session.SessionID;
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
        /// Returns new entities object.
        /// </summary>
        /// <returns></returns>
        private static JustinEntities GetEntityModel()
        {
            return new JustinEntities();
        }
    }
}