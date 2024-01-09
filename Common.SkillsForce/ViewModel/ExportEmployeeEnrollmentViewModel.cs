using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.ViewModel
{
    public class ExportEmployeeEnrollmentViewModel
    {
        //public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string NIC { get; set; } //annotation
        public string MobileNumber { get; set; }
        //public int RoleID { get; set; }
        public int ManagerID { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
