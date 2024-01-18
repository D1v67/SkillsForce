using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class AppNotificationService : IAppNotificationService, INotificationService
    {
        private readonly IAppNotificationDAL _appNotificationDAL;

        public AppNotificationService(IAppNotificationDAL notificationDAL) => _appNotificationDAL = notificationDAL;

        public async Task<int> AddAsync(AppNotificationModel model) => await _appNotificationDAL.AddAsync(model);

        public async Task<int> AddNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType) =>
            await _appNotificationDAL.AddNotificationAsync(enrollment, notificationType);

        public async Task<IEnumerable<AppNotificationModel>> GetAllAsync() => await _appNotificationDAL.GetAllAsync();

        public async Task<IEnumerable<AppNotificationModel>> GetByUserIdAsync(int id) => await _appNotificationDAL.GetByUserIdAsync(id);

        public async Task<int> GetUnreadNotificationCountAsync(int userId) => await _appNotificationDAL.GetUnreadNotificationCountAsync(userId);

        public Task<int> MarkNotificationAsReadAsync(int notificationId) => _appNotificationDAL.MarkNotificationAsReadAsync(notificationId);

        public async Task<string> SendNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType)
        {
            int affectedRows = await _appNotificationDAL.AddNotificationAsync(enrollment, notificationType);
            return $"Notification added successfully. Affected rows: {affectedRows}";
        }
    }
}
