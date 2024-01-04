using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly ITrainingService _trainingService;
        private readonly INotificationService _notificationService;

        public EnrollmentService(IEnrollmentDAL enrollmentDAL, ITrainingService trainingService, INotificationService notificationService)
        {
            _enrollmentDAL = enrollmentDAL;
            _trainingService = trainingService;
            _notificationService = notificationService;
        }
        public async Task<int> AddAsync(EnrollmentViewModel enrollment)
        {
            return await _enrollmentDAL.AddAsync(enrollment);
        }

        public async Task ApproveEnrollmentAsync(int enrollmentId)
        {
            await _enrollmentDAL.ApproveEnrollmentAsync(enrollmentId);
        }


        public async Task<IEnumerable<EnrollmentViewModel>> GetAllAsync()
        {
            return await _enrollmentDAL.GetAllAsync();
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsAsync()
        {
            return await _enrollmentDAL.GetAllEnrollmentsWithDetailsAsync();
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsByManagerAsync(int managerId)
        {
            return await _enrollmentDAL.GetAllEnrollmentsWithDetailsByManagerAsync(managerId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllApprovedEnrollmentsAsync()
        {
            return await _enrollmentDAL.GetAllApprovedEnrollmentsAsync();
        }

        public async Task<EnrollmentNotificationViewModel> GetEnrollmentNotificationDetailsByIDAsync(int id)
        {
            return await _enrollmentDAL.GetEnrollmentNotificationDetailsByIDAsync(id);
        }

        public async Task RejectEnrollmentAsync(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
            await _enrollmentDAL.RejectEnrollmentAsync(enrollmentId, rejectionReason, declinedByUserId);
        }


        public async Task<List<int>> ConfirmEnrollmentsByTrainingIDAsync(int trainingID)
        {
            return await _enrollmentDAL.ConfirmEnrollmentsByTrainingIDAsync(trainingID);
        }

        public async Task RunAutomaticSelectionOfApprovedEnrollmentsAsync(bool isCronjob)
        {
            DateTime registrationDeadline = new DateTime(2024, 03, 01);
            var trainings = await _trainingService.GetAllTrainingsByRegistrationDeadlineAsync(registrationDeadline, isCronjob);

            foreach (var training in trainings)
            {
                var enrollmentIds = await _enrollmentDAL.ConfirmEnrollmentsByTrainingIDAsync(training.TrainingID);

                if (enrollmentIds != null && enrollmentIds.Any())
                {
                    foreach (var enrollmentId in enrollmentIds)
                    {
                        EnrollmentNotificationViewModel enrollment = await GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);
#pragma warning disable CS4014 
                        _notificationService.SendNotificationAsync(enrollment, NotificationType.Confirmation);
#pragma warning restore CS4014 
                    }
                }
            }
        }
    }
}
