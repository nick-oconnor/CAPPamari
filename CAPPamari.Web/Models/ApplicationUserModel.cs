using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class ApplicationUserModel
    {
        public string UserName { get; private set; }
        public string Major { get; private set; }
        public List<AdvisorModel> Advisors { get; private set; }

        [JsonConstructor]
        public ApplicationUserModel(string UserName, string Major, List<AdvisorModel> Advisors)
        {
            this.UserName = UserName;
            this.Major = Major;
            this.Advisors = Advisors;
        }

        public static ApplicationUserModel InvalidUser()
        {
            return new ApplicationUserModel(string.Empty, string.Empty, new List<AdvisorModel>());
        }
    }
}