using Common.SkillsForce.Entity;
using System.Collections.Generic;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface ISessionService
    {
        void SetUserSession(AccountModel userDetails);
        string GetCurrentRole(List<string> userRoles);
    }
}
