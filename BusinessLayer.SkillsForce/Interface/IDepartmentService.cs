using Common.SkillsForce.Entity;
using System.Collections.Generic;


namespace BusinessLayer.SkillsForce.Interface
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentModel> GetAll();
        DepartmentModel GetByID(int id);
        void Add(DepartmentModel department);
        void Delete(int id);
        void Update(DepartmentModel department);
    }
}
