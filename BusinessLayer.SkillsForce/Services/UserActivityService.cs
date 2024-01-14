﻿using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.DAL;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityDAL _userActivityDAL;

        public UserActivityService(IUserActivityDAL userActivityDAL)
        {
            _userActivityDAL = userActivityDAL;
        }
        public async Task<bool> AddUserActivity(UserActivityModel userActivity)
        {
            return await _userActivityDAL.AddUserActivity(userActivity);
        }

        public async Task<bool> AddUserLoginActivity(UserActivityModel userActivity)
        {
            return await _userActivityDAL.AddUserLoginActivity(userActivity);
        }
    }
}
