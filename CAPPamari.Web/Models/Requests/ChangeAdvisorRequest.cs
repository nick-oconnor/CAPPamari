namespace CAPPamari.Web.Models.Requests
{
    public class ChangeAdvisorRequest
    {
        public string Username { get; set; }
        public AdvisorModel Advisor { get; set; }
    }
}