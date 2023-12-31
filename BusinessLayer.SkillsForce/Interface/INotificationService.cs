using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface INotificationService
    {
        string SendNotification(EnrollmentNotificationViewModel enrollment, NotificationType notificationType);
    }
}
