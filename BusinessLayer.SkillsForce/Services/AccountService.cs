using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserDAL _userDAL;
        private readonly IAccountDAL _loginDAL;
        private readonly IUserService _userService;

        public AccountService(IUserDAL userDAL, IAccountDAL loginDAL, IUserService userService)
        {
            _userDAL = userDAL;
            _loginDAL = loginDAL;
            _userService = userService;
        }
        public bool IsUserAuthenticated(AccountModel model)
        {
            return _loginDAL.IsUserAuthenticated(model);
        }

        public AccountModel GetUserDetailsWithRoles(AccountModel model)
        {
            return _loginDAL.GetUserDetailsWithRoles(model);
        }

        public ValidationResult RegisterUser(RegisterViewModel model)
        {
            var validationErrors = new List<string>();

            if ((_userService.IsEmailAlreadyExists(model.Email)))
            {
                validationErrors.Add("Email is already in use.");
            }

            if ((_userService.IsNICExists(model.NIC)))
            {
                validationErrors.Add("NIC is already in use.");
            }

            if (_userService.IsMobileNumberExists(model.MobileNumber))
            {
                validationErrors.Add("Mobile Number is already in use.");
            }
            if (validationErrors.Count == 0)
            {
                var hashedPassword = PasswordHasher.HashPassword(model.Password);
                model.HashedPassword = hashedPassword.Item1;
                model.SaltValue = hashedPassword.Item2;

                _loginDAL.Register(model);
                return new ValidationResult { IsSuccessful = true };
            }

            return new ValidationResult { IsSuccessful = false, Errors = validationErrors };
        }

    }
}
