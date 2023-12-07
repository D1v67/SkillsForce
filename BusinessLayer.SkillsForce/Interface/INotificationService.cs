using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface INotificationService
    {
        string SendNotification(EnrollmentNotificationViewModel enrollment);
    }
}
