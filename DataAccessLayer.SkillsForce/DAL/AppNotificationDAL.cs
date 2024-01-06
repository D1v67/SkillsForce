using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class AppNotificationDAL : IAppNotificationDAL
    {
        private readonly IDBCommand _dbCommand;

        public AppNotificationDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public async Task<int> AddNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            int relevantUserId = GetRelevantUserId(enrollment, notificationType);

            // Construct the notification message based on the notification type
            string notificationMessage = ConstructNotificationMessage(enrollment, notificationType);

            // Insert the notification into the database
            AppNotificationModel appNotification = new AppNotificationModel
            {
                UserID = relevantUserId,
                EnrollmentID = enrollment.EnrollmentID,
                NotificationSubject = GetNotificationSubject(notificationType),
                NotificationMessage = notificationMessage,
                Status = GetNotificationStatus(notificationType),
                HasRead = false,
                //SenderName = $"{enrollment.ManagerFirstName} {enrollment.ManagerLastName}",

            };

            return await AddAsync(appNotification);
        }


        public async Task<int> AddAsync(AppNotificationModel appNotification)
        {
            const string INSERT_NOTIFICATION_QUERY = @"
            INSERT INTO [dbo].[AppNotification] ([UserID], [EnrollmentID], [NotificationSubject], [NotificationMessage], [Status], [HasRead])
            VALUES (@UserID, @EnrollmentID, @NotificationSubject, @NotificationMessage, @Status, @HasRead)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", appNotification.UserID),
                new SqlParameter("@EnrollmentID", appNotification.EnrollmentID),
                new SqlParameter("@NotificationSubject", appNotification.NotificationSubject),
                new SqlParameter("@NotificationMessage", appNotification.NotificationMessage),
                new SqlParameter("@Status", appNotification.Status),
                new SqlParameter("@HasRead", appNotification.HasRead),
  
            };

            return await _dbCommand.InsertUpdateDataAsync(INSERT_NOTIFICATION_QUERY, parameters);
        }

        public async Task<IEnumerable<AppNotificationModel>> GetAllAsync()
        {
            const string GET_ALL_NOTIFICATIONS_QUERY = @"SELECT * FROM [dbo].[AppNotification]";
            List<AppNotificationModel> notifications = new List<AppNotificationModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_NOTIFICATIONS_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    AppNotificationModel notification = new AppNotificationModel
                    {
                        AppNotificationID = reader.GetInt32(reader.GetOrdinal("AppNotificationID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        NotificationSubject = reader.GetString(reader.GetOrdinal("NotificationSubject")),
                        NotificationMessage = reader.IsDBNull(reader.GetOrdinal("NotificationMessage")) ? string.Empty : reader.GetString(reader.GetOrdinal("NotificationMessage")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString(reader.GetOrdinal("Status")),
                        HasRead = reader.GetBoolean(reader.GetOrdinal("HasRead")),
                    };
                    notifications.Add(notification);
                }
            }   

            return notifications;
        }


        public async Task<IEnumerable<AppNotificationModel>> GetByUserIdAsync(int userId)
        {
            const string GET_NOTIFICATION_BY_ID_QUERY = @"SELECT * FROM [dbo].[AppNotification] WHERE UserID = @UserID ORDER BY CreateTimeStamp DESC";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId),
            };

            List<AppNotificationModel> notifications = new List<AppNotificationModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_NOTIFICATION_BY_ID_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    AppNotificationModel notification = new AppNotificationModel
                    {
                        AppNotificationID = reader.GetInt32(reader.GetOrdinal("AppNotificationID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        NotificationSubject = reader.GetString(reader.GetOrdinal("NotificationSubject")),
                        NotificationMessage = reader.IsDBNull(reader.GetOrdinal("NotificationMessage")) ? string.Empty : reader.GetString(reader.GetOrdinal("NotificationMessage")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString(reader.GetOrdinal("Status")),
                        HasRead = reader.GetBoolean(reader.GetOrdinal("HasRead")),
                        CreateTimeStamp = reader.GetDateTime(reader.GetOrdinal("CreateTimeStamp")),
                    };
                    notifications.Add(notification);
                }
            }

            return notifications;
        }

        public async Task<int> MarkNotificationAsReadAsync(int notificationId)
        {
            const string UPDATE_NOTIFICATION_QUERY = @"
        UPDATE [dbo].[AppNotification] 
        SET [HasRead] = 1
        WHERE [AppNotificationID] = @AppNotificationID";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AppNotificationID", notificationId),
            };

            return await _dbCommand.InsertUpdateDataAsync(UPDATE_NOTIFICATION_QUERY, parameters);
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            const string GET_UNREAD_NOTIFICATION_COUNT_QUERY = @"
                                                                SELECT COUNT(*) As UnreadNotifications
                                                                FROM [dbo].[AppNotification] 
                                                                WHERE UserID = @UserID AND HasRead = 0";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", userId) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_UNREAD_NOTIFICATION_COUNT_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return reader.GetInt32(reader.GetOrdinal("UnreadNotifications"));
                }
            }

            return -1;
        }

        // Helper method to get relevant user ID based on notification type
        private int GetRelevantUserId(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Approval:
                case NotificationType.Rejection:
                case NotificationType.Confirmation:
                    return enrollment.AppUserID;

                case NotificationType.Enrollment:
                    return enrollment.ManagerID;
                // Add more cases for other notification types if needed

                default:
                    return enrollment.ManagerID;
            }
        }

        // Helper methods to get subject and status based on notification type
        private string GetNotificationSubject(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Approval:
                    return "Enrollment Approved";

                case NotificationType.Rejection:
                    return "Enrollment Rejected";

                case NotificationType.Confirmation:
                    return "Enrollment Confirmed";

                case NotificationType.Enrollment: // Handle the new Enrollment type
                    return "Training Enrollment";


                default:
                    return "New Notification";
            }
        }


        private string GetNotificationStatus(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Approval:
                    return "Approved";

                case NotificationType.Rejection:
                    return "Rejected";

                case NotificationType.Confirmation:
                    return "Confirmed";

                case NotificationType.Enrollment: // Handle the new Enrollment type
                    return "Enrolled";
                // Add more cases for other notification types if needed

                default:
                    return "New";
            }
        }


        public string ConstructNotificationMessage(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            int relevantUserId = GetRelevantUserId(enrollment, notificationType);

            switch (notificationType)
            {
                case NotificationType.Approval:
                    return $"Your enrollment for '{enrollment.TrainingName}' has been approved by {enrollment.ManagerFirstName} {enrollment.ManagerLastName}.";

                case NotificationType.Rejection:
                    return $"Your Rejection for '{enrollment.TrainingName}' has been rejected. Reason: {enrollment.DeclineReason}";

                case NotificationType.Confirmation:
                    return $"Your enrollment for '{enrollment.TrainingName}' has been confirmed by {enrollment.ManagerFirstName} {enrollment.ManagerLastName}.";

                case NotificationType.Enrollment: // Handle the new Enrollment type
                    return $"Your employee {enrollment.AppUserFirstName} {enrollment.AppUserLastName} has enrolled for '{enrollment.TrainingName}'.";
                // Add more cases for other notification types if needed

                default:
                    return "New notification received.";
            }
        }

    }
}
