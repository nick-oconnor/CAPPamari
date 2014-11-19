using System.Drawing;
using System.Linq;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace CAPPamari.Web.Helpers
{
    public static class PrintingHelper
    {
        public static PdfDocument PrintCappReport(string username)
        {
            if (EntitiesHelper.GetSessionId(username) == -1) return null;
            var userData = UserHelper.GetApplicationUser(username);
            var courseData = CourseHelper.GetCappReport(username);
            var pdf = new PdfDocument();
            var cappSection = pdf.Sections.Add();
            var page = cappSection.Pages.Add();
            var font = new PdfFont(PdfFontFamily.TimesRoman, 11);
            var format = new PdfStringFormat {LineSpacing = 20f};
            var brush = PdfBrushes.Black;
            var stringData = "\n";
            stringData += userData.Username + "\t" + userData.Major + "\n";
            stringData = userData.Advisors.Aggregate(stringData, (current, adivsor) => current + (adivsor.Name + ": " + adivsor.Email + "\n"));
            stringData += "\n\n";
            foreach (var reqSet in courseData.RequirementSets)
            {
                stringData += reqSet.Name + "\n";
                stringData = reqSet.AppliedCourses.Aggregate(stringData, (current, course) => current + (course.DepartmentCode + "-" + course.CourseNumber + "\n"));
                stringData += "\n";
            }
            // put user info at the top
            // aggragate through all of them
            //      put requirementset name in the header
            //          list all of the classes applied to it
            var textWidget = new PdfTextWidget(stringData, font, brush);
            var textLayout = new PdfTextLayout
            {
                Break = PdfLayoutBreakType.FitPage,
                Layout = PdfLayoutType.Paginate
            };
            var bounds = new RectangleF(new Point(0, 0), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);
            return pdf;
        }
    }
}