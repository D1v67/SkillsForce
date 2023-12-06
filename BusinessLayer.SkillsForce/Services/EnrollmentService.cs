using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentService(IEnrollmentDAL enrollmentDAL)
        {
            _enrollmentDAL = enrollmentDAL;
        }
        public void Add(EnrollmentViewModel enrollment)
        {
            _enrollmentDAL.Add(enrollment);
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
            throw new NotImplementedException();
        }

        public EnrollmentViewModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(EnrollmentViewModel enrollment)
        {
            throw new NotImplementedException();
        }
    }
}
