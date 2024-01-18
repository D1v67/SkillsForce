using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface ITrainingService
    {
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<TrainingModel> GetByIDAsync(int id);
        Task<ValidationResult> AddAsync(TrainingViewModel training);
        Task<bool> DeleteAsync(int id);
        Task<ValidationResult> UpdateAsync(TrainingViewModel training);
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingWithPrerequisitesAsync();
        Task<int> GetCapacityIDAsync(int id);
        Task<int> GetRemainingCapacityIDAsync(int trainingID);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsByRegistrationDeadlineAsync(DateTime registrationDeadline, bool isCronJob);
        Task<IEnumerable<TrainingEnrollmentViewModel>> GetAllTrainingsEnrolledByUserAsync(int id);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsNotEnrolledByUserAsync(int id);
        Task<bool> IsTrainingNameAlreadyExistsAsync(string trainingName);
        Task<TrainingViewModel> GetTrainingWithPrerequisitesAsync(int trainingId);
        Task<bool> IsTrainingNameAlreadyExistsOnUpdateAsync(int trainingId, string newTrainingName);
        bool Delete(int id);
        Task<bool> IsTrainingHaveEnrollment(int trainingId);
        Task<IEnumerable<TrainingModel>> GetAllTrainingByTrainerIDAsync(int id);
    }
}
