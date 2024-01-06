using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Entity
{
    public class AppNotificationModel
    {
        public int AppNotificationID { get; set; }
        public int UserID { get; set; }
        public int EnrollmentID { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationMessage { get; set; }
        public string Status { get; set; }
        public bool HasRead { get; set; }

        public DateTime CreateTimeStamp { get; set; }

        public string SenderName { get; set; }
    }
}
