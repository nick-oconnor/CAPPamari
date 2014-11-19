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
using CAPPamari.Web.Models.Requests;

namespace CAPPamari.Web.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Login function for logging in a new user
        /// </summary>
        /// <param name="Request">Request containing UserName and Password of user to log in</param>
        /// <returns>ApiResponse<ApplicationUserModel> correxponding to the user if sign in was successful.</returns>
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

        /// <summary>
        /// Log out function to log a user out.
        /// </summary>
        /// <param name="UserName">UserName of user to log out.</param>
        [HttpPost]
        public void Logout([System.Web.Http.FromBody]string UserName)
        {
            UserHelper.DestroySession(UserName);
        }

        /// <summary>
        /// Register function to register a new user.
        /// </summary>
        /// <param name="Request">RegistrationRequest containing pertinent user information for creation of the new user.</param>
        /// <returns>ApiResponse<ApplicationUserModel> corresponding to the newly created user and their session.</returns>
        [HttpPost]
        public JsonResult Register(RegistrationRequest Request)
        {
            UserHelper.CreateNewUser(Request.UserName, Request.Password, Request.Major);
            UserHelper.CreateUserSession(Request.UserName);
            CourseHelper.CreateNewCAPPReport(Request.UserName, SupportedMajors.CSCI);
            var user = UserHelper.GetApplicationUser(Request.UserName);
            return Json(ApiResponse<ApplicationUserModel>.SuccessResponse("Registered successfully", user)); 
        }

        /// <summary>
        /// Checks to see if a UserName is already taken by another user.
        /// </summary>
        /// <param name="UserName">UserName to check for existence of in the database.</param>
        /// <returns>ApiReponse<bool> denoting whether or not the UserName is available.</returns>
        [HttpPost]
        public JsonResult CheckUserName(string UserName)
        {
            var userNameExists = EntitiesHelper.UserNameExists(UserName);
            return Json(ApiResponse<bool>.SuccessResponse("Checked successfully",!userNameExists)); 
        }
    }
}
