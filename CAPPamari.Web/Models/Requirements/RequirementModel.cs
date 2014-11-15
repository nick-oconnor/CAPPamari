using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPPamari.Web.Models.Requirements
{
    public class RequirementModel
    {
        public List<CourseFulfillmentModel> CourseFullfillments { get; set; }
        public int CreditsNeeded { get; set; }
        private int CreditsApplied { get; set; }
        public int MaxPassNoCreditCredits { get; set; }
        private int PassNoCreditsApplied { get; set; }
        public bool CommunicationIntensive { get; set; }
        public bool Exclusion { get; set; }

        public RequirementModel()
        {
            this.CourseFullfillments = new List<CourseFulfillmentModel>();
            this.CreditsNeeded = 0;
            this.CreditsApplied = 0;
            this.MaxPassNoCreditCredits = 0;
            this.PassNoCreditsApplied = 0;
            this.CommunicationIntensive = false;
            this.Exclusion = false;
        }

        public RequirementModel(List<CourseFulfillmentModel> CourseFullfillments, int CreditsNeeded, int CreditsApplied,
            int MaxPassNoCreditCredits, int PassNoCreditsApplied, bool CommunicationIntensive, bool Exclusion)
        {
            this.CourseFullfillments = CourseFullfillments;
            this.CreditsNeeded  = CreditsNeeded;
            this.CreditsApplied = CreditsApplied;
            this.MaxPassNoCreditCredits = MaxPassNoCreditCredits;
            this.PassNoCreditsApplied = PassNoCreditsApplied;
            this.CommunicationIntensive = CommunicationIntensive;
            this.Exclusion = Exclusion;
        }

        public bool Match(CourseModel Course)
        {
            if (CommunicationIntensive && !Course.CommIntensive) return false;
            return Exclusion ^ CourseFullfillments.Any(fulfillment => fulfillment.Match(Course)); 
        }
        public bool Apply(CourseModel Course)
        {
            if (Match(Course))
            {
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