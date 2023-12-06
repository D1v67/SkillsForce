using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
