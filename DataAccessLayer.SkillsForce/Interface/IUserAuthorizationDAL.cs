
namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationDAL
    {
        bool IsUserHavePermission(int userID, string permission);
    }
}
