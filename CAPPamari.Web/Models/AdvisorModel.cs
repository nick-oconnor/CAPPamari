using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class AdvisorModel
    {
        public string Name { get; private set; }
        public string EMail { get; private set; }

        [JsonConstructor]
        public AdvisorModel(string Name, string EMail)
        {
            this.Name = Name;
            this.EMail = EMail;
        }
    }
}