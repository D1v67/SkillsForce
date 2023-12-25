using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IUserAuthorizationDAL
    {
        bool IsUserHavePermission(int userID, string permission);

    }
}
