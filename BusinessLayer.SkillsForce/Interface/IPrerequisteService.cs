using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IPrerequisiteService
    {
        IEnumerable<PrerequisiteModel> GetPrerequisiteByTrainingID(int TrainingID);
        IEnumerable<PrerequisiteModel> GetAll();
        PrerequisiteModel GetByID(int id);
        void Add(PrerequisiteModel prerequisite);
        void Delete(int id);
        void Update(PrerequisiteModel prerequisite);
    }
}
