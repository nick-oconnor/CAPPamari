using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CAPPamari.Web.Models.Requirements;
using CAPPamari.Web.Helpers;
using System.IO;

namespace CAPPamari.Test
{
    [TestClass]
    public class AutopopulationTest
    {
        [TestMethod]
        public void autopopTest1()
        {
            //make a mock requirement set
            List<RequirementSet> reqSets = new List<RequirementSet>();
            RequirementSet csciSet = new RequirementSet();
            List<String> departmentCodes = new List<String>();
            departmentCodes.Add("CSCI");

            Requirement CSCI4440 = new SingleRequirement("CSCI", "4440", 4);
            Requirement CSCI1XXX = new LevelRequirement(departmentCodes, "1000", 4);

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

            List<CAPPamari.Web.Models.CourseModel> courses = CSVParserHelper.parse(input).ToList();

            CAPPamari.Web.Helpers.AutopopulationHelper.autopopulate(reqSets, courses);

            //check results
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(0).CourseNumber == "4440");
            Assert.IsTrue(csciSet.AppliedCourses.ElementAt(1).CourseNumber == "1100");
            Assert.IsTrue(courses.Count == 3);
        }
    }
}