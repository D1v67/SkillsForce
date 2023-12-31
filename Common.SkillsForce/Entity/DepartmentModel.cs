using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
