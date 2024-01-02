using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.ModelBinding;

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

        public async Task<ValidationResult> AddAsync(TrainingViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate Training Name
            if (string.IsNullOrWhiteSpace(model.TrainingName) || !Regex.IsMatch(model.TrainingName, @"^[A-Z][a-z]*( [A-Z][a-z]*)*$"))
            {
                validationErrors.Add("Training Name is required and must be in a valid format.");
            }
            else if (await IsTrainingNameAlreadyExistsAsync(model.TrainingName))
            {
                validationErrors.Add("Training Name is already in use.");
            }

            // Validate Start Date and Registration Deadline
            if (!IsValidDateRange(model.StartDate, model.RegistrationDeadline))
            {
                validationErrors.Add("Start date must be greater than Registration Deadline.");
            }

            // Validate Capacity
            if (!Regex.IsMatch(model.Capacity.ToString(), @"^[1-9]\d*$"))
            {
                validationErrors.Add("Capacity must be a positive integer.");
            }


            ValidateField(model.TrainingName, "Training Name", validationErrors, minLength: 2, maxLength: 100);

            if (validationErrors.Count == 0)
            {
                await _trainingDAL.AddAsync(model);
                return new ValidationResult { IsSuccessful = true };
            }


            return new ValidationResult { IsSuccessful = false, Errors = validationErrors };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _trainingDAL.DeleteAsync(id);
        }

        public  bool Delete(int id)
        {
            return  _trainingDAL.Delete(id);
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

        public async Task<ValidationResult> UpdateAsync(TrainingViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate Training Name
            if (string.IsNullOrWhiteSpace(model.TrainingName) || !Regex.IsMatch(model.TrainingName, @"^[A-Z][a-z]*( [A-Z][a-z]*)*$"))
            {
                validationErrors.Add("Training Name is required and must be in a valid format.");
            }

            if (await IsTrainingNameAlreadyExistsOnUpdateAsync(model.TrainingID, model.TrainingName))
            {
                validationErrors.Add("Training Name is already in use.");
            }

            // Validate Start Date and Registration Deadline
            if (!IsValidDateRange(model.StartDate, model.RegistrationDeadline))
            {
                validationErrors.Add("Start date must be greater than Registration Deadline.");
            }

            // Validate Capacity
            if (!Regex.IsMatch(model.Capacity.ToString(), @"^[1-9]\d*$"))
            {
                validationErrors.Add("Capacity must be a positive integer.");
            }


            ValidateField(model.TrainingName, "Training Name", validationErrors, minLength: 2, maxLength: 100);

            if (validationErrors.Count == 0)
            {
                await _trainingDAL.UpdateAsync(model);
                return new ValidationResult { IsSuccessful = true };
            }


            return new ValidationResult { IsSuccessful = false, Errors = validationErrors };
        }

        private void ValidateField(string fieldValue, string fieldName, List<string> validationErrors, int? minLength = null, int? maxLength = null, int? fixedLength = null)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                validationErrors.Add($"{fieldName} is required.");
            }
            else
            {
                if (fixedLength.HasValue && fieldValue.Length != fixedLength.Value)
                {
                    validationErrors.Add($"{fieldName} must be exactly {fixedLength.Value} characters.");
                }
                else if (minLength.HasValue && maxLength.HasValue && (fieldValue.Length < minLength.Value || fieldValue.Length > maxLength.Value))
                {
                    validationErrors.Add($"{fieldName} must be between {minLength.Value} and {maxLength.Value} characters.");
                }
            }
        }

        // Helper method to validate date range
        private bool IsValidDateRange(string startDate, string registrationDeadline)
        {
            if (DateTime.TryParse(startDate, out var start) && DateTime.TryParse(registrationDeadline, out var deadline))
            {
                return start > deadline && start > DateTime.Now && deadline > DateTime.Now;
            }
            return false;
        }


        public enum ValidationType
        {
            FixedLength,
            Range,
            Both
        }
    }
}
