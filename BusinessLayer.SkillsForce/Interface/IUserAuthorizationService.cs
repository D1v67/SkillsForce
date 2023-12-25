using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationService
    {
        bool IsUserHavePermission(int userID, string permission);
    }
}
