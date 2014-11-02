using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models.Requests
{
    public class RegistrationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Major { get; set; }
    }
}