using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CAPPamari.Web.Helpers;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAPPamari.Test
{
    [TestClass]
    public class AutopopulationTest
    {
        [TestMethod]
        public void autopopTest1()
        {
            //make a mock requirement set
            var reqSets = new List<RequirementSetModel>();
            var csciSet = new RequirementSetModel();
            var departmentCodes = new List<String>();
            departmentCodes.Add("CSCI");

            var CSCI4440 = new RequirementModel();
            CSCI4440.CourseFullfillments.Add(new CourseFulfillmentModel("CSCI", "4440"));
            var CSCI1XXX = new RequirementModel();
            CSCI1XXX.CourseFullfillments.Add(new CourseFulfillmentModel("CSCI", "1xxx"));

            csciSet.Requirements.Add(CSCI4440);
            csciSet.Requirements.Add(CSCI1XXX);

            reqSets.Add(csciSet);

            //fill out courses
            var reader = new StreamReader(File.OpenRead(@"testParser.csv"));
            string input = "";
            while (!reader.EndOfStream)
            {
                input = input + reader.ReadLine() + "\n";
            }
            reader.Close();

            List<CourseModel> courses = CsvParserHelper.Parse(input).ToList();

            AutopopulationHelper.AutoPopulate(reqSets, courses);

            //check results
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(0).CourseNumber == "4440");
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(1).CourseNumber == "1100");
            Assert.IsTrue(courses.Count == 3);
        }
    }
}