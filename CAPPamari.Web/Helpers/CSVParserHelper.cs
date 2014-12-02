using System.Collections.Generic;
using System.IO;
using CAPPamari.Web.Models;
using CsvHelper;

namespace CAPPamari.Web.Helpers
{
    public class CsvParserHelper
    {
        public static IEnumerable<CourseModel> Parse(string file)
        {
            var sr = new StringReader(file);

            var reader = new CsvReader(sr);
            //somehow we have to make it delimit the header by newline instead of whatever it's doing

            var courses = reader.GetRecords<CourseModel>();

            return courses;
        }
    }
}