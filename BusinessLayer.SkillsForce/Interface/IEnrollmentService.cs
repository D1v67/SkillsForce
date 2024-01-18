﻿using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentViewModel>> GetAllAsync();
        Task<int> AddAsync(EnrollmentViewModel enrollment);
        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsAsync();
        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsByManagerAsync(int managerId);
        Task ApproveEnrollmentAsync(int enrollmentId, int approvedByUserId);
        Task RejectEnrollmentAsync(int enrollmentId, string rejectionReason, int declinedByUserId);
        Task<EnrollmentNotificationViewModel> GetEnrollmentNotificationDetailsByIDAsync(int id);
        Task<IEnumerable<EnrollmentViewModel>> GetAllApprovedEnrollmentsAsync();
        Task<List<int>> ConfirmEnrollmentsByTrainingIDAsync(int? userId, int trainingID);
        Task<ExecutionResult> RunAutomaticSelectionOfApprovedEnrollmentsAsync(int? userId, bool isCronjob);
        Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredEnrollmentsWithDetailsAsync(int trainingId, string statusFilter);
        Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsAsync(int userId);
        Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsWithDetailsAsync();
        Task<IEnumerable<EnrollmentViewModel>> GetAllDeclinedEnrollmentsByUserIDAsync(int userId);
        Task<int> ReEnrollAddAsync(EnrollmentViewModel enrollment);
        Task<IEnumerable<EnrollmentViewModel>> GetAllPendingEnrollmentsAsync(int userId);
        Task UnEnrollAsync(int enrollmentId);
    }
}
