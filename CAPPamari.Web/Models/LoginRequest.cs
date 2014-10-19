using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class LoginRequest
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }

        [JsonConstructor]
        private LoginRequest(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}