using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class IsFulfilledRequest
    {
        public string UserName { get; set; }
        public string RequirementSetname { get; set; }
    }
}