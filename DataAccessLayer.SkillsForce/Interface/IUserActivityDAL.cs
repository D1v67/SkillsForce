using Common.SkillsForce.Entity;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public interface IUserActivityDAL
    {
        Task<bool> AddUserActivity(UserActivityModel userActivity);
        Task<bool> AddUserLoginActivity(UserActivityModel userActivity);
    }
}