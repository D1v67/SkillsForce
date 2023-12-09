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
    public class UploaderService : IUploaderService
    {
        private readonly IAttachmentDAL _attachmentDAL;

        public UploaderService(IAttachmentDAL attachmentDAL)
        {
            _attachmentDAL = attachmentDAL;
        }
        public void UploadFile(List<HttpPostedFileBase> files, int EnrollmentID, int PrerequisiteID, int UserID)
        {
            var result = new AttachmentModel();

            if (files != null && files.Any())
            {
                foreach (var item in files)
                {
                    if (item.ContentLength > 0)
                    {
                        using (var reader = new BinaryReader(item.InputStream))
                        {
                            var fileData = reader.ReadBytes(item.ContentLength);
                            var attachment = new AttachmentModel()
                            {
                                FileName = item.FileName,
                                EnrollmentID = EnrollmentID,
                                FileData = fileData,
                                PrerequisiteID = PrerequisiteID,
                      
                            };

                            _attachmentDAL.Add(attachment);
                            // result = Add(evidence);
                        }
                    }
                }
            }

           
        }
    }
}
