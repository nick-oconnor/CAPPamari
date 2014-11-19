using System.Collections.Generic;
using CAPPamari.Web.Models;

namespace CAPPamari.Web.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        ///     Create a new user
        /// </summary>
        /// <param name="username">Username of new user.</param>
        /// <param name="password">Password for new user.</param>
        /// <param name="major">Major for new user.</param>
        /// <returns>ApplicationUserModel of new user.</returns>
        public static ApplicationUserModel CreateNewUser(string username, string password, string major)
        {
            EntitiesHelper.CreateNewUser(username, password, major);
            return GetApplicationUser(username);
        }

        /// <summary>
        ///     Get application user for Username
        /// </summary>
        /// <param name="username">Username of user to get model</param>
        /// <returns>ApplicationUserModel for Username</returns>
        public static ApplicationUserModel GetApplicationUser(string username)
        {
            int sessionId = EntitiesHelper.GetSessionId(username);
            string major = EntitiesHelper.GetMajor(username);
            List<Advisor> dbAdvisors = EntitiesHelper.GetAdvisors(username);
            var advisors = new List<AdvisorModel>();
            dbAdvisors.ForEach(dbAd => advisors.Add(new AdvisorModel
            {
                Name = dbAd.Name,
                Email = dbAd.EmailAddress
            }));

            return new ApplicationUserModel(username, major, advisors, sessionId);
        }

        /// <summary>
        ///     Creates a new session for Username user
        /// </summary>
        /// <param name="username">Username for user to create new session</param>
        /// <returns>SessionID of new session for Username</returns>
        public static int CreateUserSession(string username)
        {
            int sessionId = EntitiesHelper.CreateNewSession(username);
            return sessionId;
        }

        /// <summary>
        ///     Destorys the current session for Username
        /// </summary>
        /// <param name="username">Username for user to destory current session</param>
        public static void DestroySession(string username)
        {
            int sessionId = EntitiesHelper.GetSessionId(username);
            EntitiesHelper.RemoveSession(sessionId, username);
        }

        /// <summary>
        ///     Update user if there is a session active
        /// </summary>
        /// <param name="username">Username for user to update</param>
        /// <param name="password">Password to update to</param>
        /// <param name="major">Major to update to</param>
        /// <returns>True if the user was updated, false otherwise</returns>
        public static bool UpdateUser(string username, string password, string major)
        {
            return EntitiesHelper.GetSessionId(username) != -1 && EntitiesHelper.UpdateUser(username, password, major);
        }

        /// <summary>
        ///     Add an advisor to a user
        /// </summary>
        /// <param name="username">Username of user to add an advisor to</param>
        /// <param name="newAdvisor">AdvisorModel of advisor to add to the user</param>
        /// <returns>Success status of advisor add</returns>
        public static bool AddAdvisor(string username, AdvisorModel newAdvisor)
        {
            int advisorId = EntitiesHelper.GetAdvisorId(newAdvisor.Name, newAdvisor.Email);
            if (advisorId == -1)
            {
                advisorId = EntitiesHelper.AddAdvisor(newAdvisor.Name, newAdvisor.Email);
            }
            return EntitiesHelper.AssociateAdvisorAndUser(username, advisorId);
        }

        /// <summary>
        ///     Update an advisor for a user
        /// </summary>
        /// <param name="advisor">AdvisorModel containing data to update into the advisor</param>
        /// <returns>Success status of the update</returns>
        public static bool UpdateAdvisor(AdvisorModel advisor)
        {
            return EntitiesHelper.UpdateAdvisor(advisor.Name, advisor.Email);
        }

        /// <summary>
        ///     Remove an advisor from the user
        /// </summary>
        /// <param name="username">Username of the user to remove the advisor from</param>
        /// <param name="oldAdvisor">AdvisorModel of advisor to remove from the user</param>
        /// <returns>Success status of the addvisor remove</returns>
        public static bool RemoveAdvisor(string username, AdvisorModel oldAdvisor)
        {
            int advisorId = EntitiesHelper.GetAdvisorId(oldAdvisor.Name, oldAdvisor.Email);
            return advisorId != -1 && EntitiesHelper.DisassociateAdvisorAndUser(username, advisorId);
        }

        /// <summary>
        ///     Gets user from session cookie
        /// </summary>
        /// <param name="userCookie">string stored in Javascript to store the user's state</param>
        /// <returns>ApplicationUserModel extracted from the cookie or null if the session is old or nonexistent</returns>
        public static ApplicationUserModel GetUserFromCookie(string userCookie)
        {
            string[] fields = userCookie.Split('#');
            if (fields.Length != 2) return null;

            int sessionId;
            if (!int.TryParse(fields[0], out sessionId)) return null;
            return EntitiesHelper.GetSessionId(fields[1]) != sessionId ? null : GetApplicationUser(fields[1]);
        }
    }
}