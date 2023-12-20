using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.SkillsForce.DAL
{
    public interface IUserRoleDAL
    {
        void Add(UserRoleModel userRole);
        void Delete(int id);
        IEnumerable<PrerequisiteModel> GetAll();
        PrerequisiteModel GetByID(int id);
        void Update(PrerequisiteModel prerequisite);
    }
}