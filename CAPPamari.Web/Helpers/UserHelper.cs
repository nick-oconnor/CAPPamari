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
        public static ApplicationUserModel GetApplicationUser(string UserName)
        {
            var sessionID = EntitiesHelper.GetSessionID(UserName);
            var major = EntitiesHelper.GetMajor(UserName);
            var dbAdvisors = EntitiesHelper.GetAdvisors(UserName);
            var advisors = new List<AdvisorModel>();
            dbAdvisors.ForEach(dbAd => advisors.Add(new AdvisorModel(dbAd.Name, dbAd.EMailAddress)));

            return new ApplicationUserModel(UserName, major, advisors, sessionID);
        }
    }
}