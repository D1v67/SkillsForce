using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IAccountDAL
    {
        //Task<bool> IsUserAuthenticatedAsync(AccountModel model);
        Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<AccountModel> GetUserCredentialsAsync(string email);
    }
}
