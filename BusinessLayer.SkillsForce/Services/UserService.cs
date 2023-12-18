using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAL _userDAL;

        public UserService(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userDAL.GetAll();
        }

        public IEnumerable<UserModel> GetAllManager()
        {
            return _userDAL.GetAllManager();
        }
        public UserModel GetByID(int id)
        {
            return _userDAL.GetByID(id);
        }

        public void Add(UserModel user)
        {
            _userDAL.Add(user);
        }

        public void Delete(int id)
        {
            _userDAL.Delete(id);
        }
        public void Update(UserModel user)
        {
            _userDAL.Update(user);
        }

        public bool ApproveRequest(UserModel user, TrainingModel traning)
        {
            throw new NotImplementedException();
        }

        public bool DeclineRequest(UserModel user, TrainingModel traning)
        {
            throw new NotImplementedException();
        }

        public bool Login(UserModel user)
        {
            throw new NotImplementedException();
        }

        public bool Logout(UserModel user)
        {
            throw new NotImplementedException();
        }

        public bool Register(UserModel user)
        {
            throw new NotImplementedException();
        }


    }
}
