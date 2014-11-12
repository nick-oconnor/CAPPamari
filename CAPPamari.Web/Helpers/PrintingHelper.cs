using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace CAPPamari.Web.Helpers
{
    public static class PrintingHelper
    {
        public static PdfDocument PrintCAPPReport(string UserName)
        {
            var userData = UserHelper.GetApplicationUser(UserName);
            var courseData = CourseHelper.GetAllRequirementSets(UserName);
            var pdf = new PdfDocument();
            var cappSection = pdf.Sections.Add();
            var page = cappSection.Pages.Add();
            var font = new PdfFont(PdfFontFamily.TimesRoman, 11);
            var format = new PdfStringFormat() { LineSpacing = 20f };
            var brush = PdfBrushes.Black;
            var stringData = "\n";
            stringData += userData.UserName + "\t" + userData.Major + "\n";
            foreach (var adivsor in userData.Advisors)
            {
                stringData += adivsor.Name + ": " + adivsor.EMail + "\n";
            }
            stringData += "\n\n";
            foreach (var reqSet in courseData)
            {
                stringData += reqSet.Name + "\n";
                foreach (var course in reqSet.AppliedCourses)
                {
                    stringData += course.DepartmentCode + "-" + course.CourseNumber + "\n";
                }
                stringData += "\n";
            }
            // put user info at the top
            // aggragate through all of them
            //      put requirementset name in the header
            //          list all of the classes applied to it
            var textWidget = new PdfTextWidget(stringData, font, brush);
            var textLayout = new PdfTextLayout()
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