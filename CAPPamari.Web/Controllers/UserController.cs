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
        public ApiResponse<bool> ChangeMajor([FromBody]ChangeMajorRequest Request)
        {
            var success = UserHelper.ChangeMajor(Request.UserName, Request.NewMajor);
            var message = success ? "Major changed successfully" : "Could not change major"; 
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Add an advisor for a user
        /// </summary>
        /// <param name="Request">ChangeAdvisorRequest coresponding to the user to add the advisor to.</param>
        /// <returns>ApiResponse<bool> denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> AddAdvisor([FromBody]ChangeAdvisorRequest Request)
        {
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
            var success = UserHelper.RemoveAdvisor(Request.UserName, Request.NewAdvisor);
            var message = success ? "Advisor removed successfully" : "Advisor could not be removed"; 
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
            var cappReport = CourseHelper.GetCAPPReport(UserName);
            var message = cappReport == null ? "CAPP Report not found for user " + UserName : "CAPP Report loaded successfully";
            return ApiResponse<CAPPReportModel>.From(cappReport != null, message, cappReport);
        }
    }
}
