using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IAccountService
    {
        bool IsUserAuthenticated(AccountModel model);
        AccountModel GetUserDetailsWithRoles(AccountModel model);
        ValidationResult RegisterUser(RegisterViewModel model);
    }
}
