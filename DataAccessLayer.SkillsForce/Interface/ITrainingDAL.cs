using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface ITrainingDAL
    {
        IEnumerable<TrainingModel> GetAll();
        TrainingModel GetByID(int id);
        void Add(TrainingModel training);
        void Delete(int id);
        void Update(TrainingModel training);
    }
}
