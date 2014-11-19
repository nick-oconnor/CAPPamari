using System.Collections.Generic;
using System.Linq;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementModel
    {
        public RequirementModel()
        {
            CourseFullfillments = new List<CourseFulfillmentModel>();
            CreditsNeeded = 0;
            CreditsApplied = 0;
            MaxPassNoCreditCredits = 0;
            PassNoCreditsApplied = 0;
            CommunicationIntensive = false;
            Exclusion = false;
        }

        public RequirementModel(List<CourseFulfillmentModel> courseFullfillments, int creditsNeeded, int creditsApplied,
            int maxPassNoCreditCredits, int passNoCreditsApplied, bool communicationIntensive, bool exclusion)
        {
            CourseFullfillments = courseFullfillments;
            CreditsNeeded = creditsNeeded;
            CreditsApplied = creditsApplied;
            MaxPassNoCreditCredits = maxPassNoCreditCredits;
            PassNoCreditsApplied = passNoCreditsApplied;
            CommunicationIntensive = communicationIntensive;
            Exclusion = exclusion;
        }

        public List<CourseFulfillmentModel> CourseFullfillments { get; set; }
        public int CreditsNeeded { get; set; }
        private int CreditsApplied { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        private int PassNoCreditsApplied { get; set; }
        public bool CommunicationIntensive { get; set; }
        public bool Exclusion { get; set; }

        public bool Match(CourseModel course)
        {
            if (CommunicationIntensive && !course.CommIntensive) return false;
            return CourseFullfillments.Any(fulfillment => fulfillment.Match(course));
        }

        public bool Apply(CourseModel course)
        {
            if (Match(course))
            {
                if (Exclusion) return false;
                if (course.PassNoCredit)
                {
                    if (PassNoCreditsApplied + course.Credits <= MaxPassNoCreditCredits)
                    {
                        PassNoCreditsApplied += course.Credits;
                    }
                }
                else
                {
                    CreditsApplied += course.Credits;
                }
            }
            return IsFulfilled();
        }

        public bool IsFulfilled()
        {
            return CreditsApplied >= CreditsNeeded &&
                   PassNoCreditsApplied <= MaxPassNoCreditCredits;
        }
    }
}