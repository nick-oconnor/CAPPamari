using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class ChangeMajorRequest
    {
        #region Properties
        public string UserName { get; set; }
        public string NewMajor { get; set; } 
        #endregion
    }
}