using Common.SkillsForce.ViewModel;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface INotificationService
    {
        string SendNotification(EnrollmentNotificationViewModel enrollment);
    }
}
