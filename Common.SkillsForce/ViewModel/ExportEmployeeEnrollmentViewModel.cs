
namespace Common.SkillsForce.ViewModel
{
    public class ExportEmployeeEnrollmentViewModel
    {
   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }

        public int ManagerID { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
