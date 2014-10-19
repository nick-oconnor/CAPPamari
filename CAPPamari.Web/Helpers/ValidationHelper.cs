using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates a user. 
        /// </summary>
        /// <param name="UserName">UserName of the user to validate</param>
        /// <param name="Password">Raw password for user to validate</param>
        /// <param name="SessionID">SessionID of current user if revalidating</param>
        /// <returns>True if credentials are valid, false otherwise.</returns>
        public static bool Validate(string UserName, string Password = null, int SessionID = -1)
        {
            if (Password == null && SessionID == -1) return false;

            if (Password != null)
            {
                var password = EntitiesHelper.GetPassword(UserName);
                return password == Password;
            }
            else if (SessionID > -1)
            {
                var sessionID = EntitiesHelper.GetSessionID(UserName);
                if (sessionID != SessionID) return false;
                return DateTime.Now < EntitiesHelper.GetSessionExpiration(sessionID);
            }
            else
            {
                return false;
            }
        }
    }
}