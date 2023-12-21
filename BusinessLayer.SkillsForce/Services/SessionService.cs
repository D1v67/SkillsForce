using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer.SkillsForce.Services
{
    public class SessionService : ISessionService
    {
        private readonly HttpSessionStateBase _session;

        public SessionService(HttpSessionStateBase session)
        {
            _session = session;
        }

        public void SetUserSession(AccountModel userDetails)
        {
            _session["UserID"] = userDetails.UserID;
            _session["Email"] = userDetails.Email;
            _session["FirstName"] = userDetails.FirstName;
        }

        public string GetCurrentRole(List<string> userRoles)
        {
            return userRoles.Count == 1 ? userRoles[0] : null;
        }
    }
}
