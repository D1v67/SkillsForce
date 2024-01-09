using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Enums
{
    public enum Permissions
    {
        AddTraining,
        DeleteTraining,
        EditTraining,
        GetTraining,
        ViewTraining,

        GetEnrollment,
        ApproveEnrollment,
        RejectEnrollment,
        ConfirmEnrollment,
        SaveEnrollment,
        GetEnrollmentByManager,

        GetUser,
        AddUser,
        DeleteUser,
        EditUser,

        GetDepartment,
        AddDepartment,
        EditDepartment,
        DeleteDepartment,

        GetPrerequisite,
        AddPrerequisite,
        EditPrerequisite,
        DeletePrerequisite,

        GetAttachment,

        RoleSelection,

        SessionManagement,
        AdminDashboard,
        NotificationHandling,
        AutomaticSelection,

        DownloadAttachment,
        ViewAttachment,
        FileManagement,

        ViewConfirmedEnrollments,

        ViewNotification,
        GetAllNotification,

        ViewTrainingDetails
    }
}
