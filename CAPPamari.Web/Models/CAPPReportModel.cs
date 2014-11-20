using System.Collections.Generic;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public class CappReportModel
    {
        public string Name { get; set; }
        public List<RequirementSetModel> RequirementSets { get; set; }
        public List<RequirementModel> Requirements { get; set; }

        public void CheckRequirementSetFulfillments()
        {
            foreach (RequirementSetModel reqset in RequirementSets)
            {
                reqset.IsFull = reqset.IsFulfilled();
            }
        }
    }
}