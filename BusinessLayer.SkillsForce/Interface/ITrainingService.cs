using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface ITrainingService
    {
        IEnumerable<TrainingModel> GetAll();
        TrainingModel GetByID(int id);
        void Add(TrainingModel training);
        void Delete(int id);
        void Update(TrainingModel training);
    }
}
