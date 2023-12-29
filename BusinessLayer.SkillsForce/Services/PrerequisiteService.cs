using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Services
{
    public class PrerequisiteService : IPrerequisiteService
    {
        private readonly IPrerequisiteDAL _prerequisiteDAL;
        public PrerequisiteService(IPrerequisiteDAL prerequisiteDAL)
        {
            _prerequisiteDAL = prerequisiteDAL;
        }
        public IEnumerable<PrerequisiteModel> GetPrerequisiteByTrainingID(int TrainingID)
        {
            return _prerequisiteDAL.GetPrerequisiteByTrainingID(TrainingID);
        }

        public void Add(PrerequisiteModel prerequisite)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
           return _prerequisiteDAL.GetAll();
        }

        public PrerequisiteModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PrerequisiteModel prerequisite)
        {
            throw new NotImplementedException();
        }
    }
}
