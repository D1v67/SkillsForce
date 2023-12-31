using BusinessLayer.SkillsForce.Interface;
using DataAccessLayer.SkillsForce.DAL;

namespace BusinessLayer.SkillsForce.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly UserAuthorizationDAL _useAuthorizationDAL;

        public UserAuthorizationService(UserAuthorizationDAL useAuthorizationDAL)
        {
            _useAuthorizationDAL = useAuthorizationDAL;
        }
        public bool IsUserHavePermission(int userID, string permission)
        {
            return _useAuthorizationDAL.IsUserHavePermission(userID, permission);
        }
    }
}
