using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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
