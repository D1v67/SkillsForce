using System;

namespace Common.SkillsForce.ViewModel
{
    public class EnrollmentNotificationViewModel
    {
        public int EnrollmentID { get; set; }
        public int AppUserID { get; set; }
        public string AppUserFirstName { get; set; }
        public string AppUserLastName { get; set; }
        public string AppUserEmail { get; set; }
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; }
        public int ManagerID { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string DeclineReason { get; set;}
        public bool IsSelected { get; set; }
    }
}
