using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IEnrollmentDAL
    {
        Task<IEnumerable<EnrollmentViewModel>> GetAllAsync();
        Task<int> AddAsync(EnrollmentViewModel enrollment);
        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsAsync();
        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsByManagerAsync(int managerId);
        Task ApproveEnrollmentAsync(int enrollmentId, int approvedByUserId);
        Task RejectEnrollmentAsync(int enrollmentId, string rejectionReason, int declinedByUserId);
        Task<EnrollmentNotificationViewModel> GetEnrollmentNotificationDetailsByIDAsync(int id);
        Task<IEnumerable<EnrollmentViewModel>> GetAllApprovedEnrollmentsAsync();
        Task<List<int>> ConfirmEnrollmentsByTrainingIDAsync(int trainingID);
        Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredEnrollmentsWithDetailsAsync(int trainingId, string statusFilter);
        Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredConfirmedEnrollmentsWithDetailsAsync(int trainingId);
    }
}
