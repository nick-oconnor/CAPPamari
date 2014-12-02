using System;

namespace CAPPamari.Web.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        ///     Validates a user.
        /// </summary>
        /// <param name="username">Username of the user to validate</param>
        /// <param name="password">Raw password for user to validate</param>
        /// <param name="sessionId">SessionID of current user if revalidating</param>
        /// <returns>True if credentials are valid, false otherwise.</returns>
        public static ValidationStatus Validate(string username, string password = null, int sessionId = -1)
        {
            if (string.IsNullOrEmpty(username) || (password == null && sessionId == -1)) return ValidationStatus.BadInput;

            if (!EntitiesHelper.UsernameExists(username)) return ValidationStatus.NoSuchUsername;

            if (password != null)
            {
                var currentPassword = EntitiesHelper.GetPassword(username);
                if (currentPassword == password) return ValidationStatus.Validated;
                return ValidationStatus.IncorrectPassword;
            }
            if (sessionId <= -1) return ValidationStatus.BadInput;
            var currentSessionId = EntitiesHelper.GetSessionId(username);
            if (currentSessionId != sessionId) return ValidationStatus.InvalidSession;
            return DateTime.Now < EntitiesHelper.GetSessionExpiration(currentSessionId)
                ? ValidationStatus.Validated
                : ValidationStatus.InvalidSession;
        }
    }

    public enum ValidationStatus
    {
        BadInput,
        NoSuchUsername,
        IncorrectPassword,
        InvalidSession,
        Validated
    }
}