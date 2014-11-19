using System;

namespace CAPPamari.Web.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        ///     Validates a user.
        /// </summary>
        /// <param name="UserName">UserName of the user to validate</param>
        /// <param name="Password">Raw password for user to validate</param>
        /// <param name="SessionID">SessionID of current user if revalidating</param>
        /// <returns>True if credentials are valid, false otherwise.</returns>
        public static ValidationStatus Validate(string UserName, string Password = null, int SessionID = -1)
        {
            if (Password == null && SessionID == -1) return ValidationStatus.BadInput;

            if (Password != null)
            {
                string password = EntitiesHelper.GetPassword(UserName);
                if (string.IsNullOrEmpty(password)) return ValidationStatus.NoSuchUserName;
                if (password == Password) return ValidationStatus.Validated;
                return ValidationStatus.IncorrectPassword;
            }
            if (SessionID > -1)
            {
                int sessionID = EntitiesHelper.GetSessionID(UserName);
                if (sessionID != SessionID) return ValidationStatus.InvalidSession;
                if (DateTime.Now < EntitiesHelper.GetSessionExpiration(sessionID)) return ValidationStatus.Validated;
                return ValidationStatus.InvalidSession;
            }
            return ValidationStatus.BadInput;
        }
    }

    public enum ValidationStatus
    {
        BadInput,
        NoSuchUserName,
        IncorrectPassword,
        InvalidSession,
        Validated
    }
}