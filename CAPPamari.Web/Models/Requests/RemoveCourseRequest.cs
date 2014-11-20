namespace CAPPamari.Web.Models.Requests
{
    public class RemoveCourseRequest
    {
        public string Username { get; set; }
        public CourseModel CourseToRemove { get; set; }
    }
}