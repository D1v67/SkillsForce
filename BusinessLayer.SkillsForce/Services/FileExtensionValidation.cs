using BusinessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class FileExtensionValidation : IFileExtensionValidation
    {
        public bool Validate(string fileName, IList<string> allowedFileExtensions)
        {
            var fileExtension = Path.GetExtension(fileName);
            return allowedFileExtensions.Contains(fileExtension);
        }
    }
}
