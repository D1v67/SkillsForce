using System.ComponentModel.DataAnnotations;

namespace Common.SkillsForce.Entity
{
    public class AttachmentModel
    {
        [Key]
        public  int  AttachmentID { get; set; }
        public int EnrollmentID { get; set; }
        public int PrerequisiteID { get; set; }
        public string PrerequisiteName { get; set; }
        public  string AttachmentURL  { get; set; }
        public byte[] FileData { get; set; }
        public string FileName { get; set; }
    }
}
