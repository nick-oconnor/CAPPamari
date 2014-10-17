using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CAPPamari.Web.Filters;
using CAPPamari.Web.Models;
using CAPPamari.Web.Helpers;

namespace CAPPamari.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ApplicationUserModel Login(string UserName, string Password)
        {
            if (ValidationHelper.Validate(UserName, Password))
            {
                return UserHelper.GetApplicationUser(UserName);
            }
            else
            {
                return ApplicationUserModel.InvalidUser();
            }
        }
    }
}
