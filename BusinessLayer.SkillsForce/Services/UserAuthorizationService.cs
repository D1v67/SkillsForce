using BusinessLayer.SkillsForce.Interface;
using DataAccessLayer.SkillsForce.DAL;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly UserAuthorizationDAL _useAuthorizationDAL;

        public UserAuthorizationService(UserAuthorizationDAL useAuthorizationDAL)
        {
            _useAuthorizationDAL = useAuthorizationDAL;
        }
        public async Task<bool> IsUserHavePermissionAsync(int userID, string permission)
        {
            return await _useAuthorizationDAL.IsUserHavePermissionAsync(userID, permission);
        }
    }
}
