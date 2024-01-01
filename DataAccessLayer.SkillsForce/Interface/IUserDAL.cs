using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IUserDAL
    {

        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<IEnumerable<UserModel>> GetAllManagerAsync();
        Task<UserModel> GetByIDAsync(int id);
        Task AddAsync(UserModel user);
        Task DeleteAsync(int id);
        Task UpdateAsync(UserModel user);
        Task<bool> IsEmailAlreadyExistsAsync(string email);
        Task<bool> IsNICExistsAsync(string nic);
        Task<bool> IsMobileNumberExistsAsync(string mobileNumber);




        IEnumerable<UserModel> GetAll();
        IEnumerable<UserModel> GetAllManager();
        UserModel GetByID(int id);
        void Add(UserModel user);
        void Delete(int id);
        void Update(UserModel user);
        bool IsEmailAlreadyExists(string email);
        bool IsNICExists(string nic);
        bool IsMobileNumberExists(string mobileNumber);
    }
}
