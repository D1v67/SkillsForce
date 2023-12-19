using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream stream, int trainingId, string fileName);
    }
}