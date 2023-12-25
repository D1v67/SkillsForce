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

        public bool IsEmailAlreadyExists(string email)
        {
            return _userDAL.IsEmailAlreadyExists(email);
        }

        public bool IsNICExists(string nic)
        {
            return _userDAL.IsNICExists(nic);
        }

        public bool IsMobileNumberExists(string mobileNumber)
        {
            return _userDAL.IsMobileNumberExists(mobileNumber);
        }
    }
}
