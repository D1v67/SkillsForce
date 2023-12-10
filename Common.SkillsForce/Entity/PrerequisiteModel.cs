using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Entity
{
    public class PrerequisiteModel
    {
        [Key]
        public int PrerequisiteID { get; set; }
        public string PrerequisiteName { get; set; }
    }
}
