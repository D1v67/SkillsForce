using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public interface INotificationHandler
    {
        Task NotifyHandlersAsync(EnrollmentNotificationViewModel enrollment, NotificationType type);
    }
}