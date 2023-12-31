using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.AppLogger;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
           // var exist = _userService.IsMobileNumberExists("51111111");
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(AccountModel account)
        {
            bool isUserValid = _loginService.IsUserAuthenticated(account);
            if (isUserValid)
            {
                var userDetailsWithRoles = _loginService.GetUserDetailsWithRoles(account);
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
            //IEnumerable<DepartmentModel> departments = _departmentService.GetAll();
            //IEnumerable<UserModel> managers = _userService.GetAllManager();

            //RegisterViewModel registerViewModel = new RegisterViewModel() { ListOfDepartments = departments, ListOfManagers = managers };
            return View();
        }


        [HttpPost]
        public JsonResult Register(RegisterViewModel registerViewModel)
        {
            var result = _loginService.RegisterUser(registerViewModel);

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

       
        [HttpGet]
        public JsonResult GetDepartments()
        {
            IEnumerable<DepartmentModel> departments = _departmentService.GetAll();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetManagers()
        {
            IEnumerable<UserModel> managers = _userService.GetAllManager();
            return Json(managers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RoleSelection()
        {
            // IEnumerable<UserRoleModel> departments = _departmentService.GetAll();
            List<string> userRoles = (List<string>)Session["UserRoles"];

            // Pass user roles to the view
            return View(userRoles);
        }

        [HttpPost]
        public ActionResult SetRole(string selectedRole)
        {
            // Set the selected role in the session
            Session["CurrentRole"] = selectedRole;

            // Redirect to the home page or any other desired page based on the selected role
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
            return ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
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