using System.Collections.Generic;
using System.Linq;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementSetModel
    {
        public RequirementSetModel()
        {
            Requirements = new List<RequirementModel>();
            RequirementSetRequirements = new List<RequirementModel>();
            AppliedCourses = new List<CourseModel>();
            DepthRequirementSetRequirement = false;
            CreditsNeeded = 0;
            MaxPassNoCreditCredits = 0;
            Name = "Default";
            Description = "Default";
        }

        public RequirementSetModel(List<RequirementModel> requirements,
            List<RequirementModel> requirementSetRequirements,
            List<CourseModel> appliedCourses, bool depthRequirementSetRequirement, int creditsNeeded,
            int maxPassNoCreditCredits, string name, string description)
        {
            Requirements = requirements;
            RequirementSetRequirements = requirementSetRequirements;
            AppliedCourses = appliedCourses;
            DepthRequirementSetRequirement = depthRequirementSetRequirement;
            CreditsNeeded = creditsNeeded;
            MaxPassNoCreditCredits = maxPassNoCreditCredits;
            Name = name;
            Description = description;
        }

        public List<RequirementModel> Requirements { get; set; }
        public List<RequirementModel> RequirementSetRequirements { get; set; }
        public List<CourseModel> AppliedCourses { get; set; }
        public bool DepthRequirementSetRequirement { get; set; }
        public int CreditsNeeded { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFull { get; set; }

        public bool CanApplyCourse(CourseModel course)
        {
            if (Name == "Unapplied Courses" || Name == "Free Electives") return true;
            ApplyCourses();
            bool positiveMatch = false;
            foreach (RequirementModel req in Requirements.Where(req => req.Match(course)))
            {
                if (req.Exclusion) return false;
                positiveMatch |= !req.IsFulfilled();
            }
            return positiveMatch;
        }

        private void ApplyCourses()
        {
            List<Fulfillment> workingSet = Requirements.Select(req => new Fulfillment
            {
                Requirement = req,
                Courses = AppliedCourses.Where(req.Match).ToList()
            }).ToList();
            while (workingSet.Count > 0)
            {
                IEnumerable<Fulfillment> positiveCounts = workingSet.Where(set => set.Courses.Any());
                IList<Fulfillment> fulfillments = positiveCounts as IList<Fulfillment> ?? positiveCounts.ToList();
                if (!fulfillments.Any()) return;
                int smallestListCount = fulfillments.Min(set => set.Courses.Count());

                Fulfillment weakestLink = workingSet.First(set => set.Courses.Count() == smallestListCount);
                List<CourseCount> courseCounts =
                    weakestLink.Courses.Select(course => GetCourseCount(course, workingSet)).ToList();
                int mostSelectiveClass = courseCounts.Min(cc => cc.Count);

                CourseModel weakestCourse = courseCounts.First(cc => cc.Count == mostSelectiveClass).Course;
                DeleteCourseFromFulfillments(weakestCourse, workingSet);
                if (weakestLink.Requirement.Apply(weakestCourse))
                {
                    workingSet.Remove(weakestLink);
                }
            }
        }

        public bool Fulfills(List<CourseModel> courses)
        {
            if (courses == null) return false;

            // check depth
            if (DepthRequirementSetRequirement)
            {
                if (!CheckDepthRequirement(courses)) return false;
            }

            //check credits
            if (courses.Where(course => course.PassNoCredit).Sum(course => course.Credits) > MaxPassNoCreditCredits)
                return false;
            if (courses.Where(course => !course.PassNoCredit).Sum(course => course.Credits) < CreditsNeeded)
                return false;

            // check requirement set requirements
            foreach (RequirementModel req in RequirementSetRequirements)
            {
                RequirementModel req1 = req;
                IEnumerable<CourseModel> matchingCourses = courses.Where(req1.Match);
                RequirementModel req2 = req;
                matchingCourses.Select(req2.Apply);
                if (!req.IsFulfilled()) return false;
            }

            // check requirements
            ApplyCourses();

            return Requirements.All(req => req.IsFulfilled());
        }

        public bool IsFulfilled()
        {
            return Fulfills(AppliedCourses);
        }

        public void ApplyCourse(CourseModel newCourse)
        {
            if (AppliedCourses == null)
            {
                AppliedCourses = new List<CourseModel>();
            }
            AppliedCourses.Add(newCourse);
        }

        private static bool CheckDepthRequirement(List<CourseModel> courses)
        {
            IEnumerable<string> twoThousandDepts =
                courses.Where(course => course.CourseNumber.StartsWith("2")).Select(course => course.DepartmentCode);
            return
                courses.Any(
                    course => twoThousandDepts.Contains(course.DepartmentCode) && course.CourseNumber.StartsWith("4"));
        }

        private static CourseCount GetCourseCount(CourseModel course, IEnumerable<Fulfillment> fulfillments)
        {
            int count = fulfillments.Count(fulfillment => fulfillment.Courses.Contains(course));
            return new CourseCount
            {
                Count = count,
                Course = course
            };
        }

        private static void DeleteCourseFromFulfillments(CourseModel course, IEnumerable<Fulfillment> fulfillments)
        {
            foreach (Fulfillment fulfillment in fulfillments)
            {
                fulfillment.Courses.Remove(course);
            }
        }

        internal class CourseCount
        {
            public CourseModel Course { get; set; }
            public int Count { get; set; }
        }

        internal class Fulfillment
        {
            public RequirementModel Requirement { get; set; }
            public List<CourseModel> Courses { get; set; }
        }
    }
}