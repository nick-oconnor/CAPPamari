namespace CAPPamari.Web.Models.Requests
{
    public class MoveCourseRequest
    {
        public string Username { get; set; }
        public CourseModel CourseToMove { get; set; }
    }
}