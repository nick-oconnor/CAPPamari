using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class UpdateUserRequest
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Major { get; set; }
        #endregion
    }
}