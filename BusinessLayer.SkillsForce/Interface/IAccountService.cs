using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IAccountService
    {
        bool IsUserAuthenticated(AccountModel model);
        AccountModel GetUserDetailsWithRoles(AccountModel model);
        void RegisterUser(RegisterViewModel model);
    }
}
