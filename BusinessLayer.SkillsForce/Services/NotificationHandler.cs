using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class NotificationHandler : INotificationHandler
    {
        private List<INotificationService> handlers = new List<INotificationService>();

        public NotificationHandler(IEnumerable<INotificationService> notificationSevices)
        {
            handlers = notificationSevices.ToList();
        }
        public async Task NotifyHandlersAsync(EnrollmentNotificationViewModel enrollment, NotificationType type)
        {
            foreach (var handler in handlers)
            {
                await handler.SendNotificationAsync(enrollment, type);
            }
        }
    }
}
