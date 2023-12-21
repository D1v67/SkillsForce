using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface ISessionService
    {
        void SetUserSession(AccountModel userDetails);
        string GetCurrentRole(List<string> userRoles);
    }
}
