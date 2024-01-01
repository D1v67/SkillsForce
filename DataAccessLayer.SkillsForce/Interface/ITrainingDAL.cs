using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface ITrainingDAL
    {

        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<TrainingModel> GetByIDAsync(int id);
        Task AddAsync(TrainingViewModel training);
        Task<bool> DeleteAsync(int id);
        Task UpdateAsync(TrainingViewModel training);
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingWithPrerequisitesAsync();
        Task<int> GetCapacityIDAsync(int id);
        Task<int> GetRemainingCapacityIDAsync(int trainingID);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsByRegistrationDeadlineAsync(DateTime registrationDeadline, bool isCronJob);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsEnrolledByUserAsync(int id);
        Task<IEnumerable<TrainingModel>> GetAllTrainingsNotEnrolledByUserAsync(int id);
        Task<bool> IsTrainingNameAlreadyExistsAsync(string trainingName);
        Task<TrainingViewModel> GetTrainingWithPrerequisitesAsync(int trainingId);
        Task<bool> IsTrainingNameAlreadyExistsOnUpdateAsync(int trainingId, string newTrainingName);





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
        bool IsTrainingNameAlreadyExists(string trainingName);
        TrainingViewModel GetTrainingWithPrerequisites(int trainingId);
        bool IsTrainingNameAlreadyExistsOnUpdate(int trainingId, string newTrainingName);
    }
}
