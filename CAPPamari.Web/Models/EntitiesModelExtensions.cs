using System.Linq;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public static class EntitiesModelExtensions
    {
        public static CappReportModel ToCappReportModel(this CAPPReport entitiyCappReport)
        {
            return new CappReportModel
            {
                Name = entitiyCappReport.Name,
                RequirementSets =
                    entitiyCappReport.RequirementSets.Select(reqset => reqset.ToRequirementSetModel()).ToList(),
                Requirements = entitiyCappReport.Requirements.Select(req => req.ToRequirementModel()).ToList()
            };
        }

        public static RequirementSetModel ToRequirementSetModel(this RequirementSet entityRequirementSet)
        {
            return new RequirementSetModel
            {
                CreditsNeeded = entityRequirementSet.Credits,
                DepthRequirementSetRequirement = entityRequirementSet.DepthRSR,
                Description = entityRequirementSet.Description,
                MaxPassNoCreditCredits = entityRequirementSet.PassNCCredits,
                Name = entityRequirementSet.Name,
                Requirements = entityRequirementSet.Requirements.Select(req => req.ToRequirementModel()).ToList(),
                RequirementSetRequirements =
                    entityRequirementSet.RequirementSetRequirements.Select(req => req.ToRequirementModel()).ToList(),
                AppliedCourses = entityRequirementSet.Courses.Select(course => course.ToCourseModel()).ToList()
            };
        }

        public static RequirementModel ToRequirementModel(this Requirement entityRequirement)
        {
            return new RequirementModel
            {
                CommunicationIntensive = entityRequirement.CommunicationIntensive,
                CourseFullfillments =
                    entityRequirement.CourseFulfillments.Select(cf => cf.ToCourseFulfillmentModel()).ToList(),
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