using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface ITrainingDAL
    {
        IEnumerable<TrainingModel> GetAll();
        TrainingModel GetByID(int id);
        void Add(TrainingViewModel training);
        bool Delete(int id);
        void Update(TrainingViewModel training);
        IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsiites();
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
