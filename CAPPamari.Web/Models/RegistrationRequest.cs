using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class RegistrationRequest
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Major { get; private set; }

        [JsonConstructor]
        private RegistrationRequest(string UserName, string Password, string Major)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Major = Major;
        }
    }
}