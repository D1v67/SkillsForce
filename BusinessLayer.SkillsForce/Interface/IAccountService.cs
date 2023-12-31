using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IAccountService
    {
        bool IsUserAuthenticated(AccountModel model);
        AccountModel GetUserDetailsWithRoles(AccountModel model);
        bool RegisterUser(RegisterViewModel model, out List<string> validationErrors);
    }
}
