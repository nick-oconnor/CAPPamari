using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using CAPPamari.Web.Models;

namespace CAPPamari.Web.Helpers
{
    public static class UserHelper
    {
        public static ApplicationUserModel CreateNewUser(string UserName, string Password, string Major)
        {
            EntitiesHelper.CreateNewUser(UserName, Password, Major);
            return GetApplicationUser(UserName);
        }
        /// <summary>
        /// Get application user for UserName
        /// </summary>
        /// <param name="UserName">UserName of user to get model</param>
        /// <returns>ApplicationUserModel for UserName</returns>
        public static ApplicationUserModel GetApplicationUser(string UserName)
        {
            var sessionID = EntitiesHelper.GetSessionID(UserName);
            var major = EntitiesHelper.GetMajor(UserName);
            var dbAdvisors = EntitiesHelper.GetAdvisors(UserName);
            var advisors = new List<AdvisorModel>();
            dbAdvisors.ForEach(dbAd => advisors.Add(new AdvisorModel(dbAd.Name, dbAd.EMailAddress)));

            return new ApplicationUserModel(UserName, major, advisors, sessionID);
        }

        /// <summary>
        /// Creates a new session for UserName user
        /// </summary>
        /// <param name="UserName">UserName for user to create new session</param>
        /// <returns>SessionID of new session for UserName</returns>
        public static int CreateUserSession(string UserName)
        {
            var sessionID = EntitiesHelper.CreateNewSession(UserName);
            return sessionID;
        }

        /// <summary>
        /// Destorys the current session for UserName
        /// </summary>
        /// <param name="UserName">UserName for user to destory current session</param>
        public static void DestroySession(string UserName)
        {
            var sessionID = EntitiesHelper.GetSessionID(UserName); 
            EntitiesHelper.RemoveSession(sessionID, UserName);
        }
    }
}