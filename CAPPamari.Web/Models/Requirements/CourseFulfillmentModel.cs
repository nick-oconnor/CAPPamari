namespace CAPPamari.Web.Models.Requirements
{
    public class CourseFulfillmentModel
    {
        public CourseFulfillmentModel(string DepartmentCode, string CourseNumber)
        {
            this.DepartmentCode = DepartmentCode;
            this.CourseNumber = CourseNumber;
        }

        public CourseFulfillmentModel()
        {
            DepartmentCode = "";
            CourseNumber = "";
        }

        public string DepartmentCode { get; set; }
        public string CourseNumber { get; set; }

        public bool Match(CourseModel Course)
        {
            if (DepartmentCode != Course.DepartmentCode) return false;
            for (int i = 0; i < 4; i++)
            {
                if (CourseNumber[i] == 'x' || Course.CourseNumber[i] == 'x') continue;
                if (CourseNumber[i] != Course.CourseNumber[i]) return false;
            }
            return true;
        }
    }
}