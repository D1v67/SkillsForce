using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MVC.SkillsForce.Custom
{
    /// <summary>
    /// Utility class for managing user-related information in session variables within an ASP.NET application.
    /// Provides methods to set and retrieve session values such as user ID, email, name, roles, and unread notification count.
    /// Utilizes generic methods for flexibility in handling session values of different types.
    /// Interacts with an external service (IAppNotificationService) to update notification session data asynchronously.
    /// </summary>
    public class SessionManager
    {

        #region Session Keys

        private const string USER_ID_KEY = "UserID";
        private const string EMAIL_KEY = "Email";
        private const string FIRST_NAME_KEY = "FirstName";
        private const string LAST_NAME_KEY = "LastName";
        private const string CURRENT_ROLE_KEY = "CurrentRole";
        private const string UNREAD_NOTIFICATION_COUNT_KEY = "UnreadNotificationCount";
        private const string USER_ROLES_KEY = "UserRoles";

        #endregion

        private readonly IAppNotificationService _appNotificationService;

        public SessionManager( IAppNotificationService appNotificationService)
        {
            _appNotificationService = appNotificationService;
        }

        #region Set Methods

        public void SetUserId(int userId) => SetSessionValue(USER_ID_KEY, userId);
        public void SetEmail(string email) => SetSessionValue(EMAIL_KEY, email);
        public void SetFirstName(string firstName) => SetSessionValue(FIRST_NAME_KEY, firstName);
        public void SetLastName(string lastName) => SetSessionValue(LAST_NAME_KEY, lastName);
        public void SetCurrentRole(string currentRole) => SetSessionValue(CURRENT_ROLE_KEY, currentRole);
        public void SetUnreadNotificationCount(int count) => SetSessionValue(UNREAD_NOTIFICATION_COUNT_KEY, count);
        public void SetUserRoles(List<string> userRoles) => SetSessionValue(USER_ROLES_KEY, userRoles);

        #endregion

        #region Get Methods

        public int GetUserId() => GetSessionValue<int>(USER_ID_KEY);
        public string GetEmail() => GetSessionValue<string>(EMAIL_KEY);
        public string GetFirstName() => GetSessionValue<string>(FIRST_NAME_KEY);
        public string GetLastName() => GetSessionValue<string>(LAST_NAME_KEY);
        public string GetCurrentRole() => GetSessionValue<string>(CURRENT_ROLE_KEY);
        public int GetUnreadNotificationCount() => GetSessionValue<int>(UNREAD_NOTIFICATION_COUNT_KEY);
        public List<string> GetUserRoles() => GetSessionValue<List<string>>(USER_ROLES_KEY);

        #endregion

        private void SetSessionValue(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        private T GetSessionValue<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }

        public async Task SetSessionVariables(AccountModel userDetailsWithRoles)
        {
            SetUserId(userDetailsWithRoles.UserID);
            SetEmail(userDetailsWithRoles.Email);
            SetFirstName(userDetailsWithRoles.FirstName);
            SetLastName(userDetailsWithRoles.LastName);
            SetCurrentRole(userDetailsWithRoles.listOfRoles.Count == 1 ? userDetailsWithRoles.listOfRoles[0].RoleName : null);

            int userId = userDetailsWithRoles.UserID;
            int unreadNotificationCount = await _appNotificationService.GetUnreadNotificationCountAsync(userId);
            SetUnreadNotificationCount(unreadNotificationCount);
        }

        public void SetUserRolesInSession(List<UserRoleModel> userRoles)
        {
            SetUserRoles(userRoles.Select(r => r.RoleName).ToList());
        }

    }
}