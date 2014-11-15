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
            Attachment report;
            MailMessage email = new MailMessage("do-not-reply@iecfusor.com", Advisor.EMail);
            email.Subject = "CAPPamari - Please review " + UserName + "'s CAPP report";
            email.Body = Advisor.Name + ",\n\n" +
                    "Please review my latest plan for fulfulling my graduation requirements.\n\n" +
                    "Sincerely,\n" +
                    UserName + "\n\n" +
                    "Sent on " + UserName + "'s behalf by CAPPamari.";
            using (var pdfStream = new MemoryStream())
            {
                document.SaveToStream(pdfStream);
                report = new Attachment(pdfStream, new ContentType(MediaTypeNames.Application.Pdf));
            }
            report.ContentDisposition.FileName = "CAPP Report.pdf";
            email.Attachments.Add(report);
            SmtpClient SMTPServer = new SmtpClient("127.0.0.1");
            try
            {
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