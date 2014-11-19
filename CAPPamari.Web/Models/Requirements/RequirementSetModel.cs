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

        public RequirementSetModel(List<RequirementModel> Requirements,
            List<RequirementModel> RequirementSetRequirements,
            List<CourseModel> AppliedCourses, bool DepthRequirementSetRequirement, int CreditsNeeded,
            int MaxPassNoCreditCredits, string Name, string Description)
        {
            this.Requirements = Requirements;
            this.RequirementSetRequirements = RequirementSetRequirements;
            this.AppliedCourses = AppliedCourses;
            this.DepthRequirementSetRequirement = DepthRequirementSetRequirement;
            this.CreditsNeeded = CreditsNeeded;
            this.MaxPassNoCreditCredits = MaxPassNoCreditCredits;
            this.Name = Name;
            this.Description = Description;
        }

        public List<RequirementModel> Requirements { get; set; }
        public List<RequirementModel> RequirementSetRequirements { get; set; }
        public List<CourseModel> AppliedCourses { get; set; }
        public bool DepthRequirementSetRequirement { get; set; }
        public int CreditsNeeded { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool CanApplyCourse(CourseModel Course)
        {
            if (Name == "Unapplied Courses" || Name == "Free Electives") return true;
            ApplyCourses();
            bool positiveMatch = false;
            foreach (RequirementModel req in Requirements)
            {
                if (req.Match(Course))
                {
                    if (req.Exclusion) return false;
                    positiveMatch |= !req.IsFulfilled();
                }
            }
            return positiveMatch;
        }

        private void ApplyCourses()
        {
            var workingSet = new List<Fulfillment>();
            foreach (RequirementModel req in Requirements)
            {
                workingSet.Add(new Fulfillment
                {
                    Requirement = req,
                    Courses = AppliedCourses.Where(course => req.Match(course)).ToList()
                });
            }
            while (workingSet.Count > 0)
            {
                IEnumerable<Fulfillment> positiveCounts = workingSet.Where(set => set.Courses.Count() > 0);
                if (positiveCounts.Count() == 0) return;
                int smallestListCount = positiveCounts.Min(set => set.Courses.Count());

                Fulfillment weakestLink = workingSet.First(set => set.Courses.Count() == smallestListCount);
                var courseCounts = new List<CourseCount>();
                foreach (CourseModel course in weakestLink.Courses)
                {
                    courseCounts.Add(GetCourseCount(course, workingSet));
                }
                int mostSelectiveClass = courseCounts.Min(cc => cc.Count);

                CourseModel weakestCourse = courseCounts.First(cc => cc.Count == mostSelectiveClass).Course;
                RemoveCourseFromFulfillments(weakestCourse, workingSet);
                if (weakestLink.Requirement.Apply(weakestCourse))
                {
                    workingSet.Remove(weakestLink);
                }
            }
        }

        public bool Fulfills(List<CourseModel> Courses)
        {
            if (Courses == null) return false;

            // check depth
            if (DepthRequirementSetRequirement)
            {
                if (!CheckDepthRequirement(Courses)) return false;
            }

            //check credits
            if (Courses.Where(course => course.PassNoCredit).Sum(course => course.Credits) > MaxPassNoCreditCredits)
                return false;
            if (Courses.Where(course => !course.PassNoCredit).Sum(course => course.Credits) < CreditsNeeded)
                return false;

            // check requirement set requirements
            foreach (RequirementModel req in RequirementSetRequirements)
            {
                IEnumerable<CourseModel> matchingCourses = Courses.Where(course => req.Match(course));
                matchingCourses.Select(course => req.Apply(course));
                if (!req.IsFulfilled()) return false;
            }

            // check requirements
            ApplyCourses();

            return !Requirements.Any(req => !req.IsFulfilled());
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
            IEnumerable<string> twoThousandDepts =
                Courses.Where(course => course.CourseNumber.StartsWith("2")).Select(course => course.DepartmentCode);
            return
                Courses.Any(
                    course => twoThousandDepts.Contains(course.DepartmentCode) && course.CourseNumber.StartsWith("4"));
        }

        private CourseCount GetCourseCount(CourseModel Course, List<Fulfillment> Fulfillments)
        {
            int count = 0;
            foreach (Fulfillment fulfillment in Fulfillments)
            {
                if (fulfillment.Courses.Contains(Course)) count++;
            }
            return new CourseCount
            {
                Count = count,
                Course = Course
            };
        }

        private void RemoveCourseFromFulfillments(CourseModel Course, List<Fulfillment> Fulfillments)
        {
            foreach (Fulfillment fulfillment in Fulfillments)
            {
                fulfillment.Courses.Remove(Course);
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