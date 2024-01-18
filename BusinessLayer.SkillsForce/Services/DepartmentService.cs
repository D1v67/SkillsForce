using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentDAL _departmentDAL;

        public DepartmentService(IDepartmentDAL departmentDAL) => _departmentDAL = departmentDAL;

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync() => await _departmentDAL.GetAllAsync();

        public async Task<DepartmentModel> GetByIDAsync(int id) => await _departmentDAL.GetByIDAsync(id);

        public async Task AddAsync(DepartmentModel department) => await _departmentDAL.AddAsync(department);

        public async Task DeleteAsync(int id) => await _departmentDAL.DeleteAsync(id);

        public async Task UpdateAsync(DepartmentModel department) => await _departmentDAL.UpdateAsync(department);
    }
}
