using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace BusinessLayer.SkillsForce.Services
{
    public class StorageService
    {
        private readonly FirebaseStorage _storage;

        public StorageService()
        {
            _storage = new FirebaseStorage("skillslab-7ca87.appspot.com");
        }
        public async Task<string> UploadFileAsync(Stream stream, int trainingId, string fileName)
        {
            try
            {
                var task = _storage
                    .Child($"training_{trainingId}")
                    .Child(fileName)
                    .PutAsync(stream);
                var downloadUrl = await task;
                Debug.WriteLine($"Finished uploading: {downloadUrl}");
                return downloadUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error uploading image: {ex.Message}");
                throw;
            }
        }
    }
}
