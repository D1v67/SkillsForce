using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.EmailSender;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{

        public class NotificationService : INotificationService
        {
            public string SendNotification(EnrollmentNotificationViewModel enrollment)
            {
                // Update the IsApproved to true / false based on the clicks
                string result = enrollment.EnrollmentStatus;
                // Build the message body
                string htmlBody = $@"
                                    <html>
                                    <head>
                                    <title>HTML Email</title>
                                    </head>
                                    <body>
                                    <p>Hello <strong>{enrollment.AppUserFirstName}</strong>.</p>
                                    <p>Your training <strong>{enrollment.TrainingName}</strong> has been <strong>{result}</strong> by your
                                                         manager <strong>{enrollment.ManagerFirstName}</strong>.</p>
                                    <br/>
                                    <p>Please liaise with your manager for further information.</p>
                                    </body>
                                    </html>
                                                ";
                string subject = $"Training Request - {result}";
                try
                {
                    string success = EmailSender.SendEmail(subject, htmlBody, enrollment.AppUserEmail);
                    return success;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
        }
    }


