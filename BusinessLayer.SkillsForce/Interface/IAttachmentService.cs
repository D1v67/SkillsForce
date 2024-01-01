using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public interface IAttachmentService
    {
        Task UploadFileAsync(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs);
        Task<IEnumerable<AttachmentModel>> GetAllAsync();
        Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id);
        Task<AttachmentModel> GetByAttachmentIDAsync(int id);
    }
}