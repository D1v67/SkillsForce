using System;
using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class TrainingModel
    {
        [Key]
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDescription { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int Capacity { get; set; }
        public int DepartmentID { get; set; }
        public DateTime StartDate { get; set; }
        public string DepartmentName { get; set; }
        public bool IsSelectionOver { get; set; }
    }
}
