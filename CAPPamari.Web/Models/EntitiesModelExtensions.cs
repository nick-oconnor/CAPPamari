using System.Linq;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public static class EntitiesModelExtensions
    {
        public static CAPPReportModel ToCAPPReportModel(this CAPPReport EntitiyCAPPReport)
        {
            return new CAPPReportModel
            {
                Name = EntitiyCAPPReport.Name,
                RequirementSets =
                    EntitiyCAPPReport.RequirementSets.Select(reqset => reqset.ToRequirementSetModel()).ToList(),
                Requirements = EntitiyCAPPReport.Requirements.Select(req => req.ToRequirementModel()).ToList()
            };
        }

        public static RequirementSetModel ToRequirementSetModel(this RequirementSet EntityRequirementSet)
        {
            return new RequirementSetModel
            {
                CreditsNeeded = EntityRequirementSet.Credits,
                DepthRequirementSetRequirement = EntityRequirementSet.DepthRSR,
                Description = EntityRequirementSet.Description,
                MaxPassNoCreditCredits = EntityRequirementSet.PassNCCredits,
                Name = EntityRequirementSet.Name,
                Requirements = EntityRequirementSet.Requirements.Select(req => req.ToRequirementModel()).ToList(),
                RequirementSetRequirements =
                    EntityRequirementSet.RequirementSetRequirements.Select(req => req.ToRequirementModel()).ToList(),
                AppliedCourses = EntityRequirementSet.Courses.Select(course => course.ToCourseModel()).ToList()
            };
        }

        public static RequirementModel ToRequirementModel(this Requirement EntityRequirement)
        {
            return new RequirementModel
            {
                CommunicationIntensive = EntityRequirement.CommunicationIntensive,
                CourseFullfillments =
                    EntityRequirement.CourseFulfillments.Select(cf => cf.ToCourseFulfillmentModel()).ToList(),
                CreditsNeeded = EntityRequirement.CreditsNeeded,
                Exclusion = EntityRequirement.Exclusion,
                MaxPassNoCreditCredits = EntityRequirement.PassNoCreditCreditsAllowed
            };
        }

        public static CourseFulfillmentModel ToCourseFulfillmentModel(this CourseFulfillment EntityCourseFulfillment)
        {
            return new CourseFulfillmentModel
            {
                CourseNumber = EntityCourseFulfillment.CourseNumber,
                DepartmentCode = EntityCourseFulfillment.DepartmentCode
            };
        }

        public static CourseModel ToCourseModel(this Course EntityCourse)
        {
            return new CourseModel
            {
                CommIntensive = EntityCourse.CommunicationIntensive,
                CourseNumber = EntityCourse.Number,
                Credits = EntityCourse.Credits,
                DepartmentCode = EntityCourse.Department,
                Grade = EntityCourse.Grade,
                PassNoCredit = EntityCourse.PassNC,
                Semester = EntityCourse.Semester
            };
        }
    }
}