using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IAccountService
    {
        bool IsUserAuthenticated(AccountModel model);
        AccountModel GetUserDetailsWithRoles(AccountModel model);
        void RegisterUser(RegisterViewModel model);
    }
}
