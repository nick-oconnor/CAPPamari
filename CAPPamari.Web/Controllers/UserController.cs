using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CAPPamari.Web.Helpers;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requests;

namespace CAPPamari.Web.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Change a users major
        /// </summary>
        /// <param name="Request">ChangeMajorRequest denoting which user needs to change their major and what to change it to.</param>
        /// <returns>ApiResponse<bool> denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<ApplicationUserModel> UpdateUser([FromBody]UpdateUserRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<ApplicationUserModel>.FailureResponse("Your session is bad, please refresh and sign back in");
            }
            var success = UserHelper.UpdateUser(Request.UserName, Request.Password, Request.Major);
            var userData = success ? UserHelper.GetApplicationUser(Request.UserName) : null;
            var message = success ? "Major changed successfully" : "Could not change major";
            return ApiResponse<ApplicationUserModel>.From(success, message, userData);
        }

        /// <summary>
        /// Add an advisor for a user
        /// </summary>
        /// <param name="Request">ChangeAdvisorRequest coresponding to the user to add the advisor to.</param>
        /// <returns>ApiResponse<bool> denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> AddAdvisor([FromBody]ChangeAdvisorRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = UserHelper.AddAdvisor(Request.UserName, Request.NewAdvisor);
            var message = success ? "Advisor added successfully" : "Could not add advisor"; 
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Remove an advisor for a user
        /// </summary>
        /// <param name="Request">ChangeAdvisorRequest coresponding to the user to remove the advisor from.</param>
        /// <returns>ApiResponse<bool> denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> RemoveAdvisor([FromBody]ChangeAdvisorRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = UserHelper.RemoveAdvisor(Request.UserName, Request.NewAdvisor);
            var message = success ? "Advisor removed successfully" : "Advisor could not be removed"; 
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Update an advisor for a user
        /// </summary>
        /// <param name="Request">ChangeAdvisorRequest coresponding to the user to update the advisor for</param>
        /// <returns>ApiResponse<bool> denoting whether or not the advisor was updated.</returns>
        public ApiResponse<bool> UpdateAdvisor([FromBody]AdvisorModel Request)
        {
            var success = UserHelper.UpdateAdvisor(Request);
            var message = success ? "Advisor updated successfully" : "Could not update advisor email";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Email the current report to your advisor
        /// </summary>
        /// <param name="Request">EmailToAdvisorRequest coresponding to the advisor to email the user's report to.</param>
        /// <returns>ApiResponse<bool> denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> EmailToAdvisor([FromBody]EmailToAdvisorRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = EmailHelper.EmailToAdvisor(Request.UserName, Request.Advisor);
            var message = success ? "Email sent successfully" : "Email could not be sent";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Load a CAPP Report for a user
        /// </summary>
        /// <param name="UserName">UserName of user to load CAPP Report for</param>
        /// <returns>ApiResponse<CAPPReportModel> containing user's CAPP Report</returns>
        [HttpPost]
        public ApiResponse<CAPPReportModel> GetCAPPReport([FromBody]string UserName)
        {
            if (!EntitiesHelper.UpdateSession(UserName))
            {
                return ApiResponse<CAPPReportModel>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var cappReport = CourseHelper.GetCAPPReport(UserName);
            var message = cappReport == null ? "CAPP Report not found for user " + UserName : "CAPP Report loaded successfully";
            return ApiResponse<CAPPReportModel>.From(cappReport != null, message, cappReport);
        }

        /// <summary>
        /// Loads a user from the session cookie set on the client
        /// </summary>
        /// <param name="UserSessionCookie">string stored in Javascript to save user's session</param>
        /// <returns>ApiResponse<ApplicationUserModel> for user or null if the cookie is bad</returns>
        public ApiResponse<ApplicationUserModel> LoadFromUserSessionCookie([FromBody]string UserSessionCookie)
        {
            var user = UserHelper.GetUserFromCookie(UserSessionCookie);
            var success = user != null;
            var message = success ? "User loaded successfully from the session cookie" : "User could not be loaded from the session cookie";
            return ApiResponse<ApplicationUserModel>.From(success, message, user);
        }
    }
}
