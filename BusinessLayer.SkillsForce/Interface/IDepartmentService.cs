using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentModel>> GetAllAsync();
        Task<DepartmentModel> GetByIDAsync(int id);
        Task AddAsync(DepartmentModel department);
        Task DeleteAsync(int id);
        Task UpdateAsync(DepartmentModel department);



        IEnumerable<DepartmentModel> GetAll();
        DepartmentModel GetByID(int id);
        void Add(DepartmentModel department);
        void Delete(int id);
        void Update(DepartmentModel department);
    }
}
