using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using Spire.Pdf;
using System.IO;
using CAPPamari.Web.Models;

namespace CAPPamari.Web.Helpers
{
    public static class EmailHelper
    {
        public static bool EmailToAdvisor(string UserName, AdvisorModel Advisor)
        {
            var document = PrintingHelper.PrintCAPPReport(UserName);
            Attachment attachment;
            MailMessage email = new MailMessage("do-not-reply@iecfusor.com", Advisor.EMail);
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
                    SmtpClient SMTPServer = new SmtpClient("localhost");
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