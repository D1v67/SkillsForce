using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IDepartmentDAL
    {
        IEnumerable<DepartmentModel> GetAll();
        DepartmentModel GetByID(int id);
        void Add(DepartmentModel department);
        void Delete(int id);
        void Update(DepartmentModel department);
    }
}
