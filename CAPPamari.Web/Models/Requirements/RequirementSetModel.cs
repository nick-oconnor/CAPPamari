using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementSetModel
    {
        public List<RequirementModel> Requirements { get; set; }
        public List<RequirementModel> RequirementSetRequirements { get; set; }
        public List<CourseModel> AppliedCourses { get; set; }
        public bool DepthRequirementSetRequirement { get; set; }
        public int CreditsNeeded { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public RequirementSetModel()
        {
            this.Requirements = new List<RequirementModel>();
            this.RequirementSetRequirements = new List<RequirementModel>();
            this.AppliedCourses = new List<CourseModel>();
            this.DepthRequirementSetRequirement = false;
            this.CreditsNeeded = 0;
            this.MaxPassNoCreditCredits = 0;
            this.Name = "Default";
            this.Description = "Default";
        }

        public RequirementSetModel( List<RequirementModel> Requirements, List<RequirementModel> RequirementSetRequirements,
            List<CourseModel> AppliedCourses, bool DepthRequirementSetRequirement, int CreditsNeeded,
            int MaxPassNoCreditCredits, string Name, string Description){
            this.Requirements = Requirements;
            this.RequirementSetRequirements = RequirementSetRequirements;
            this.AppliedCourses = AppliedCourses;
            this.DepthRequirementSetRequirement = DepthRequirementSetRequirement;
            this.CreditsNeeded = CreditsNeeded;
            this.MaxPassNoCreditCredits = MaxPassNoCreditCredits;
            this.Name = Name;
            this.Description = Description;
        }

        public bool CanApplyCourse(CourseModel Course)
        {
            foreach (var req in Requirements)
            {
                if (req.Match(Course)) return true;
            }
            return false;
        }
        public bool Fulfills(List<CourseModel> Courses)
        {
            if (Courses == null) return false;

            // check depth
            if (DepthRequirementSetRequirement)
            {
                if(!CheckDepthRequirement(Courses)) return false;
            }

            //check credits
            if (Courses.Where(course => course.PassNoCredit).Sum(course => course.Credits) > MaxPassNoCreditCredits) return false;
            if (Courses.Where(course => !course.PassNoCredit).Sum(course => course.Credits) < CreditsNeeded) return false;

            // check requirement set requirements
            foreach (var req in RequirementSetRequirements)
            {
                var matchingCourses = Courses.Where(course => req.Match(course));
                matchingCourses.Select(course => req.Apply(course));
                if (!req.IsFulfilled()) return false;
            }

            // check requirements
            var workingSet = new List<Fulfillment>();
            foreach (var req in Requirements)
            {
                workingSet.Add(new Fulfillment()
                {
                    Requirement = req,
                    Courses = Courses.Where(course => req.Match(course))
                });
            }
            while (workingSet.Count > 0)
            {
                var smallestListCount = workingSet.Min(set => set.Courses.Count());
                if (smallestListCount == 0) return false;

                var weakestLink = workingSet.First(set => set.Courses.Count() == smallestListCount);
                var courseCounts = new List<CourseCount>();
                foreach (var course in weakestLink.Courses)
                {
                    courseCounts.Add(GetCourseCount(course, workingSet));
                }
                var mostSelectiveClass = courseCounts.Min(cc => cc.Count);

                var weakestCourse = courseCounts.First(cc => cc.Count == mostSelectiveClass).Course;
                RemoveCourseFromFulfillments(weakestCourse, workingSet);
                if (weakestLink.Requirement.Apply(weakestCourse))
                {
                    workingSet.Remove(weakestLink);
                }
            }

            return true;
        }
        public bool IsFulfilled()
        {
            return Fulfills(AppliedCourses);
        }
        public void ApplyCourse(CourseModel NewCourse)
        {
            if (AppliedCourses == null)
            {
                AppliedCourses = new List<CourseModel>();
            }
            AppliedCourses.Add(NewCourse);
        }
        private bool CheckDepthRequirement(List<CourseModel> Courses)
        {
            var twoThousandDepts = Courses.Where(course => course.CourseNumber.StartsWith("2")).Select(course => course.DepartmentCode);
            return Courses.Any(course => twoThousandDepts.Contains(course.DepartmentCode) && course.CourseNumber.StartsWith("4")); 
        }
        private CourseCount GetCourseCount(CourseModel Course, List<Fulfillment> Fulfillments)
        {
            var count = 0;
            foreach (var fulfillment in Fulfillments)
            {
                if(fulfillment.Courses.Contains(Course)) count++;
            }
            return new CourseCount()
            {
                Count = count, 
                Course = Course
            };
        }
        private void RemoveCourseFromFulfillments(CourseModel Course, List<Fulfillment> Fulfillments)
        {
            foreach (var fulfillment in Fulfillments)
            {
                fulfillment.Courses.ToList().Remove(Course);
            }
        }

        internal class Fulfillment
        {
            public RequirementModel Requirement { get; set; }
            public IEnumerable<CourseModel> Courses { get; set; } 
        }
        internal class CourseCount
        {
            public CourseModel Course { get; set; }
            public int Count { get; set; }
        }
    }
}