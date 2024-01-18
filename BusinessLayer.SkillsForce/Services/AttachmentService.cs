using BusinessLayer.SkillsForce.Interface;
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
        private readonly IFileExtensionValidation _fileExtensionValidation;

        public AttachmentService(IAttachmentDAL attachmentDAL, IFileExtensionValidation fileExtensionValidation) =>
            (_attachmentDAL, _fileExtensionValidation) = (attachmentDAL, fileExtensionValidation);

        public async Task<IEnumerable<AttachmentModel>> GetAllAsync() => await _attachmentDAL.GetAllAsync();

        public async Task<AttachmentModel> GetByAttachmentIDAsync(int id) => await _attachmentDAL.GetByAttachmentIDAsync(id);

        public async Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id) =>
            await _attachmentDAL.GetAllByEnrollmentIDAsync(id);

        public async Task<ValidationResult> UploadFileAsync(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
        {
            var validationErrors = new List<string>();
            bool isValid = true;

            if (!string.IsNullOrEmpty(PrerequisiteIDs))
            {
                var prerequisiteIdArray = PrerequisiteIDs.Split(',');

                for (int i = 0; i < files.Count; i++)
                {
                    var item = files[i];

                    if (item.ContentLength > 0 && IsPdfFile(item.FileName))
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
                    else
                    {
                        isValid = false;
                        validationErrors.Add($"Invalid file: {item.FileName}. Only PDF files are allowed.");
                    }
                }
            }
            return new ValidationResult { IsSuccessful = isValid, Errors = validationErrors };
        }

        private bool IsPdfFile(string fileName)
        {
            var allowedExtensions = new List<string> { ".pdf" };
            return _fileExtensionValidation.Validate(fileName, allowedExtensions);
        }

        private bool IsValidPdf(HttpPostedFileBase file)
        {
            return file.ContentType.ToLower() == "application/pdf" && file.FileName.ToLower().EndsWith(".pdf");
        }
    }
}
