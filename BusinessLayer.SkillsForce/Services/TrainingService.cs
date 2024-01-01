using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingDAL _trainingDAL;
        private readonly IDepartmentDAL  _departmentDAL;
        private readonly IPrerequisiteDAL _prerequisiteDAL;

        public TrainingService(ITrainingDAL trainingDAL, IDepartmentDAL departmentDAL, IPrerequisiteDAL prerequisiteDAL)
        {
            _trainingDAL = trainingDAL;
            _departmentDAL = departmentDAL;
            _prerequisiteDAL = prerequisiteDAL;
        }

        public async Task AddAsync(TrainingViewModel training)
        {
            await _trainingDAL.AddAsync(training);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _trainingDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            return await _trainingDAL.GetAllAsync();
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsByRegistrationDeadlineAsync(DateTime registrationDeadline, bool isCronJob)
        {
            return await _trainingDAL.GetAllTrainingsByRegistrationDeadlineAsync(registrationDeadline, isCronJob);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsEnrolledByUserAsync(int id)
        {
            return await _trainingDAL.GetAllTrainingsEnrolledByUserAsync(id);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsNotEnrolledByUserAsync(int id)
        {
            return await _trainingDAL.GetAllTrainingsNotEnrolledByUserAsync(id);
        }

        public async Task<IEnumerable<TrainingViewModel>> GetAllTrainingWithPrerequisitesAsync()
        {
            return await _trainingDAL.GetAllTrainingWithPrerequisitesAsync();
        }

        public async Task<TrainingModel> GetByIDAsync(int id)
        {
            return await _trainingDAL.GetByIDAsync(id);
        }

        public async Task<int> GetCapacityIDAsync(int id)
        {
            return await _trainingDAL.GetCapacityIDAsync(id);
        }

        public async Task<int> GetRemainingCapacityIDAsync(int trainingID)
        {
            return await _trainingDAL.GetRemainingCapacityIDAsync(trainingID);
        }

        public async Task<TrainingViewModel> GetTrainingViewModelDetailsWithDepartmentsAndPrerequisitesAsync()
        {
            var departments = await _departmentDAL.GetAllAsync();
            var prerequisites = await _prerequisiteDAL.GetAllAsync();

            var trainingViewModel = new TrainingViewModel
            {
                Prerequisites = prerequisites.ToList(),
                Departments = departments.ToList(),
            };

            return trainingViewModel;
        }

        public async Task<TrainingViewModel> GetTrainingWithPrerequisitesAsync(int trainingId)
        {
            return await _trainingDAL.GetTrainingWithPrerequisitesAsync(trainingId);
        }

        public async Task<bool> IsTrainingNameAlreadyExistsAsync(string trainingName)
        {
            return await _trainingDAL.IsTrainingNameAlreadyExistsAsync(trainingName);
        }

        public async Task<bool> IsTrainingNameAlreadyExistsOnUpdateAsync(int trainingId, string newTrainingName)
        {
            return await _trainingDAL.IsTrainingNameAlreadyExistsOnUpdateAsync(trainingId, newTrainingName);
        }

        public async Task UpdateAsync(TrainingViewModel training)
        {
            await _trainingDAL.UpdateAsync(training);
        }








        public void Add(TrainingViewModel training)
        {
            _trainingDAL.Add(training);
        }

        public bool Delete(int id)
        {
            return _trainingDAL.Delete(id);
        }

        public IEnumerable<TrainingModel> GetAll()
        {
            return _trainingDAL.GetAll();
        }

        public IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline, bool isCronJob)
        {
            return _trainingDAL.GetAllTrainingsByRegistrationDeadline(registrationDeadline, isCronJob);
        }

        public IEnumerable<TrainingModel> GetAllTrainingsEnrolledByUser(int id)
        {
            return _trainingDAL.GetAllTrainingsEnrolledByUser(id);
        }

        public IEnumerable<TrainingModel> GetAllTrainingsNotEnrolledByUser(int id)
        {
            return _trainingDAL.GetAllTrainingsNotEnrolledByUser(id);
        }

        public IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsites()
        {
            return _trainingDAL.GetAllTrainingWithPrerequsites();
        }

        public TrainingModel GetByID(int id)
        {
            return _trainingDAL.GetByID(id);
        }

        public int GetCapacityID(int id)
        {
            return _trainingDAL.GetCapacityID(id);
        }

        public int GetRemainingCapacityID(int trainingID)
        {
            return _trainingDAL.GetRemainingCapacityID(trainingID);
        }

        public TrainingViewModel GetTrainingViewModelDetailsWithDepartmentsAndPrerequsites()
        {
            var departments = _departmentDAL.GetAll();
            var prerequisites = _prerequisiteDAL.GetAll();

            var trainingViewModel = new TrainingViewModel
            {   
                Prerequisites = prerequisites.ToList(),
                Departments = departments.ToList(),      
            };

            return trainingViewModel;
        }

        public TrainingViewModel GetTrainingWithPrerequisites(int trainingId)
        {
            return _trainingDAL.GetTrainingWithPrerequisites(trainingId);
        }

        public bool IsTrainingNameAlreadyExists(string trainingName)
        {
            return _trainingDAL.IsTrainingNameAlreadyExists(trainingName);
        }

        public bool IsTrainingNameAlreadyExistsOnUpdate(int trainingId, string newTrainingName)
        {
            return _trainingDAL.IsTrainingNameAlreadyExistsOnUpdate(trainingId, newTrainingName);
        }

        public void Update(TrainingViewModel training)
        {
            _trainingDAL.Update(training);
        }
    }
}
