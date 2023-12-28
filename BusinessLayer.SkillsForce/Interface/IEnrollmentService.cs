using Common.SkillsForce.ViewModel;
using System.Collections.Generic;


namespace BusinessLayer.SkillsForce.Interface
{
    public interface IEnrollmentService
    {
        IEnumerable<EnrollmentViewModel> GetAll();
        EnrollmentViewModel GetByID(int id);
        int Add(EnrollmentViewModel enrollment);
        void Delete(int id);
        void Update(EnrollmentViewModel enrollment);
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetails();
        IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetailsByManager(int managerId);
        void ApproveEnrollment(int enrollmentId);
        void RejectEnrollment(int enrollmentId, string rejectionReason, int declinedByUserId);
        EnrollmentNotificationViewModel GetEnrollmentNotificationDetailsByID(int id);
        IEnumerable<EnrollmentViewModel> GetAllApprovedEnrollments();
        List<int> ConfirmEnrollmentsByTrainingID(int trainingID);

        void RunAutomaticSelectionOfApprovedEnrollments(bool isCronjob);
    }
}
