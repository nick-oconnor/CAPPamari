using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using CAPPamari.Web.Models;
using Spire.Pdf;

namespace CAPPamari.Web.Helpers
{
    public static class EmailHelper
    {
        public static bool EmailToAdvisor(string UserName, AdvisorModel Advisor)
        {
            PdfDocument document = PrintingHelper.PrintCAPPReport(UserName);
            if (document == null) return false;
            Attachment attachment;
            var email = new MailMessage("do-not-reply@iecfusor.com", Advisor.EMail);
            email.Subject = "CAPPamari - Please review " + UserName + "'s CAPP report";
            email.Body = "Dear " + Advisor.Name + ",\n" +
                         "\n" +
                         "Please review my latest plan for fulfulling my graduation requirements.\n" +
                         "\n" +
                         "Sincerely,\n" +
                         UserName + "\n" +
                         "--\n" +
                         HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var pdfStream = new MemoryStream())
            {
                document.SaveToStream(pdfStream);
                pdfStream.Position = 0;
                attachment = new Attachment(pdfStream, new ContentType(MediaTypeNames.Application.Pdf));
                attachment.ContentDisposition.FileName = "CAPP Report.pdf";
                email.Attachments.Add(attachment);
                try
                {
                    var SMTPServer = new SmtpClient("localhost");
                    SMTPServer.Send(email);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
    }
}