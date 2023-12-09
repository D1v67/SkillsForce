using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Entity
{
    public class AttachmentModel
    {
        public  int  AttachmentID { get; set; }
        public int EnrollmentID { get; set; }
        public int PrerequisiteID { get; set; }
        public  string AttachmentURL  { get; set; }
        public byte[] FileData { get; set; }

        public string FileName { get; set; }
    }
}
