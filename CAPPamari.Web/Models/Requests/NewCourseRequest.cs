namespace CAPPamari.Web.Models.Requests
{
    public class NewCourseRequest
    {
        public string UserName { get; set; }
        public CourseModel NewCourse { get; set; }
    }
}