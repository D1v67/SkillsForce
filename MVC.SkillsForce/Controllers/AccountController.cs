﻿using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{    
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountService _loginService;
        private readonly IDepartmentService _departmentService;

        public AccountController(IUserService userService, IAccountService loginService, IDepartmentService departmentService)
        {
            _userService = userService;
            _loginService = loginService;
            _departmentService = departmentService;    
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Authenticate(AccountModel account)
        {
            bool isUserValid = await _loginService.IsUserAuthenticatedAsync(account);
            if (isUserValid)
            {
                var userDetailsWithRoles = await _loginService.GetUserDetailsWithRolesAsync(account);
                SetSessionVariables(userDetailsWithRoles);
                var userRoles = userDetailsWithRoles.listOfRoles.Select(r => r.RoleName).ToList();
                Session["UserRoles"] = userRoles;
                var redirectController = userRoles.Count == 1 ? "Home" : "Account";
                var redirectAction = userRoles.Count == 1 ? "Index" : "RoleSelection";

                return Json(new { result = isUserValid, url = Url.Action(redirectAction, redirectController) });
            }

            return Json(new { result = isUserValid, url = Url.Action("Login", "Account") });
        }

        private void SetSessionVariables(AccountModel userDetailsWithRoles)
        {
            Session["UserID"] = userDetailsWithRoles.UserID;
            Session["Email"] = userDetailsWithRoles.Email;
            Session["FirstName"] = userDetailsWithRoles.FirstName;
            Session["CurrentRole"] = userDetailsWithRoles.listOfRoles.Count == 1 ? userDetailsWithRoles.listOfRoles[0].RoleName : null;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterViewModel registerViewModel)
        {
            var result = await _loginService.RegisterUserAsync(registerViewModel);
            if (result.IsSuccessful)
            {
                return Json(new { url = Url.Action("Index", "Account") });
            }
            AddErrorsToModelState(result.Errors);

            var errors = GetModelStateErrors();
            return Json(new { errorMessage = errors });
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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


//public JsonResult Authenticate(AccountModel account)
//{
//    bool IsUserValid = _loginService.IsUserAuthenticated(account);
//    if (IsUserValid)
//    {
//        AccountModel userDetailsWithRoles = _loginService.GetUserDetailsWithRoles(account);
//        this.Session["UserID"] = userDetailsWithRoles.UserID;
//        //this.Session["CurrentRole"] = userDetailsWithRoles.RoleName;
//        this.Session["Email"] = userDetailsWithRoles.Email;
//        this.Session["FirstName"] = userDetailsWithRoles.FirstName;

//        List<string> userRoles = userDetailsWithRoles.listOfRoles.Select(r => r.RoleName).ToList();
//        this.Session["UserRoles"] = userRoles;

//        // If the user has only one role, set it as the CurrentRole
//        if (userRoles.Count == 1)
//        {
//            this.Session["CurrentRole"] = userRoles[0];
//        }
//    }
//    return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });
//}



//public async Task<bool> Trial()
//{
//    StorageService storage = new StorageService();
//    byte[] fileContent = Encoding.UTF8.GetBytes("This is some fake file content.");


//    using (MemoryStream stream = new MemoryStream(fileContent))
//    {

//        int fakeTrainingId = 123;

//        string fakeFileName = "fakeFile.txt";


//        string result = await storage.UploadFileAsync(stream, fakeTrainingId, fakeFileName);

//    }
//    return true;
//}

//public JsonResult Authenticate(AccountModel account)
//{
//    bool IsUserValid = _loginService.IsUserAuthenticated(account);
//    if (IsUserValid)
//    {
//        AccountModel userDetailsWithRoles = _loginService.GetUserDetailsWithRoles(account);
//        this.Session["UserID"] = userDetailsWithRoles.UserID;
//        this.Session["Email"] = userDetailsWithRoles.Email;
//        this.Session["FirstName"] = userDetailsWithRoles.FirstName;

//        List<string> userRoles = userDetailsWithRoles.listOfRoles.Select(r => r.RoleName).ToList();
//        this.Session["UserRoles"] = userRoles;

//        // If the user has only one role, set it as the CurrentRole and redirect to the home page
//        if (userRoles.Count == 1)
//        {
//            this.Session["CurrentRole"] = userRoles[0];
//            return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });
//        }
//        else
//        {
//            // If the user has multiple roles, redirect to the role selection page
//            return Json(new { result = IsUserValid, url = Url.Action("RoleSelection", "Account") });
//        }
//    }
//    return Json(new { result = IsUserValid, url = Url.Action("Login", "Account") });
//}