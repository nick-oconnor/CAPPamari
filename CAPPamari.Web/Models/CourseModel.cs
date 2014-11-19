namespace CAPPamari.Web.Models
{
    public class CourseModel
    {
        public CourseModel()
        {
            DepartmentCode = string.Empty;
            CourseNumber = string.Empty;
            Grade = -1;
            Credits = -1;
            Semester = string.Empty;
            PassNoCredit = false;
            CommIntensive = false;
            RequirementSetName = "Unapplied Courses";
        }

        public string DepartmentCode { get; set; }
        public string CourseNumber { get; set; }
        public double Grade { get; set; }
        public int Credits { get; set; }
        public string Semester { get; set; }
        public bool PassNoCredit { get; set; }
        public bool CommIntensive { get; set; }
        public string RequirementSetName { get; set; }
    }
}