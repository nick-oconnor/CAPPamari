namespace CAPPamari.Web.Models.Requests
{
    public class EmailToAdvisorRequest
    {
        public string Username { get; set; }
        public AdvisorModel Advisor { get; set; }
    }
}