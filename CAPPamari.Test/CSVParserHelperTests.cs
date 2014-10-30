using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAPPamari.Web.Models;
using CAPPamari.Web.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CAPPamari.Test
{
    [TestClass]
    public class CSVParserHelperTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //take a sample csv file and convert it to a string- this part works
            var reader = new StreamReader(File.OpenRead(@"testParser.csv"));
            string input = "";
            while (!reader.EndOfStream)
            {
                input = input + reader.ReadLine();
            }
            //parse it
            IEnumerable<CourseModel> courses = CSVParserHelper.parse(input);

            // and spit out the info
            foreach (CourseModel course in courses)
            {
                Debug.Print("{0} {1}, {2} credits, {3}, {4}", course.DepartmentCode, course.CourseNumber, course.Credits,
                   course.Semester, course.Grade);
            }
        }
    }
}
