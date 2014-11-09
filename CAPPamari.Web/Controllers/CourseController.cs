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
            var reqSet = CourseHelper.GetRequirementSet(Request.UserName, Request.RequirementSetName);
            var success = reqSet.CanApplyCourse(Request.CourseToMove);
            var message = success ? "Moved course successfully" : "Could not move course"; 
            if (success)
            {
                // save changes in the database
            }
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
            var success = CourseHelper.RemoveCourse(Request.UserName, Request.CourseToRemove);
            var message = success ? "Course removed successfully" : "Could not remove course";
            return ApiResponse<bool>.SuccessResponse(message, success);
        }
    }
}
