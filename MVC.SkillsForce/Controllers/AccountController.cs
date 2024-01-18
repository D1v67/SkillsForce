﻿using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System.Collections.Generic;
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
        private readonly SessionManager _sessionManager;

        public AccountController(IUserService userService, IAccountService accountService, IDepartmentService departmentService, IAppNotificationService appNotificationService)
        {
            _userService = userService;
            _accountService = accountService;
            _departmentService = departmentService;    
            _appNotificationService = appNotificationService;
            _sessionManager = new SessionManager( _appNotificationService);
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
                    await _sessionManager.SetSessionVariables(userDetailsWithRoles);
                    _sessionManager.SetUserRolesInSession(userDetailsWithRoles.listOfRoles);
                    await _sessionManager.SetSessionVariables(userDetailsWithRoles);
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

        [AuthorizePermission(Permissions.RoleSelection)]
        public ActionResult RoleSelection()
        {
            List<string> userRoles = _sessionManager.GetUserRoles();
            return View(userRoles);
        }

        [HttpPost]
        public ActionResult SetRole(string selectedRole)
        {
            _sessionManager.SetCurrentRole(selectedRole);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            List<string> userRoles = _sessionManager.GetUserRoles();
            return Json(userRoles, JsonRequestBehavior.AllowGet);
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

    }
}
