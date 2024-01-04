
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationService
    {
        Task<bool> IsUserHavePermissionAsync(int userID, string permission);
    }
}
