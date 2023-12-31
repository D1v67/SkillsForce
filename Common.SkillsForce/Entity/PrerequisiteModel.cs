using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class PrerequisiteModel
    {
        [Key]
        public int PrerequisiteID { get; set; }
        public string PrerequisiteName { get; set; }
    }
}
