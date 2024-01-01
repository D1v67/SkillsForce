using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class PrerequisiteService : IPrerequisiteService
    {
        private readonly IPrerequisiteDAL _prerequisiteDAL;
        public PrerequisiteService(IPrerequisiteDAL prerequisiteDAL)
        {
            _prerequisiteDAL = prerequisiteDAL;
        }

        public async Task<IEnumerable<PrerequisiteModel>> GetPrerequisiteByTrainingIDAsync(int TrainingID)
        {
            return await _prerequisiteDAL.GetPrerequisiteByTrainingIDAsync(TrainingID);
        }

        public async Task<IEnumerable<PrerequisiteModel>> GetAllAsync()
        {
            return await _prerequisiteDAL.GetAllAsync();
        }




        public IEnumerable<PrerequisiteModel> GetPrerequisiteByTrainingID(int TrainingID)
        {
            return _prerequisiteDAL.GetPrerequisiteByTrainingID(TrainingID);
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
           return _prerequisiteDAL.GetAll();
        }

    }
}
