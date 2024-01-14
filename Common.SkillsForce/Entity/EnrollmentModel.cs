using System;
using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class EnrollmentModel
    {
        [Key]
        public int EnrollmentID { get; set; }
        public int UserID { get; set; }
        public int TrainingID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; }

    }
}
