using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IPrerequisiteService
    {
        Task<IEnumerable<PrerequisiteModel>> GetPrerequisiteByTrainingIDAsync(int TrainingID);
        Task<IEnumerable<PrerequisiteModel>> GetAllAsync();
    }
}
