namespace CAPPamari.Web.Models.Requests
{
    public class NewCourseRequest
    {
        public string Username { get; set; }
        public CourseModel NewCourse { get; set; }
    }
}