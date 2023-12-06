using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IEnrollmentDAL
    {
        IEnumerable<EnrollmentModel> GetAll();
        EnrollmentModel GetByID(int id);
        void Add(EnrollmentModel enrollment);
        void Delete(int id);
        void Update(EnrollmentModel enrollment);
    }
}
