using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;


namespace BusinessLayer.SkillsForce.Services
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentService(IEnrollmentDAL enrollmentDAL)
        {
            _enrollmentDAL = enrollmentDAL;
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
    }
}
