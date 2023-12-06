using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.ViewModel
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public int DepartmentID { get; set; }
        public int ManagerID { get; set; }
        public string Password { get; set; }
        public IEnumerable<UserModel> ListOfManagers { get; set; }
        public IEnumerable<DepartmentModel> ListOfDepartments { get; set; }
    }
}
