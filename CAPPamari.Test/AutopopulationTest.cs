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
    public class AutoPopulationTest
    {
        [TestMethod]
        public void AutoPopulationTest1()
        {
            //make a mock requirement set
            var reqSets = new List<RequirementSetModel>();
            var csciSet = new RequirementSetModel();

            var csci4440 = new RequirementModel();
            csci4440.CourseFulfillments.Add(new CourseFulfillmentModel("CSCI", "4440"));
            var csci1XXX = new RequirementModel();
            csci1XXX.CourseFulfillments.Add(new CourseFulfillmentModel("CSCI", "1xxx"));

            csciSet.Requirements.Add(csci4440);
            csciSet.Requirements.Add(csci1XXX);

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

            AutoPopulationHelper.AutoPopulate(reqSets, courses);

            //check results
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(0).CourseNumber == "4440");
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(1).CourseNumber == "1100");
            Assert.IsTrue(courses.Count == 3);
        }
    }
}