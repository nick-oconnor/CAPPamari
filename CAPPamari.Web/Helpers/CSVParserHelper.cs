using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CAPPamari.Web.Models;
using CsvHelper;

namespace CAPPamari.Web.Helpers
{
    public class CSVParserHelper
    {
        public static IEnumerable<CourseModel> parse(string file)
        {
            var sr = new StringReader(file);
            
            var reader = new CsvReader(sr);
            //somehow we have to make it delimit the header by newline instead of whatever it's doing
               
            IEnumerable<CourseModel> courses = reader.GetRecords<CourseModel>();

            return courses;
            
        }
    }
}