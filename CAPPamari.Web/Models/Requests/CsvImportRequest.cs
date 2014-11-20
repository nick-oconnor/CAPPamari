namespace CAPPamari.Web.Models.Requests
{
    public class CsvImportRequest
    {
        public string Username { get; set; }
        public string CsvData { get; set; }
        public bool AutoPopulate { get; set; }
    }
}