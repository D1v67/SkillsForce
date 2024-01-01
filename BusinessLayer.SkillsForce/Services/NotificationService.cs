using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.EmailSender;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class NotificationService : INotificationService
    {
        public async Task<string> SendNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            try
            {
                string result = enrollment.EnrollmentStatus;
                string htmlBody =  GenerateHtmlBody(enrollment, notificationType);
                string subject = $"Training Request - {result}";
                string success = await EmailSender.SendEmailAsync(subject, htmlBody, enrollment.AppUserEmail);
                return success;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string GenerateHtmlBody(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
                    string actionVerb = GetActionVerb(notificationType);
                    string htmlBody;

                    if (notificationType == NotificationType.Confirmation)
                    {
                        htmlBody = $@"
                <html>
                <head>
                    <title>HTML Email</title>
                </head>
                <body>
                    <p>Hello <strong>{enrollment.AppUserFirstName}</strong>.</p>
                    <p>Congratulations! You have been confirmed for the training <strong>{enrollment.TrainingName}</strong>.</p>
                    <br/>
                    <p>Please check your email for further details.</p>
                </body>
                </html>";
                    }
                    else
                    {
                        htmlBody = $@"
                <html>
                <head>
                    <title>HTML Email</title>
                </head>
                <body>
                    <p>Hello <strong>{enrollment.AppUserFirstName}</strong>.</p>
                    <p>Your training <strong>{enrollment.TrainingName}</strong> has been {actionVerb} by your
                        manager <strong>{enrollment.ManagerFirstName}</strong>.</p>";

                        if (notificationType == NotificationType.Rejection && !string.IsNullOrEmpty(enrollment.DeclineReason))
                        {
                            htmlBody += $"<p>Decline Reason: <strong>{enrollment.DeclineReason}</strong></p>";
                        }

                        htmlBody += @"
                    <br/>
                    <p>Please liaise with your manager for further information.</p>
                </body>
                </html>";
                    }

                    return htmlBody;
        }


        private string GetActionVerb(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Approval:
                    return "approved";
                case NotificationType.Rejection:
                    return "rejected";
                case NotificationType.Confirmation:
                    return "confirmed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null);
            }
        }

    }
}


