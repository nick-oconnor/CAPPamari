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
        public static bool EmailToAdvisor(string username, AdvisorModel advisor)
        {
            PdfDocument document = PrintingHelper.PrintCappReport(username);
            if (document == null) return false;
            var email = new MailMessage("do-not-reply@iecfusor.com", advisor.Email)
            {
                Subject = "CAPPamari - Please review " + username + "'s CAPP report",
                Body = "Dear " + advisor.Name + ",\n" +
                       "\n" +
                       "Please review my latest plan for fulfulling my graduation requirements.\n" +
                       "\n" +
                       "Sincerely,\n" +
                       username + "\n" +
                       "--\n" +
                       HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)
            };
            using (var pdfStream = new MemoryStream())
            {
                document.SaveToStream(pdfStream);
                pdfStream.Position = 0;
                var attachment = new Attachment(pdfStream, new ContentType(MediaTypeNames.Application.Pdf));
                attachment.ContentDisposition.FileName = "CAPP Report.pdf";
                email.Attachments.Add(attachment);
                try
                {
                    var smtpServer = new SmtpClient("localhost");
                    smtpServer.Send(email);
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