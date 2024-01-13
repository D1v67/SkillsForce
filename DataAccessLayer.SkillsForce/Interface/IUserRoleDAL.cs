using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.SkillsForce.DAL
{
    public interface IUserRoleDAL
    {
        void Add(UserRoleModel userRole);
        void Delete(int id);
        IEnumerable<UserRoleModel> GetAll();
        UserRoleModel GetByID(int id);
        void Update(UserRoleModel prerequisite);
    }
}