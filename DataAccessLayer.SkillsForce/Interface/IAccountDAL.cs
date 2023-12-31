using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IAccountDAL
    {
        bool IsUserAuthenticated(AccountModel model);
        AccountModel GetUserDetailsWithRoles(AccountModel model);
        void Register(RegisterViewModel model);
    }
}
