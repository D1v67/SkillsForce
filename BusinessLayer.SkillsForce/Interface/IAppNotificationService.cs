using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
   public  interface IAppNotificationService
    {
        Task<int> AddNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType);
        Task<IEnumerable<AppNotificationModel>> GetAllAsync();
        Task<int> AddAsync(AppNotificationModel model);
        Task<IEnumerable<AppNotificationModel>> GetByUserIdAsync(int id);
        Task<int> MarkNotificationAsReadAsync(int notificationId);
        Task<int> GetUnreadNotificationCountAsync(int userId);
    }
}
