using System.Web.Http;
using CAPPamari.Web.Helpers;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requests;

namespace CAPPamari.Web.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        ///     Change a users major
        /// </summary>
        /// <param name="request">ChangeMajorRequest denoting which user needs to change their major and what to change it to.</param>
        /// <returns>ApplicationUserModel denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<ApplicationUserModel> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return
                    ApiResponse<ApplicationUserModel>.FailureResponse(
                        "Your session is bad, please refresh and sign back in");
            }
            var success = UserHelper.UpdateUser(request.Username, request.Password, request.Major);
            var userData = success ? UserHelper.GetApplicationUser(request.Username) : null;
            var message = success ? "Account updated successfully" : "Could not update account";
            return ApiResponse<ApplicationUserModel>.From(success, message, userData);
        }

        /// <summary>
        ///     Add an advisor for a user
        /// </summary>
        /// <param name="request">ChangeAdvisorRequest coresponding to the user to add the advisor to.</param>
        /// <returns>Bool denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> AddAdvisor([FromBody] ChangeAdvisorRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = UserHelper.AddAdvisor(request.Username, request.Advisor);
            var message = success ? "Advisor added successfully" : "Could not add advisor";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Remove an advisor for a user
        /// </summary>
        /// <param name="request">ChangeAdvisorRequest coresponding to the user to remove the advisor from.</param>
        /// <returns>Bool denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> RemoveAdvisor([FromBody] ChangeAdvisorRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = UserHelper.RemoveAdvisor(request.Username, request.Advisor);
            var message = success ? "Advisor removed successfully" : "Advisor could not be removed";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Update an advisor for a user
        /// </summary>
        /// <param name="request">ChangeAdvisorRequest coresponding to the user to update the advisor for</param>
        /// <returns>Bool denoting whether or not the advisor was updated.</returns>
        public ApiResponse<bool> UpdateAdvisor([FromBody] AdvisorModel request)
        {
            var success = UserHelper.UpdateAdvisor(request);
            var message = success ? "Advisor updated successfully" : "Could not update advisor";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Email the current report to your advisor
        /// </summary>
        /// <param name="request">EmailToAdvisorRequest coresponding to the advisor to email the user's report to.</param>
        /// <returns>Bool denoting whether or not the action was successful.</returns>
        [HttpPost]
        public ApiResponse<bool> EmailToAdvisor([FromBody] EmailToAdvisorRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = EmailHelper.EmailToAdvisor(request.Username, request.Advisor);
            var message = success ? "Email sent successfully" : "Email could not be sent";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Load a CAPP report for a user
        /// </summary>
        /// <param name="userName">Username of user to load CAPP report for</param>
        /// <returns>CAPPReportModel containing user's CAPP report</returns>
        [HttpPost]
        public ApiResponse<CappReportModel> GetCappReport([FromBody] string userName)
        {
            if (!EntitiesHelper.UpdateSession(userName))
            {
                return
                    ApiResponse<CappReportModel>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var cappReport = CourseHelper.GetCappReport(userName);
            var message = cappReport == null
                ? "CAPP report not found for user " + userName
                : "CAPP report loaded successfully";
            if (cappReport != null) cappReport.CheckRequirementSetFulfillments();
            return ApiResponse<CappReportModel>.From(cappReport != null, message, cappReport);
        }

        /// <summary>
        ///     Loads a user from the session cookie set on the client
        /// </summary>
        /// <param name="userSessionCookie">string stored in Javascript to save user's session</param>
        /// <returns>ApplicationUserModel for user or null if the cookie is bad</returns>
        public ApiResponse<ApplicationUserModel> LoadFromUserSessionCookie([FromBody] string userSessionCookie)
        {
            var user = UserHelper.GetUserFromCookie(userSessionCookie);
            var success = user != null;
            var message = success
                ? "User loaded from the session cookie successfully"
                : "User could not be loaded from the session cookie";
            return ApiResponse<ApplicationUserModel>.From(success, message, user);
        }
    }
}