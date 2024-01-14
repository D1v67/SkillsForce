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
        public string DepartmentName { get; set; }  //User Department
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; }
        public string DeclineReason { get; set; }
        public string TrainingDepartmentName { get; set; }//Training Department}
        public bool IsSelected { get; set; }
        public int ApprovedByUserId { get; set; }
        public int DeclinedByUserId { get; set; }
        public DateTime TrainingRegistrationDeadline { get; set; }
        public DateTime TrainingStartDate { get; set; }

        public int? rejectedEnrollmentID { get; set; }
    }
}
