using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;
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
            return await _accountDAL.IsUserAuthenticatedAsync(model);
        }

        public async Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel model)
        {
            return await _accountDAL.GetUserDetailsWithRolesAsync(model);
        }

        public async Task<ValidationResult> RegisterUserAsync(RegisterViewModel model)
        {
            var validationErrors = new List<string>();

            if (await _userService.IsEmailAlreadyExistsAsync(model.Email))
            {
                validationErrors.Add("Email is already in use.");
            }

            if (await _userService.IsNICExistsAsync(model.NIC))
            {
                validationErrors.Add("NIC is already in use.");
            }

            if (await _userService.IsMobileNumberExistsAsync(model.MobileNumber))
            {
                validationErrors.Add("Mobile Number is already in use.");
            }

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
    }
}
