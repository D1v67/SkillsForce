using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{   
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IDepartmentService _departmentService;
        private readonly IAppNotificationService _appNotificationService;

        public AccountController(IUserService userService, IAccountService accountService, IDepartmentService departmentService, IAppNotificationService appNotificationService)
        {
            _userService = userService;
            _accountService = accountService;
            _departmentService = departmentService;    
            _appNotificationService = appNotificationService;
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [LoginActivityFilter]
        public async Task<JsonResult> Authenticate(AccountModel account)
        {           
            bool isUserValid = await _accountService.IsUserAuthenticatedAsync(account);
                if (isUserValid)
                {
                    var userDetailsWithRoles = await _accountService.GetUserDetailsWithRolesAsync(account);
                    await SetSessionVariables(userDetailsWithRoles);
                    SetUserRolesInSession(userDetailsWithRoles.listOfRoles);
                    var (redirectController, redirectAction) = GetRedirectInfo(userDetailsWithRoles.listOfRoles);
                    return Json(new { result = isUserValid, url = Url.Action(redirectAction, redirectController) });
                }
            return Json(new { result = false, url = Url.Action("Login", "Account") });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterViewModel registerViewModel)
        {
            var result = await _accountService.RegisterUserAsync(registerViewModel);
            if (result.IsSuccessful)
            {
                return Json(new { url = Url.Action("Index", "Account") });
            }
            AddErrorsToModelState(result.Errors);
            var errors = GetModelStateErrors();
            return Json(new { errorMessage = errors });
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [LogoutActivityFilter]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> GetDepartments()
        {
            IEnumerable<DepartmentModel> departments = await _departmentService.GetAllAsync();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetManagers()
        {
            IEnumerable<UserModel> managers = await _userService.GetAllManagerAsync();
            return Json(managers, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.RoleSelection)]
        public ActionResult RoleSelection()
        {
            List<string> userRoles = (List<string>)Session["UserRoles"];
            return View(userRoles);
        }

        [HttpPost]
        public ActionResult SetRole(string selectedRole)
        {
            Session["CurrentRole"] = selectedRole;
            return RedirectToAction("Index", "Home");
        }

        private async  Task SetSessionVariables(AccountModel userDetailsWithRoles)
        {
            //HttpContext.Response.Cache.AppendCacheExtension("no-cache=\"Set-Cookie\"");
            Session["UserID"] = userDetailsWithRoles.UserID;
            Session["Email"] = userDetailsWithRoles.Email;
            Session["FirstName"] = userDetailsWithRoles.FirstName;
            Session["LastName"] = userDetailsWithRoles.LastName;
            Session["CurrentRole"] = userDetailsWithRoles.listOfRoles.Count == 1 ? userDetailsWithRoles.listOfRoles[0].RoleName : null;

            int userId = userDetailsWithRoles.UserID;
            int unreadNotificationCount = await _appNotificationService.GetUnreadNotificationCountAsync(userId);
            Session["UnreadNotificationCount"] = unreadNotificationCount;
        }

        private void SetUserRolesInSession(List<UserRoleModel> userRoles)
        {
            Session["UserRoles"] = userRoles.Select(r => r.RoleName).ToList();
        }

        private (string redirectController, string redirectAction) GetRedirectInfo(List<UserRoleModel> userRoles)
        {
            var redirectController = userRoles.Count == 1 ? "Home" : "Account";
            var redirectAction = userRoles.Count == 1 ? "Index" : "RoleSelection";
            return (redirectController, redirectAction);
        }

        private void AddErrorsToModelState(List<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private List<string> GetModelStateErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            List<string> userRoles = (List<string>)Session["UserRoles"];
            return Json(userRoles, JsonRequestBehavior.AllowGet);
        }
    }
}
