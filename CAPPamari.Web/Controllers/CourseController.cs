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
    public class CourseController : ApiController
    {
        /// <summary>
        /// Adds a new course for the user
        /// </summary>
        /// <param name="Request">The NewCourseRequest object denoting the new course attributes</param>
        /// <returns>ApiResponse<bool> telling the client whether or not the course was added</returns>
        [HttpPost]
        public ApiResponse<bool> AddNewCourse([FromBody]NewCourseRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = CourseHelper.AddNewCourse(Request.UserName, Request.NewCourse);
            var message = success ? "Course added successfully" : "Could not add course"; 
            return ApiResponse<bool>.SuccessResponse(message,success);
        }

        /// <summary>
        /// Moves a course for the user to the new requirement set
        /// </summary>
        /// <param name="Request">The MoveCourseRequest object denoting which course to move to which RequirementSet</param>
        /// <returns>ApiResponse<bool> telling the client whether or not the operation is valid</returns>
        [HttpPost]
        public ApiResponse<bool> MoveCourse([FromBody]MoveCourseRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var reqSet = CourseHelper.GetRequirementSet(Request.UserName, Request.RequirementSetName);
            var success = reqSet.CanApplyCourse(Request.CourseToMove);
            if (success)
            {
                success &= CourseHelper.ApplyCourse(Request.UserName, Request.CourseToMove, reqSet);
            }
            var message = success ? "Moved course successfully" : "You cannot apply this course to this requirement set"; 
            return ApiResponse<bool>.SuccessResponse(message, success);
        }

        /// <summary>
        /// Removes a course for a user
        /// </summary>
        /// <param name="Request">The RemoveCourseRequest object denoting which course to remove</param>
        /// <returns>ApiResponse<bool> telling the client whether or not the course was removed</returns>
        [HttpPost]
        public ApiResponse<bool> RemoveCourse([FromBody]RemoveCourseRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<bool>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var success = CourseHelper.RemoveCourse(Request.UserName, Request.CourseToRemove);
            var message = success ? "Course removed successfully" : "Could not remove course";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }
        /// <summary>
        /// Upload a csv file with many courses
        /// </summary>
        /// <param name="Request">CsvImportRequest containing course and user information</param>
        /// <returns>ApiResponse<CAPPReportModel> representing the new CAPP Report for the user</returns>
        [HttpPost]
        public ApiResponse<CAPPReportModel> AddCsvFile([FromBody]CsvImportRequest Request)
        {
            if (!EntitiesHelper.UpdateSession(Request.UserName))
            {
                return ApiResponse<CAPPReportModel>.FailureResponse("Your session is bad, please refresh and sign back in.");
            }
            var courses = CSVParserHelper.parse(Request.CsvData);
            // var cappReport = CourseHelper.GetCAPPReport(Request.UserName);
            // AutopopulationHelper.autopopulate(cappReport, courses); 
            var success = true;
            foreach (var course in courses)
            {
                var reqSetName = string.IsNullOrEmpty(course.RequirementSetName) ? "Unapplied Courses" : course.RequirementSetName;
                success &= CourseHelper.AddNewCourse(Request.UserName, course, reqSetName);
            }
            var message = success ? "All courses uploaded successfully" : "One or more courses were missed in upload";
            var cappReport = CourseHelper.GetCAPPReport(Request.UserName);
            return ApiResponse<CAPPReportModel>.From(success, message, cappReport);
        }
    }
}
