using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
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
        public void Add(EnrollmentModel enrollment)
        {
            _enrollmentDAL.Add(enrollment);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EnrollmentModel> GetAll()
        {
            return _enrollmentDAL.GetAll();
        }

        public EnrollmentModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(EnrollmentModel enrollment)
        {
            throw new NotImplementedException();
        }
    }
}
