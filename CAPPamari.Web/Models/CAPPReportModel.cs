using System.Collections.Generic;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Models
{
    public class CAPPReportModel
    {
        public string Name { get; set; }
        public List<RequirementSetModel> RequirementSets { get; set; }
        public List<RequirementModel> Requirements { get; set; }
    }
}