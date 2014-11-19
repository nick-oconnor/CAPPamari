namespace CAPPamari.Web.Models.Requests
{
    public class EmailToAdvisorRequest
    {
        #region Properties

        public string UserName { get; set; }
        public AdvisorModel Advisor { get; set; }

        #endregion
    }
}