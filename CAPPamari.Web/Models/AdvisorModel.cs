using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CAPPamari.Web.Models
{
    public class AdvisorModel
    {
        public string Name { get; set; }
        public string EMail { get; set; }

        public AdvisorModel()
        {

        }
    }
}