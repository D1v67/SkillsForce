using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.ViewModel
{
    public class TrainingEnrollmentViewModel
    {
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDescription { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int Capacity { get; set; }
        public int DepartmentID { get; set; }
        public DateTime StartDate { get; set; }
        public int EnrollmentID { get; set; }
    }
}
