using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public interface IAttachmentService
    {
        void UploadFile(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs);
        IEnumerable<AttachmentModel> GetAll();
        IEnumerable<AttachmentModel> GetAllByEnrollmentID(int id);
        AttachmentModel GetByAttachmentID(int id);
    }
}