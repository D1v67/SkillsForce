using Common.SkillsForce.Entity;
using Common.SkillsForce.Helpers;
using DataAccessLayer.SkillsForce.DAL;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IEnumerable<AttachmentModel>> GetAllAsync()
        {
            return await _attachmentDAL.GetAllAsync();
        }

        public async Task<AttachmentModel> GetByAttachmentIDAsync(int id)
        {
            return await _attachmentDAL.GetByAttachmentIDAsync(id);
        }

        public async Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id)
        {
            return await _attachmentDAL.GetAllByEnrollmentIDAsync(id);
        }

        public async Task UploadFileAsync(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
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
                                FileName = item.FileName,
                                EnrollmentID = EnrollmentID,
                                FileData = fileData,
                                PrerequisiteID = prerequisiteId,
                            };
                            await _attachmentDAL.AddAsync(attachment);
                        }
                    }
                }
            }

        }

        //public async Task<ValidationResult> UploadFileAsync(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
        //{
        //    var validationResult = new ValidationResult
        //    {
        //        IsSuccessful = true,
        //        Errors = new List<string>()
        //    };

        //    if (!string.IsNullOrEmpty(PrerequisiteIDs))
        //    {
        //        var prerequisiteIdArray = PrerequisiteIDs.Split(',');

        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            var item = files[i];

        //            if (item.ContentLength > 0)
        //            {
        //                // Perform file type validation
        //                if (!IsValidPdf(item))
        //                {
        //                    // Add an error message to the ValidationResult
        //                    validationResult.IsSuccessful = false;
        //                    validationResult.Errors.Add("Invalid file type. Only PDF files are allowed.");
        //                    continue; // Skip processing the current file if it's invalid
        //                }

        //                using (var reader = new BinaryReader(item.InputStream))
        //                {
        //                    var fileData = reader.ReadBytes(item.ContentLength);
        //                    int prerequisiteId = int.Parse(prerequisiteIdArray[i]);
        //                    var attachment = new AttachmentModel()
        //                    {
        //                        FileName = item.FileName,
        //                        EnrollmentID = EnrollmentID,
        //                        FileData = fileData,
        //                        PrerequisiteID = prerequisiteId,
        //                    };

        //                    await _attachmentDAL.AddAsync(attachment);
        //                }
        //            }
        //        }
        //    }

        //    return validationResult;
        //}

        private bool IsValidPdf(HttpPostedFileBase file)
        {
            // Add your file type validation logic here
            return file.ContentType.ToLower() == "application/pdf" && file.FileName.ToLower().EndsWith(".pdf");
        }
    }
}
