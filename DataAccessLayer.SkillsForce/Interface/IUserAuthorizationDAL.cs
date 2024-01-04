
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationDAL
    {
        Task<bool> IsUserHavePermissionAsync(int userID, string permission);
    }
}
