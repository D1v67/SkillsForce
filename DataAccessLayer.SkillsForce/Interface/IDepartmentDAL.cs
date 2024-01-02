using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IDepartmentDAL
    {
        Task<IEnumerable<DepartmentModel>> GetAllAsync();
        Task<DepartmentModel> GetByIDAsync(int id);
        Task AddAsync(DepartmentModel department);
        Task DeleteAsync(int id);
        Task UpdateAsync(DepartmentModel department);
    }
}
