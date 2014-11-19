using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public static class EntitiesModelExtensions
    {
        public static CAPPReportModel ToCAPPReportModel(this CAPPReport EntitiyCAPPReport)
        {
            var reqsets = EntitiyCAPPReport.RequirementSets.Count > 0 ? EntitiyCAPPReport.RequirementSets.Select(reqset => reqset.ToRequirementSetModel()).ToList() : new List<RequirementSetModel>();
            var reqs = EntitiyCAPPReport.Requirements.Count > 0 ? EntitiyCAPPReport.Requirements.Select(req => req.ToRequirementModel()).ToList() : new List<RequirementModel>();
            return new CAPPReportModel()
            {
                Name = EntitiyCAPPReport.Name,
                RequirementSets = reqsets,
                Requirements = reqs 
            };
        }
        public static RequirementSetModel ToRequirementSetModel(this RequirementSet EntityRequirementSet)
        {
            var reqs = EntityRequirementSet.Requirements.Count > 0 ? EntityRequirementSet.Requirements.Select(req => req.ToRequirementModel()).ToList() : new List<RequirementModel>();
            var reqReqs = EntityRequirementSet.RequirementSetRequirements.Count > 0 ? EntityRequirementSet.RequirementSetRequirements.Select(req => req.ToRequirementModel()).ToList() : new List<RequirementModel>();
            var courses = EntityRequirementSet.Courses.Count > 0 ? EntityRequirementSet.Courses.Select(course => course.ToCourseModel()).ToList() : new List<CourseModel>();
            return new RequirementSetModel()
            {
                CreditsNeeded = EntityRequirementSet.Credits,
                DepthRequirementSetRequirement = EntityRequirementSet.DepthRSR,
                Description = EntityRequirementSet.Description,
                MaxPassNoCreditCredits = EntityRequirementSet.PassNCCredits,
                Name = EntityRequirementSet.Name,
                Requirements = reqs, 
                RequirementSetRequirements = reqReqs,
                AppliedCourses = courses 
            };
        }
        public static RequirementModel ToRequirementModel(this Requirement EntityRequirement)
        {
            var fulfillments = EntityRequirement.CourseFulfillments.Count > 0 ? EntityRequirement.CourseFulfillments.Select(cf => cf.ToCourseFulfillmentModel()).ToList() : new List<CourseFulfillmentModel>();
            return new RequirementModel()
            {
                CommunicationIntensive = EntityRequirement.CommunicationIntensive,
                CourseFullfillments = fulfillments, 
                CreditsNeeded = EntityRequirement.CreditsNeeded,
                Exclusion = EntityRequirement.Exclusion,
                MaxPassNoCreditCredits = EntityRequirement.PassNoCreditCreditsAllowed
            };
        }
        public static CourseFulfillmentModel ToCourseFulfillmentModel(this CourseFulfillment EntityCourseFulfillment)
        {
            return new CourseFulfillmentModel()
            {
                CourseNumber = EntityCourseFulfillment.CourseNumber,
                DepartmentCode = EntityCourseFulfillment.DepartmentCode
            };
        }
        public static CourseModel ToCourseModel(this Course EntityCourse)
        {
            return new CourseModel()
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