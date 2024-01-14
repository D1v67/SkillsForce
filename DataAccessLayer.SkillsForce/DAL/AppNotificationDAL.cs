using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            string notificationMessage = ConstructNotificationMessage(enrollment, notificationType);
            AppNotificationModel appNotification = new AppNotificationModel
            {
                UserID = relevantUserId,
                EnrollmentID = enrollment.EnrollmentID,
                NotificationSubject = GetNotificationSubject(notificationType),
                NotificationMessage = notificationMessage,
                Status = GetNotificationStatus(notificationType),
                HasRead = false,
                NotificationSender = "SkillsForce"
                //NotificationSender = $"{enrollment.ManagerFirstName} {enrollment.ManagerLastName}",
            };
            return await AddAsync(appNotification);
        }

        public async Task<int> AddAsync(AppNotificationModel appNotification)
        {
            const string INSERT_NOTIFICATION_QUERY = @"
            INSERT INTO [dbo].[AppNotification] ([UserID], [EnrollmentID], [NotificationSubject], [NotificationMessage], [Status], [HasRead], [NotificationSender])
            VALUES (@UserID, @EnrollmentID, @NotificationSubject, @NotificationMessage, @Status, @HasRead, @NotificationSender)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", appNotification.UserID),
                new SqlParameter("@EnrollmentID", appNotification.EnrollmentID),
                new SqlParameter("@NotificationSubject", appNotification.NotificationSubject),
                new SqlParameter("@NotificationMessage", appNotification.NotificationMessage),
                new SqlParameter("@Status", appNotification.Status),
                new SqlParameter("@HasRead", appNotification.HasRead),
                new SqlParameter("@NotificationSender", appNotification.NotificationSender),
            };
            return await _dbCommand.InsertUpdateDataAsync(INSERT_NOTIFICATION_QUERY, parameters);
        }

        public async Task<IEnumerable<AppNotificationModel>> GetAllAsync()
        {
            const string GET_ALL_NOTIFICATIONS_QUERY = @"SELECT [AppNotificationID], [UserID], [EnrollmentID], [NotificationSubject],  [NotificationMessage], [Status], [HasRead], [NotificationSender] 
            FROM [dbo].[AppNotification]";

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
                        NotificationSender = reader.GetString(reader.GetOrdinal("NotificationSender")),
                    };
                    notifications.Add(notification);
                }
            }   
            return notifications;
        }

        public async Task<IEnumerable<AppNotificationModel>> GetByUserIdAsync(int userId)
        {
            const string GET_NOTIFICATION_BY_ID_QUERY = @"SELECT [AppNotificationID], [UserID], [EnrollmentID], [NotificationSubject],  [NotificationMessage], [Status], [HasRead], [CreateTimeStamp], [NotificationSender] 
            FROM [dbo].[AppNotification] WHERE UserID = @UserID ORDER BY CreateTimeStamp DESC";

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
                        NotificationSender = reader.GetString(reader.GetOrdinal("NotificationSender")),
                    };
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        public async Task<int> MarkNotificationAsReadAsync(int notificationId)
        {
            const string UPDATE_NOTIFICATION_QUERY = @"UPDATE [dbo].[AppNotification] SET [HasRead] = 1 WHERE [AppNotificationID] = @AppNotificationID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AppNotificationID", notificationId),
            };
            return await _dbCommand.InsertUpdateDataAsync(UPDATE_NOTIFICATION_QUERY, parameters);
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            const string GET_UNREAD_NOTIFICATION_COUNT_QUERY = @"SELECT COUNT(*) As UnreadNotifications FROM [dbo].[AppNotification] WHERE UserID = @UserID AND HasRead = 0";
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

                case NotificationType.Enrollment: 
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

                case NotificationType.Enrollment: 
                    return "Enrolled";
  
                default:
                    return "New";
            }
        }

        private string ConstructNotificationMessage(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            int relevantUserId = GetRelevantUserId(enrollment, notificationType);

            switch (notificationType)
            {
                case NotificationType.Approval:
                    return $"Hello {enrollment.AppUserFirstName} {enrollment.AppUserLastName},\n\nCongratulations! Your enrollment for the '{enrollment.TrainingName}' training program has been approved by {enrollment.ManagerFirstName} {enrollment.ManagerLastName}. We're delighted to share this exciting news with you. Your commitment to continuous learning is truly commendable.";

                case NotificationType.Rejection:
                    return $"Hello {enrollment.AppUserFirstName} {enrollment.AppUserLastName},\n\nWe regret to inform you that your enrollment for the '{enrollment.TrainingName}' training program has been rejected. The reason for the rejection is: {enrollment.DeclineReason}. We understand that this might be disappointing, and we encourage you to reach out if you have any questions or if there's anything we can assist you with.";

                case NotificationType.Confirmation:
                    return $"Hello {enrollment.AppUserFirstName} {enrollment.AppUserLastName},\n\nGreat news! Your enrollment for the '{enrollment.TrainingName}' training program has been confirmed by {enrollment.ManagerFirstName} {enrollment.ManagerLastName}. We're thrilled to have you on board and look forward to your active participation. If you have any further queries or need additional information, feel free to let us know.";

                case NotificationType.Enrollment:
                    return $"Hello {enrollment.ManagerFirstName} {enrollment.ManagerLastName},\n\nWe are pleased to inform you that your employee, {enrollment.AppUserFirstName} {enrollment.AppUserLastName}, has enrolled for the '{enrollment.TrainingName}' training program. This initiative reflects the dedication to professional development within your team. If you have any specific requirements or would like to offer further support, please don't hesitate to reach out.";

                default:
                    return "New notification received.";
            }
        }

    }
}
