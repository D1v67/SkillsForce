using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        IEnumerable<UserModel> GetAllManager();
        UserModel GetByID(int id);
        void Add(UserModel user);
        void Delete(int id);
        void Update(UserModel user);
        bool Register(UserModel user);
        bool Login(UserModel user);
        bool Logout(UserModel user);
        bool ApproveRequest(UserModel user, TrainingModel traning);

    }
}
