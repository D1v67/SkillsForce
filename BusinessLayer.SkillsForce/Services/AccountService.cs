using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDAL _accountDAL;
        private readonly IUserService _userService;

        public AccountService(IAccountDAL accountDAL, IUserService userService)
        {
            _accountDAL = accountDAL;
            _userService = userService;
        }

        public async Task<bool> IsUserAuthenticatedAsync(AccountModel model)
        {
            // return await _accountDAL.IsUserAuthenticatedAsync(model);

            AccountModel storedAccount = await _accountDAL.GetUserCredentialsAsync(model.Email);

            if (storedAccount != null)
            {
                return PasswordHasher.VerifyPassword(model.Password, storedAccount.HashedPassword, storedAccount.SaltValue);
            }

            return false;
        }

        public async Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel model)
        {
            return await _accountDAL.GetUserDetailsWithRolesAsync(model);
        }

        public async Task<ValidationResult> RegisterUserAsync(RegisterViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate Email
            if (string.IsNullOrWhiteSpace(model.Email) || !Regex.IsMatch(model.Email, @"^[a-zA-Z0-9]{1,30}(?:[a-zA-Z0-9]*[._%+-]?[a-zA-Z0-9]+)@gmail\.com$"))
            {
                validationErrors.Add("Email is required and must be in a valid format.");
            }
            else if (await _userService.IsEmailAlreadyExistsAsync(model.Email))
            {
                validationErrors.Add("Email is already in use.");
            }

            // Validate NIC
            if (string.IsNullOrWhiteSpace(model.NIC) || !Regex.IsMatch(model.NIC, @"^[A-Za-z0-9]{14}$"))
            {
                validationErrors.Add("NIC is required and must be a valid format.");
            }
            else if (await _userService.IsNICExistsAsync(model.NIC))
            {
                validationErrors.Add("NIC is already in use.");
            }

            // Validate MobileNumber
            if (string.IsNullOrWhiteSpace(model.MobileNumber) || !Regex.IsMatch(model.MobileNumber, @"^5[0-9]{7}$"))
            {
                validationErrors.Add("Mobile Number is required and must be in a valid format.");
            }
            else if (await _userService.IsMobileNumberExistsAsync(model.MobileNumber))
            {
                validationErrors.Add("Mobile Number is already in use.");
            }

            // Validate  fields for required, min, and max length
            ValidateField(model.FirstName, "First Name", validationErrors, minLength: 2, maxLength: 50);
            ValidateField(model.LastName, "Last Name", validationErrors, minLength: 2, maxLength: 50 );
            ValidateField(model.Email, "Last Name", validationErrors, minLength: 2, maxLength: 255);
            ValidateField(model.NIC, "NIC", validationErrors, fixedLength: 14 );
            ValidateField(model.MobileNumber, "Mobile Number", validationErrors, fixedLength: 8);




            if (validationErrors.Count == 0)
            {
                var hashedPassword = PasswordHasher.HashPassword(model.Password);
                model.HashedPassword = hashedPassword.Item1;
                model.SaltValue = hashedPassword.Item2;

                await _accountDAL.RegisterAsync(model);
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

        public enum ValidationType
        {
            FixedLength,
            Range,
            Both
        }

    }
}


//public async Task<ValidationResult> RegisterUserAsync(RegisterViewModel model)
//{
//    var validationErrors = new List<string>();

//    if (await _userService.IsEmailAlreadyExistsAsync(model.Email))
//    {
//        validationErrors.Add("Email is already in use.");
//    }

//    if (await _userService.IsNICExistsAsync(model.NIC))
//    {
//        validationErrors.Add("NIC is already in use.");
//    }

//    if (await _userService.IsMobileNumberExistsAsync(model.MobileNumber))
//    {
//        validationErrors.Add("Mobile Number is already in use.");
//    }

//    if (validationErrors.Count == 0)
//    {
//        var hashedPassword = PasswordHasher.HashPassword(model.Password);
//        model.HashedPassword = hashedPassword.Item1;
//        model.SaltValue = hashedPassword.Item2;

//        await _accountDAL.RegisterAsync(model);
//        return new ValidationResult { IsSuccessful = true };
//    }

//    return new ValidationResult { IsSuccessful = false, Errors = validationErrors };
//}