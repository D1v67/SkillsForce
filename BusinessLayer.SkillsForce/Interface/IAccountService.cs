using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IAccountService
    {
        Task<bool> IsUserAuthenticatedAsync(AccountModel model);
        Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel model);
        Task<ValidationResult> RegisterUserAsync(RegisterViewModel model);
    }
}
