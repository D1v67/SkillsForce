using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.EmailSender;
using Common.SkillsForce.ViewModel;
using System;

namespace BusinessLayer.SkillsForce.Services
{

    public class NotificationService : INotificationService
    {
        public string SendApprovalNotification(EnrollmentNotificationViewModel enrollment)
        {
            try
            {
                string result = enrollment.EnrollmentStatus;
                string htmlBody = GenerateHtmlBody(enrollment);
                string subject = $"Training Request - {result}";
                string success = EmailSender.SendEmail(subject, htmlBody, enrollment.AppUserEmail);
                return success;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string SendConfirmationNotification(EnrollmentNotificationViewModel enrollment)
        {
            throw new NotImplementedException();
        }

        public string SendEnrollNotification(EnrollmentNotificationViewModel enrollment)
        {
            throw new NotImplementedException();
        }

        public string SendRejectionNotification(EnrollmentNotificationViewModel enrollment)
        {
            try
            {
                string result = enrollment.EnrollmentStatus;
                string htmlBody = GenerateHtmlBody(enrollment, includeDeclineReason: true);
                string subject = $"Training Request - {result}";
                string success = EmailSender.SendEmail(subject, htmlBody, enrollment.AppUserEmail);
                return success;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string GenerateHtmlBody(EnrollmentNotificationViewModel enrollment, bool includeDeclineReason = false)
        {
            string htmlBody = $@"
        <html>
        <head>
            <title>HTML Email</title>
        </head>
        <body>
            <p>Hello <strong>{enrollment.AppUserFirstName}</strong>.</p>
            <p>Your training <strong>{enrollment.TrainingName}</strong> has been <strong>{enrollment.EnrollmentStatus}</strong> by your
                manager <strong>{enrollment.ManagerFirstName}</strong>.</p>";

            if (includeDeclineReason && !string.IsNullOrEmpty(enrollment.DeclineReason))
            {
                htmlBody += $"<p>Decline Reason: <strong>{enrollment.DeclineReason}</strong></p>";
            }

            htmlBody += @"
            <br/>
            <p>Please liaise with your manager for further information.</p>
        </body>
        </html>";

            return htmlBody;
        }
    }
}


