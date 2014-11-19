namespace CAPPamari.Web.Models.Requests
{
    public class EmailToAdvisorRequest
    {
        #region Properties

        public string Username { get; set; }
        public AdvisorModel Advisor { get; set; }

        #endregion
    }
}