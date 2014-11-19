namespace CAPPamari.Web.Models.Requests
{
    public class RemoveCourseRequest
    {
        public string UserName { get; set; }
        public CourseModel CourseToRemove { get; set; }
    }
}