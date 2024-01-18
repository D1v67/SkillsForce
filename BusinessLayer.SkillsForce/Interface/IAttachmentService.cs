using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentModel>> GetAllAsync();
        Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id);
        Task<AttachmentModel> GetByAttachmentIDAsync(int id);
        Task<ValidationResult> UploadFileAsync(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs);
    }
}