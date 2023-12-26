using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingService(ITrainingDAL trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

        public void Add(TrainingModel training)
        {
            _trainingDAL.Add(training);
        }

        public void Delete(int id)
        {
            _trainingDAL.Delete(id);
        }

        public IEnumerable<TrainingModel> GetAll()
        {
            return _trainingDAL.GetAll();
        }

        public IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline)
        {
            return _trainingDAL.GetAllTrainingsByRegistrationDeadline(registrationDeadline);
        }

        public IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsiites()
        {
            return _trainingDAL.GetAllTrainingWithPrerequsiites();
        }

        public TrainingModel GetByID(int id)
        {
            return _trainingDAL.GetByID(id);
        }

        public int GetCapacityID(int id)
        {
            return _trainingDAL.GetCapacityID(id);
        }

        public void Update(TrainingModel training)
        {
            _trainingDAL.Update(training);
        }
    }
}
