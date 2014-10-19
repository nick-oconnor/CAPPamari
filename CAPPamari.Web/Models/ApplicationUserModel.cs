using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class ApplicationUserModel
    {
        public int SessionID { get; private set; }
        public string UserName { get; private set; }
        public string Major { get; private set; }
        public List<AdvisorModel> Advisors { get; private set; }

        [JsonConstructor]
        public ApplicationUserModel(string UserName, string Major, List<AdvisorModel> Advisors, int SessionID)
        {
            this.UserName = UserName;
            this.Major = Major;
            this.Advisors = Advisors;
            this.SessionID = SessionID;
        }

        public static ApplicationUserModel InvalidUser()
        {
            return new ApplicationUserModel(string.Empty, string.Empty, new List<AdvisorModel>(), -1);
        }
    }
}