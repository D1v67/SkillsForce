using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "National Identity Card (NIC)")]
        public string NIC { get; set; } 

        public string MobileNumber { get; set; }

        public int RoleID { get; set; }

        public int? ManagerID { get; set; }

        public int DepartmentID { get; set; }

    }
}
