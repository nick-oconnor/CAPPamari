namespace CAPPamari.Web.Models.Requests
{
    public class CsvImportRequest
    {
        public string UserName { get; set; }
        public string CsvData { get; set; }
        public bool Autopopulate { get; set; }
    }
}