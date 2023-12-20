using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;

namespace BusinessLayer.SkillsForce.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserDAL _userDAL;
        private readonly IAccountDAL _loginDAL;

        public AccountService(IUserDAL userDAL, IAccountDAL loginDAL)
        {
            _userDAL = userDAL;
            _loginDAL = loginDAL;
        }
        public bool IsUserAuthenticated(AccountModel model)
        {
            return _loginDAL.IsUserAuthenticated(model);
        }

        public AccountModel GetUserDetailsWithRoles(AccountModel model)
        {
            return _loginDAL.GetUserDetailsWithRoles(model);
        }

        public void RegisterUser(RegisterViewModel model)
        {
            var hashedPassword = PasswordHasher.HashPassword(model.Password);
            model.HashedPassword = hashedPassword.Item1;
            model.SaltValue = hashedPassword.Item2;

            _loginDAL.Register(model);
        }
    }
}
