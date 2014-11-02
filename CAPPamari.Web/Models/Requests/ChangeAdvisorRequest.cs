using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class ChangeAdvisorRequest
    {
        #region Properties
        public string UserName { get; set; }
        public AdvisorModel NewAdvisor { get; set; }
        #endregion
    }
}