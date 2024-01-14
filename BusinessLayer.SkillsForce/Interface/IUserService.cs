﻿ 
using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<IEnumerable<UserModel>> GetAllManagerAsync();
        Task<UserModel> GetByIDAsync(int id);
        Task AddAsync(UserModel user);
        Task DeleteAsync(int id);
        Task UpdateAsync(UserModel user);
        Task<bool> IsEmailAlreadyExistsAsync(string email);
        Task<bool> IsNICExistsAsync(string nic);
        Task<bool> IsMobileNumberExistsAsync(string mobileNumber);
    }
}
