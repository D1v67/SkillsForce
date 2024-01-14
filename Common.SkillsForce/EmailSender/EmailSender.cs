using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.SkillsForce.EmailSender
{
    public static class EmailSender
    {
        private const string SENDER_MAIL = "SkillsForceAdmin@ceridian.com";
        private const string SMTP_SERVER = "relay.ceridian.com";
        private const int SMTP_PORT = 25;

        public static async Task<string> SendEmailAsync(string subject, string body, string recipientEmail)
        {        
            var smtpClient = CreateSmtpClient();
            var mailMessage = CreateMailMessage(subject, body, recipientEmail);

            try
            {
                #pragma warning disable CS4014 
                Task.Run(()=> { smtpClient.Send(mailMessage); }).ConfigureAwait(false);
                #pragma warning restore CS4014 

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(SMTP_SERVER)
            {
                Port = SMTP_PORT,
                EnableSsl = true,
                UseDefaultCredentials = true
            };
        }

        private static MailMessage CreateMailMessage(string subject, string body, string recipientEmail)
        {
            return new MailMessage(SENDER_MAIL, recipientEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
        }
    }
}
