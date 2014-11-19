using System.Collections.Generic;
using System.Linq;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public static class EntitiesModelExtensions
    {
        public static CappReportModel ToCappReportModel(this CAPPReport entitiyCappReport)
        {
            List<RequirementSetModel> reqsets = entitiyCappReport.RequirementSets.Count > 0
                ? entitiyCappReport.RequirementSets.Select(reqset => reqset.ToRequirementSetModel()).ToList()
                : new List<RequirementSetModel>();
            List<RequirementModel> reqs = entitiyCappReport.Requirements.Count > 0
                ? entitiyCappReport.Requirements.Select(req => req.ToRequirementModel()).ToList()
                : new List<RequirementModel>();
            return new CappReportModel
            {
                Name = entitiyCappReport.Name,
                RequirementSets = reqsets,
                Requirements = reqs
            };
        }

        public static RequirementSetModel ToRequirementSetModel(this RequirementSet entityRequirementSet)
        {
            List<RequirementModel> reqs = entityRequirementSet.Requirements.Count > 0
                ? entityRequirementSet.Requirements.Select(req => req.ToRequirementModel()).ToList()
                : new List<RequirementModel>();
            List<RequirementModel> reqReqs = entityRequirementSet.RequirementSetRequirements.Count > 0
                ? entityRequirementSet.RequirementSetRequirements.Select(req => req.ToRequirementModel()).ToList()
                : new List<RequirementModel>();
            List<CourseModel> courses = entityRequirementSet.Courses.Count > 0
                ? entityRequirementSet.Courses.Select(course => course.ToCourseModel()).ToList()
                : new List<CourseModel>();
            return new RequirementSetModel
            {
                CreditsNeeded = entityRequirementSet.Credits,
                DepthRequirementSetRequirement = entityRequirementSet.DepthRSR,
                Description = entityRequirementSet.Description,
                MaxPassNoCreditCredits = entityRequirementSet.PassNCCredits,
                Name = entityRequirementSet.Name,
                Requirements = reqs,
                RequirementSetRequirements = reqReqs,
                AppliedCourses = courses
            };
        }

        public static RequirementModel ToRequirementModel(this Requirement entityRequirement)
        {
            List<CourseFulfillmentModel> fulfillments = entityRequirement.CourseFulfillments.Count > 0
                ? entityRequirement.CourseFulfillments.Select(cf => cf.ToCourseFulfillmentModel()).ToList()
                : new List<CourseFulfillmentModel>();
            return new RequirementModel
            {
                CommunicationIntensive = entityRequirement.CommunicationIntensive,
                CourseFullfillments = fulfillments,
                CreditsNeeded = entityRequirement.CreditsNeeded,
                Exclusion = entityRequirement.Exclusion,
                MaxPassNoCreditCredits = entityRequirement.PassNoCreditCreditsAllowed
            };
        }

        public static CourseFulfillmentModel ToCourseFulfillmentModel(this CourseFulfillment entityCourseFulfillment)
        {
            return new CourseFulfillmentModel
            {
                CourseNumber = entityCourseFulfillment.CourseNumber,
                DepartmentCode = entityCourseFulfillment.DepartmentCode
            };
        }

        public static CourseModel ToCourseModel(this Course entityCourse)
        {
            return new CourseModel
            {
                CommIntensive = entityCourse.CommunicationIntensive,
                CourseNumber = entityCourse.Number,
                Credits = entityCourse.Credits,
                DepartmentCode = entityCourse.Department,
                Grade = entityCourse.Grade,
                PassNoCredit = entityCourse.PassNC,
                Semester = entityCourse.Semester
            };
        }
    }
}