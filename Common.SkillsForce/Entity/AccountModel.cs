using System.Collections.Generic;

namespace Common.SkillsForce.Entity
{
    public class AccountModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public List<UserRoleModel> listOfRoles { get; set; }

        public byte[] SaltValue { get; set; }

        public byte[] StringPassword { get; set; }

        public byte[] HashedPassword { get; set; }
    }
}
