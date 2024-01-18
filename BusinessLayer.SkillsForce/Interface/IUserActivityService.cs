using Common.SkillsForce.Entity;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserActivityService
    {
        Task<bool> AddUserActivity(UserActivityModel userActivity);
        Task<bool> AddUserLoginActivity(UserActivityModel userActivity);
    }
}
