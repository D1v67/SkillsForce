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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentDAL _departmentDAL;
        public DepartmentService(IDepartmentDAL departmentDAL)
        {
            _departmentDAL = departmentDAL;
        }

        public void Add(DepartmentModel department)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DepartmentModel> GetAll()
        {
            return _departmentDAL.GetAll();
        }

        public DepartmentModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(DepartmentModel department)
        {
            throw new NotImplementedException();
        }
    }
}
