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
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="UserName">UserName of new user.</param>
        /// <param name="Password">Password for new user.</param>
        /// <param name="Major">Major for new user.</param>
        /// <returns>ApplicationUserModel of new user.</returns>
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
            dbAdvisors.ForEach(dbAd => advisors.Add(new AdvisorModel() {
                Name = dbAd.Name,
                EMail = dbAd.EMailAddress
            }));

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

        /// <summary>
        /// Change major for a user
        /// </summary>
        /// <param name="UserName">UserName of user to change major of.</param>
        /// <param name="NewMajor">Major to change to.</param>
        /// <returns>Success status of change.</returns>
        public static bool ChangeMajor(string UserName, string NewMajor)
        {
            return EntitiesHelper.ChangeMajor(UserName, NewMajor);
        }

        /// <summary>
        /// Add an advisor to a user
        /// </summary>
        /// <param name="UserName">UserName of user to add an advisor to</param>
        /// <param name="NewAdvisor">AdvisorModel of advisor to add to the user</param>
        /// <returns>Success status of advisor add</returns>
        public static bool AddAdvisor(string UserName, AdvisorModel NewAdvisor)
        {
            var advisorID = EntitiesHelper.GetAdvisorID(NewAdvisor.Name, NewAdvisor.EMail);
            if (advisorID == -1)
            {
                advisorID = EntitiesHelper.AddAdvisor(NewAdvisor.Name, NewAdvisor.EMail);
            }
            return EntitiesHelper.AssociateAdvisorAndUser(UserName, advisorID);
        }

        /// <summary>
        /// Remove an advisor from the user
        /// </summary>
        /// <param name="UserName">UserName of the user to remove the advisor from</param>
        /// <param name="OldAdvisor">AdvisorModel of advisor to remove from the user</param>
        /// <returns>Success status of the addvisor remove</returns>
        public static bool RemoveAdvisor(string UserName, AdvisorModel OldAdvisor)
        {
            var advisorID = EntitiesHelper.GetAdvisorID(OldAdvisor.Name, OldAdvisor.EMail);
            if (advisorID == -1) return false;
            return EntitiesHelper.DisassociateAdvisorAndUser(UserName, advisorID);
        }
    }
}