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
        /// <returns>True if credentials are valid, false otherwise.</returns>
        public static bool Validate(string UserName, string Password)
        {
            var password = EntitiesHelper.GetPassword(UserName);
            return password == Password;
        }
    }
}