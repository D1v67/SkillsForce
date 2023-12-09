using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public interface IUploaderService
    {
        void UploadFile(List<HttpPostedFileBase> files, int EnrollmentID, int TrainingID, int UserID);
    }
}