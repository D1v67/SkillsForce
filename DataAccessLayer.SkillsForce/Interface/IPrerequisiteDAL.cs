using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IPrerequisiteDAL
    {
        Task<IEnumerable<PrerequisiteModel>> GetPrerequisiteByTrainingIDAsync(int TrainingID);
        Task<IEnumerable<PrerequisiteModel>> GetAllAsync();
    }
}
