using System.Collections.Generic;

namespace Common.SkillsForce.Helpers
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }
    }
}
