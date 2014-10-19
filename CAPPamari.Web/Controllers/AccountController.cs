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
        public ApiResponse<ApplicationUserModel> Login([System.Web.Http.FromBody]LoginRequest Request)
        {
            if (ValidationHelper.Validate(Request.UserName, Request.Password))
            {
                var user = UserHelper.GetApplicationUser(Request.UserName);
                return ApiResponse<ApplicationUserModel>.SuccessResponse("Logged in successfully", user);
            }
            else
            {
                var badUser = ApplicationUserModel.InvalidUser();
                return ApiResponse<ApplicationUserModel>.FailureResponse("Sign in failure", badUser);
            }
        }

        [HttpPost]
        public void Logout([System.Web.Http.FromBody]string UserName)
        {
            var sessionID = EntitiesHelper.GetSessionID(UserName);
            EntitiesHelper.RemoveSession(sessionID, UserName);
        }

        [HttpPost]
        public ApplicationUserModel Register(RegistrationRequest Request)
        {
            // do stuff to add this new user to the database
            return ApplicationUserModel.InvalidUser();
        }
    }
}
