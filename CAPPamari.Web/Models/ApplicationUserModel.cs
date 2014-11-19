using System.Collections.Generic;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class ApplicationUserModel
    {
        [JsonConstructor]
        public ApplicationUserModel(string username, string major, List<AdvisorModel> advisors, int sessionId)
        {
            Username = username;
            Major = major;
            Advisors = advisors;
            SessionId = sessionId;
        }

        public int SessionId { get; private set; }
        public string Username { get; private set; }
        public string Major { get; private set; }
        public List<AdvisorModel> Advisors { get; private set; }

        public static ApplicationUserModel InvalidUser()
        {
            return new ApplicationUserModel(string.Empty, string.Empty, new List<AdvisorModel>(), -1);
        }
    }
}