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
        void Add(TrainingViewModel training);
        bool Delete(int id);
        void Update(TrainingViewModel training);
        IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsites();
        int GetCapacityID(int id);
        int GetRemainingCapacityID(int trainingID);
        IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline, bool isCronJob);
        IEnumerable<TrainingModel> GetAllTrainingsEnrolledByUser(int id);
        IEnumerable<TrainingModel> GetAllTrainingsNotEnrolledByUser(int id);
        TrainingViewModel GetTrainingViewModelDetailsWithDepartmentsAndPrerequsites();
        bool IsTrainingNameAlreadyExists(string trainingName);
        TrainingViewModel GetTrainingWithPrerequisites(int trainingId);
        bool IsTrainingNameAlreadyExistsOnUpdate(int trainingId, string newTrainingName);
    }
}
