using System;

namespace Common.SkillsForce.ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; }
    }
}
