
namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationService
    {
        bool IsUserHavePermission(int userID, string permission);
    }
}
