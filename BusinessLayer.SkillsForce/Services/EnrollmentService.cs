﻿using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.AppLogger;
using Common.SkillsForce.BackgoundJobLogger;
using Common.SkillsForce.Enums;
using Common.SkillsForce.Helpers;
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
        private readonly INotificationHandler _notificationHandler;
        private readonly CustomLogger _logger;

        public EnrollmentService(IEnrollmentDAL enrollmentDAL, ITrainingService trainingService, INotificationHandler notificationHandler)
        {
            _enrollmentDAL = enrollmentDAL;
            _trainingService = trainingService;
            _notificationHandler = notificationHandler;
            _logger = new CustomLogger();
        }
        public async Task<int> AddAsync(EnrollmentViewModel enrollment)
        {
            return await _enrollmentDAL.AddAsync(enrollment);
        }

        public async Task ApproveEnrollmentAsync(int enrollmentId, int approvedByUserId)
        {
            await _enrollmentDAL.ApproveEnrollmentAsync(enrollmentId, approvedByUserId);
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


        public async Task<ExecutionResult> RunAutomaticSelectionOfApprovedEnrollmentsAsync(bool isCronjob)
        {
            DateTime executionStartTime = DateTime.Now;
            _logger.Log($"Executing Automatic Selection at {executionStartTime}...");

            var Errors = new List<string>();
            var SuccessMessages = new List<string>();

            DateTime registrationDeadline = new DateTime(2024, 03, 01);
            var trainings = await _trainingService.GetAllTrainingsByRegistrationDeadlineAsync(registrationDeadline, isCronjob);

            if (trainings == null || !trainings.Any())
            {
                Errors.Add("No trainings available for selection until today.");

                _logger.Log("No trainings available for selection until today.");

                return new ExecutionResult { IsSuccessful = false, Errors = Errors, SuccessMessages = SuccessMessages };
            }

            foreach (var training in trainings)
            {
                var enrollmentIds = await _enrollmentDAL.ConfirmEnrollmentsByTrainingIDAsync(training.TrainingID);

                if (enrollmentIds != null && enrollmentIds.Any())
                {
                    SuccessMessages.Add($"Selection done for training: {training.TrainingName}");
                    _logger.Log($"Selection done for training: {training.TrainingName}");

                    foreach (var enrollmentId in enrollmentIds)
                    {
                        EnrollmentNotificationViewModel enrollment = await GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);

                        await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Confirmation);                      
                    }
                }
            }

            //_logger.Log(SuccessMessages.ToString());
            _logger.Log($"RunAutomaticSelectionOfApprovedEnrollmentsAsync executed successfully at {DateTime.Now}.{Environment.NewLine}");
            return new ExecutionResult { IsSuccessful = true, Errors = Errors, SuccessMessages = SuccessMessages };
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredEnrollmentsWithDetailsAsync(int trainingId, string statusFilter)
        {
            if (statusFilter == EnrollmentStatusEnum.Selected.ToString())
            {
                return await _enrollmentDAL.GetAllFilteredConfirmedEnrollmentsWithDetailsAsync(trainingId);
            }
            else
            {
                return await _enrollmentDAL.GetAllFilteredEnrollmentsWithDetailsAsync(trainingId, statusFilter);
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsAsync(int userId)
        {
           return await _enrollmentDAL.GetAllConfirmedEnrollmentsAsync(userId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsWithDetailsAsync()
        {
            return await _enrollmentDAL.GetAllConfirmedEnrollmentsWithDetailsAsync();
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllDeclinedEnrollmentsByUserIDAsync(int userId)
        {
            return await _enrollmentDAL.GetAllDeclinedEnrollmentsByUserIDAsync(userId);
        }

        public async Task<int> ReEnrollAddAsync(EnrollmentViewModel enrollment)
        {
            return await _enrollmentDAL.ReEnrollAddAsync(enrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllPendingEnrollmentsAsync(int userId)
        {
            return await _enrollmentDAL.GetAllPendingEnrollmentsAsync(userId);
        }

        public async Task UnEnrollAsync(int enrollmentId)
        {
            await _enrollmentDAL.DeleteEnrollmentAsync(enrollmentId);
        }
    }
}


//public async Task RunAutomaticSelectionOfApprovedEnrollmentsAsync(bool isCronjob)
//{
//    DateTime registrationDeadline = new DateTime(2024, 03, 01);
//    var trainings = await _trainingService.GetAllTrainingsByRegistrationDeadlineAsync(registrationDeadline, isCronjob);

//    foreach (var training in trainings)
//    {
//        var enrollmentIds = await _enrollmentDAL.ConfirmEnrollmentsByTrainingIDAsync(training.TrainingID);

//        if (enrollmentIds != null && enrollmentIds.Any())
//        {
//            foreach (var enrollmentId in enrollmentIds)
//            {
//                EnrollmentNotificationViewModel enrollment = await GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);

//                await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Confirmation);

//            }
//        }
//    }
//}