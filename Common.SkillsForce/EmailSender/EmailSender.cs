using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.SkillsForce.EmailSender
{
    public static class EmailSender
    {
        public static async Task<string> SendEmailAsync(string Subject, string Body, string recipientEmail)
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

                #pragma warning disable CS4014 
                Task.Run(()=> { smtpClent.SendMailAsync(mailMessage); }).ConfigureAwait(false);
                 #pragma warning restore CS4014 

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
