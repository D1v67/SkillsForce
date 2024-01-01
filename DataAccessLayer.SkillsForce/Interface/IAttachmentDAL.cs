using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public interface IAttachmentDAL
    {
        Task AddAsync(AttachmentModel attachment);
        Task<IEnumerable<AttachmentModel>> GetAllAsync();
        Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id);
        Task<AttachmentModel> GetByAttachmentIDAsync(int id);
    }
}