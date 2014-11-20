using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CAPPamari.Web.Helpers;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requests;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Controllers
{
    public class CourseController : ApiController
    {
        /// <summary>
        ///     Adds a new course for the user
        /// </summary>
        /// <param name="request">The NewCourseRequest object denoting the new course attributes</param>
        /// <returns>Bool telling the client whether or not the course was added</returns>
        [HttpPost]
        public ApiResponse<bool> AddNewCourse([FromBody] NewCourseRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            bool success = CourseHelper.AddNewCourse(request.Username, request.NewCourse);
            string message = success ? "Course added successfully" : "Could not add course";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Moves a course for the user to the new requirement set
        /// </summary>
        /// <param name="request">The MoveCourseRequest object denoting which course to move to which RequirementSet</param>
        /// <returns>Bool telling the client whether or not the operation is valid</returns>
        [HttpPost]
        public ApiResponse<MoveCourseResponse> MoveCourse([FromBody] MoveCourseRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<MoveCourseResponse>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            RequirementSetModel reqSet = CourseHelper.GetRequirementSet(request.Username, request.RequirementSetName);
            bool success = reqSet.CanApplyCourse(request.CourseToMove);
            if (success)
            {
                success &= CourseHelper.ApplyCourse(request.Username, request.CourseToMove, reqSet);
                reqSet.ApplyCourse(request.CourseToMove);
            }
            string message = success
                ? "Moved course successfully"
                : "You cannot apply this course to this requirement set";
            var fulfillment = reqSet.IsFulfilled();
            return ApiResponse<MoveCourseResponse>.SuccessResponse(message, new MoveCourseResponse()
                {
                    MoveSuccessful = success,
                    RequirementSetFulfilled = fulfillment
                });
        }

        /// <summary>
        ///     Removes a course for a user
        /// </summary>
        /// <param name="request">The RemoveCourseRequest object denoting which course to remove</param>
        /// <returns>Bool telling the client whether or not the course was removed</returns>
        [HttpPost]
        public ApiResponse<bool> RemoveCourse([FromBody] RemoveCourseRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            bool success = CourseHelper.RemoveCourse(request.Username, request.CourseToRemove);
            string message = success ? "Course removed successfully" : "Could not remove course";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        ///     Upload a csv file with many courses
        /// </summary>
        /// <param name="request">CsvImportRequest containing course and user information</param>
        /// <returns>CAPPReportModel representing the new CAPP Report for the user</returns>
        [HttpPost]
        public ApiResponse<CappReportModel> AddCsvFile([FromBody] CsvImportRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.Username))
            {
                return
                    ApiResponse<CappReportModel>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            IEnumerable<CourseModel> courses = CsvParserHelper.Parse(request.CsvData);
            CappReportModel cappReport;
            IList<CourseModel> courseModels = courses as IList<CourseModel> ?? courses.ToList();
            if (request.AutoPopulate)
            {
                cappReport = CourseHelper.GetCappReport(request.Username);
                AutopopulationHelper.AutoPopulate(cappReport.RequirementSets, courseModels.ToList());
            }
            bool success = true;
            foreach (CourseModel course in courseModels)
            {
                string reqSetName = string.IsNullOrEmpty(course.RequirementSetName)
                    ? "Unapplied Courses"
                    : course.RequirementSetName;
                success &= CourseHelper.AddNewCourse(request.Username, course, reqSetName);
            }
            string message = success ? "All courses uploaded successfully" : "One or more courses were missed in upload";
            cappReport = CourseHelper.GetCappReport(request.Username);
            cappReport.CheckRequirementSetFulfillments();
            return ApiResponse<CappReportModel>.From(success, message, cappReport);
        }

        /// <summary>
        ///     Find a place to put all unapplied courses
        /// </summary>
        /// <param name="username">Username of user to autopopulate for</param>
        /// <returns>CAPPReportModel reporesenting the new CAPP Report for the user</returns>
        [HttpPost]
        public ApiResponse<CappReportModel> AutoPopulateUnappliedCourses([FromBody] string username)
        {
            if (!EntitiesHelper.UpdateSession(username))
            {
                return
                    ApiResponse<CappReportModel>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            List<CourseModel> courses = CourseHelper.GetRequirementSet(username, "Unapplied Courses").AppliedCourses;
            CappReportModel cappReport = CourseHelper.GetCappReport(username);
            AutopopulationHelper.AutoPopulate(cappReport.RequirementSets, courses);
            bool success = true;
            foreach (CourseModel course in courses)
            {
                string reqSetName = string.IsNullOrEmpty(course.RequirementSetName)
                    ? "Unapplied Courses"
                    : course.RequirementSetName;
                RequirementSetModel reqSet = CourseHelper.GetRequirementSet(username, reqSetName);
                success &= CourseHelper.ApplyCourse(username, course, reqSet);
            }
            string message = success ? "All courses uploaded successfully" : "One or more courses were missed in upload";
            cappReport = CourseHelper.GetCappReport(username);
            cappReport.CheckRequirementSetFulfillments();
            return ApiResponse<CappReportModel>.From(success, message, cappReport);
        }
        /// <summary>
        /// Checks whether or not a requirement set is full for a user
        /// </summary>
        /// <param name="request">IsFulfilledRequest containing UserName of user and RequirementSetName of set to check fulfillment on</param>
        /// <returns>ApiResponse<bool> telling the user whether or not the requirement set is full</returns>
        [HttpPost]
        public ApiResponse<bool> CheckFulfillment([FromBody]IsFulfilledRequest request)
        {
            if (!EntitiesHelper.UpdateSession(request.UserName))
            {
                return
                    ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var reqset = EntitiesHelper.GetRequirementSet(request.UserName, request.RequirementSetname);
            var isFull = reqset.IsFulfilled();
            var message = "Found fulfillment successfully";
            return ApiResponse<bool>.SuccessResponse(message, isFull);
        }
    }
}