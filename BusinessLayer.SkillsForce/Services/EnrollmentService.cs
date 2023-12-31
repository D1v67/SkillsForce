using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public int Add(EnrollmentViewModel enrollment)
        {
           return _enrollmentDAL.Add(enrollment);
        }

        public void ApproveEnrollment(int enrollmentId)
        {
            _enrollmentDAL.ApproveEnrollment(enrollmentId);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EnrollmentViewModel> GetAll()
        {
            return _enrollmentDAL.GetAll();
        }

        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetails()
        {
            return _enrollmentDAL.GetAllEnrollmentsWithDetails();
        }

        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetailsByManager(int managerId)
        {
            return _enrollmentDAL.GetAllEnrollmentsWithDetailsByManager(managerId);
        }

        public IEnumerable<EnrollmentViewModel> GetAllApprovedEnrollments()
        {
            return _enrollmentDAL.GetAllApprovedEnrollments();
        }

        public EnrollmentViewModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public EnrollmentNotificationViewModel GetEnrollmentNotificationDetailsByID(int id)
        {
            return _enrollmentDAL.GetEnrollmentNotificationDetailsByID(id);
        }

        public void RejectEnrollment(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
            _enrollmentDAL.RejectEnrollment( enrollmentId,  rejectionReason, declinedByUserId);
        }

        public void Update(EnrollmentViewModel enrollment)
        {
            throw new NotImplementedException();
        }

        public List<int> ConfirmEnrollmentsByTrainingID(int trainingID)
        {
            return _enrollmentDAL.ConfirmEnrollmentsByTrainingID(trainingID);
        }

        public void RunAutomaticSelectionOfApprovedEnrollments(bool isCronjob)
        {
            DateTime registrationDeadline = new DateTime(2024,03,01);
            //DateTime registrationDeadline = DateTime.Now;
            var trainings = _trainingService.GetAllTrainingsByRegistrationDeadline(registrationDeadline, isCronjob);

            foreach (var training in trainings)
            {
                var enrollmentIds = _enrollmentDAL.ConfirmEnrollmentsByTrainingID(training.TrainingID);

                if (enrollmentIds != null && enrollmentIds.Any())
                {
                    foreach (var enrollmentId in enrollmentIds)
                    {
                        EnrollmentNotificationViewModel enrollment = GetEnrollmentNotificationDetailsByID(enrollmentId);
                        _notificationService.SendNotification(enrollment, NotificationType.Confirmation);
                    }
                }
            }
        }


        


        public bool RunAutomaticSelectionOfApprovedEnrollmentsByAdmin(bool isCronjob)
        {
            var trainings = _trainingService.GetAllTrainingsByRegistrationDeadline(DateTime.Now, isCronjob);

            return trainings?.Any() == true && trainings.All(training =>
            {
                _enrollmentDAL.ConfirmEnrollmentsByTrainingID(training.TrainingID);
                return true;
            });
        }
    }
}
