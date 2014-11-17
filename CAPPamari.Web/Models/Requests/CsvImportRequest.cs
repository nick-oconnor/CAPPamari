using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requests
{
    public class CsvImportRequest
    {
        public string UserName { get; set; }
        public string CsvData { get; set; }
    }
}