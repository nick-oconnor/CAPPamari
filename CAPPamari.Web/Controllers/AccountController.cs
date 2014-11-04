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
        public JsonResult Login([System.Web.Http.FromBody]LoginRequest Request)
        {
            var validationStatus = ValidationHelper.Validate(Request.UserName, Request.Password);
            var badUser = ApplicationUserModel.InvalidUser();
            switch (validationStatus)
            {
                case ValidationStatus.BadInput:
                    return Json(ApiResponse<ApplicationUserModel>.FailureResponse("Bad input", badUser));
                case ValidationStatus.IncorrectPassword:
                    return Json(ApiResponse<ApplicationUserModel>.FailureResponse("Incorrect password", badUser));
                case ValidationStatus.InvalidSession:
                    return Json(ApiResponse<ApplicationUserModel>.FailureResponse("Invalid session", badUser));
                case ValidationStatus.NoSuchUserName:
                    return Json(ApiResponse<ApplicationUserModel>.FailureResponse("No such user name exists", badUser));
                case ValidationStatus.Validated:
                    UserHelper.CreateUserSession(Request.UserName);
                    var user = UserHelper.GetApplicationUser(Request.UserName);
                    return Json(ApiResponse<ApplicationUserModel>.SuccessResponse("Logged in successfully", user));
            }

            return Json(ApiResponse<ApplicationUserModel>.FailureResponse("Unknown sing in failure", badUser));
        }

        [HttpPost]
        public void Logout([System.Web.Http.FromBody]string UserName)
        {
            UserHelper.DestroySession(UserName);
        }

        [HttpPost]
        public JsonResult Register(RegistrationRequest Request)
        {
            UserHelper.CreateNewUser(Request.UserName, Request.Password, Request.Major);
            UserHelper.CreateUserSession(Request.UserName);
            var user = UserHelper.GetApplicationUser(Request.UserName);
            return Json(ApiResponse<ApplicationUserModel>.SuccessResponse("Registered successfully", user)); 
        }

        [HttpPost]
        public JsonResult CheckUserName(string UserName)
        {
            var userNameExists = EntitiesHelper.UserNameExists(UserName);
            return Json(ApiResponse<bool>.SuccessResponse("Checked successfully",!userNameExists)); 
        }
    }
}
