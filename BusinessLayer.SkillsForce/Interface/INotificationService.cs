using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface INotificationService
    {
        Task<string> SendNotificationAsync(EnrollmentNotificationViewModel enrollment, NotificationType notificationType);
    }
}
