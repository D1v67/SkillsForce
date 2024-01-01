using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAL _userDAL;

        public UserService(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }


        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return await _userDAL.GetAllAsync();
        }

        public async Task<IEnumerable<UserModel>> GetAllManagerAsync()
        {
            return await _userDAL.GetAllManagerAsync();
        }

        public async Task<UserModel> GetByIDAsync(int id)
        {
            return await _userDAL.GetByIDAsync(id);
        }

        public async Task AddAsync(UserModel user)
        {
            await _userDAL.AddAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userDAL.DeleteAsync(id);
        }

        public async Task UpdateAsync(UserModel user)
        {
            await _userDAL.UpdateAsync(user);
        }

        public async Task<bool> IsEmailAlreadyExistsAsync(string email)
        {
            return await _userDAL.IsEmailAlreadyExistsAsync(email);
        }

        public async Task<bool> IsNICExistsAsync(string nic)
        {
            return await _userDAL.IsNICExistsAsync(nic);
        }

        public async Task<bool> IsMobileNumberExistsAsync(string mobileNumber)
        {
            return await _userDAL.IsMobileNumberExistsAsync(mobileNumber);
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
