using System;
using System.Net.Mail;

namespace Common.SkillsForce.EmailSender
{
    public static class EmailSender
    {
        public static string SendEmail(string Subject, string Body, string recipientEmail)
        {
            string senderEmail = "SkillsForceAdmin@ceridian.com";
            var smtpClent = new SmtpClient("relay.ceridian.com")
            {
                Port = 25,
                EnableSsl = true,
                UseDefaultCredentials = true,
            };

            var mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };

            try
            {
                smtpClent.Send(mailMessage);
                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
