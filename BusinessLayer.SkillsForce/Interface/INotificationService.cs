using Common.SkillsForce.ViewModel;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface INotificationService
    {
        string SendApprovalNotification(EnrollmentNotificationViewModel enrollment);
        string SendRejectionNotification(EnrollmentNotificationViewModel enrollment);
    }
}
