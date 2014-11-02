using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            // add course to the database
            // return success state
            return ApiResponse<bool>.FailureResponse("Not yet configured");
        }

        /// <summary>
        /// Moves a course for the user to the new requirement set
        /// </summary>
        /// <param name="Request">The MoveCourseRequest object denoting which course to move to which RequirementSet</param>
        /// <returns>ApiResponse<bool> telling the client whether or not the operation is valid</returns>
        [HttpPost]
        public ApiResponse<bool> MoveCourse([FromBody]MoveCourseRequest Request)
        {
            // find course for user,
            // find move course to new requirement set
            // return success state
            return ApiResponse<bool>.FailureResponse("Not yet configured");
        }

        /// <summary>
        /// Removes a course for a user
        /// </summary>
        /// <param name="Request">The RemoveCourseRequest object denoting which course to remove</param>
        /// <returns>ApiResponse<bool> telling the client whether or not the course was removed</returns>
        [HttpPost]
        public ApiResponse<bool> RemoveCourse([FromBody]RemoveCourseRequest Request)
        {
            // remove course from database
            // return success state
            return ApiResponse<bool>.FailureResponse("Not yet configured");
        }
    }
}
