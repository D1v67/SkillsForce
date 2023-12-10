using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentDAL _attachmentDAL;

        public AttachmentService(IAttachmentDAL attachmentDAL)
        {
            _attachmentDAL = attachmentDAL;
        }

        public IEnumerable<AttachmentModel> GetAll()
        {
            return _attachmentDAL.GetAll();
        }

        public AttachmentModel GetByAttachmentID(int id)
        {
            return _attachmentDAL.GetByAttachmentID(id);
        }

        public IEnumerable<AttachmentModel> GetAllByEnrollmentID(int id)
        {
            return _attachmentDAL.GetAllByEnrollmentID(id);
        }

        public void UploadFile(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
        {
            if (!string.IsNullOrEmpty(PrerequisiteIDs))
            {             
                var prerequisiteIdArray = PrerequisiteIDs.Split(',');
               
                for (int i = 0; i < files.Count; i++)
                {
                    var item = files[i];

                    if (item.ContentLength > 0)
                    {
                        using (var reader = new BinaryReader(item.InputStream))
                        {
                            var fileData = reader.ReadBytes(item.ContentLength);
                            int prerequisiteId = int.Parse(prerequisiteIdArray[i]);
                            var attachment = new AttachmentModel()
                            {
                                //FileName = item.FileName,
                                EnrollmentID = EnrollmentID,
                                FileData = fileData,
                                PrerequisiteID = prerequisiteId,
                            };
                            _attachmentDAL.Add(attachment);
                        }
                    }
                }
            }

        }
    }
}
