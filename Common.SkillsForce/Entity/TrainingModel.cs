using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Entity
{
    public class TrainingModel
    {
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDescription { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int Capacity { get; set; }
        public int DepartmentID { get; set; }

    }
}
