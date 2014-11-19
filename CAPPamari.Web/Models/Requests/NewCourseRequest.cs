namespace CAPPamari.Web.Models.Requests
{
    public class NewCourseRequest
    {
        #region Properties

        public string Username { get; set; }
        public CourseModel NewCourse { get; set; }

        #endregion
    }
}