using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            return await _departmentDAL.GetAllAsync();
        }

        public async Task<DepartmentModel> GetByIDAsync(int id)
        {
            return await _departmentDAL.GetByIDAsync(id);
        }

        public async Task AddAsync(DepartmentModel department)
        {
            await _departmentDAL.AddAsync(department);
        }

        public async Task DeleteAsync(int id)
        {
            await _departmentDAL.DeleteAsync(id);
        }

        public async Task UpdateAsync(DepartmentModel department)
        {
            await _departmentDAL.UpdateAsync(department);
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
