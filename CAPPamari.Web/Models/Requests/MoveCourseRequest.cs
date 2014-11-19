namespace CAPPamari.Web.Models.Requests
{
    public class MoveCourseRequest
    {
        #region Properties

        public string Username { get; set; }
        public CourseModel CourseToMove { get; set; }
        public string RequirementSetName { get; set; }

        #endregion
    }
}