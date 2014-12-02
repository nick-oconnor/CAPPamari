namespace CAPPamari.Web.Models.Requirements
{
    public class CourseFulfillmentModel
    {
        public CourseFulfillmentModel(string departmentCode, string courseNumber)
        {
            DepartmentCode = departmentCode;
            CourseNumber = courseNumber;
        }

        public CourseFulfillmentModel()
        {
            DepartmentCode = "";
            CourseNumber = "";
        }

        public string DepartmentCode { get; set; }
        public string CourseNumber { get; set; }

        public bool Match(CourseModel course)
        {
            if (DepartmentCode != course.DepartmentCode) return false;
            for (var i = 0; i < 4; i++)
            {
                if (CourseNumber[i] == 'x' || course.CourseNumber[i] == 'x') continue;
                if (CourseNumber[i] != course.CourseNumber[i]) return false;
            }
            return true;
        }
    }
}