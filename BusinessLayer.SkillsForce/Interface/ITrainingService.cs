using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface ITrainingService
    {
        IEnumerable<TrainingModel> GetAll();
        TrainingModel GetByID(int id);
        void Add(TrainingModel training);
        void Delete(int id);
        void Update(TrainingModel training);
        IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsiites();
        int GetCapacityID(int id);
        IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline);
    }
}
