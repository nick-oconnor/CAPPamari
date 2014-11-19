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

        public RequirementModel(List<CourseFulfillmentModel> CourseFullfillments, int CreditsNeeded, int CreditsApplied,
            int MaxPassNoCreditCredits, int PassNoCreditsApplied, bool CommunicationIntensive, bool Exclusion)
        {
            this.CourseFullfillments = CourseFullfillments;
            this.CreditsNeeded = CreditsNeeded;
            this.CreditsApplied = CreditsApplied;
            this.MaxPassNoCreditCredits = MaxPassNoCreditCredits;
            this.PassNoCreditsApplied = PassNoCreditsApplied;
            this.CommunicationIntensive = CommunicationIntensive;
            this.Exclusion = Exclusion;
        }

        public List<CourseFulfillmentModel> CourseFullfillments { get; set; }
        public int CreditsNeeded { get; set; }
        private int CreditsApplied { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        private int PassNoCreditsApplied { get; set; }
        public bool CommunicationIntensive { get; set; }
        public bool Exclusion { get; set; }

        public bool Match(CourseModel Course)
        {
            if (CommunicationIntensive && !Course.CommIntensive) return false;
            return CourseFullfillments.Any(fulfillment => fulfillment.Match(Course));
        }

        public bool Apply(CourseModel Course)
        {
            if (Match(Course))
            {
                if (Exclusion) return false;
                if (Course.PassNoCredit)
                {
                    if (PassNoCreditsApplied + Course.Credits <= MaxPassNoCreditCredits)
                    {
                        PassNoCreditsApplied += Course.Credits;
                    }
                }
                else
                {
                    CreditsApplied += Course.Credits;
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