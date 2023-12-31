using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Helpers
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }
    }
}
