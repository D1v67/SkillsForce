using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
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
            //throw new NotImplementedException();
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(AccountModel account)
        {
            bool IsUserValid = _loginService.IsUserAuthenticated(account);
            if (IsUserValid)
            {
                AccountModel userDetailsWithRoles = _loginService.GetUserDetailsWithRoles(account);
                this.Session["UserID"] = userDetailsWithRoles.UserID;
                this.Session["CurrentRole"] = userDetailsWithRoles.RoleName;
                this.Session["Email"] = userDetailsWithRoles.Email;
                this.Session["FirstName"] = userDetailsWithRoles.FirstName;
            }
            return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });
        }

        //[HttpPost]
        public ActionResult Register()
        {
            IEnumerable<DepartmentModel> departments = _departmentService.GetAll();
            IEnumerable<UserModel> managers = _userService.GetAllManager();

            RegisterViewModel registerViewModel = new RegisterViewModel() { ListOfDepartments = departments, ListOfManagers = managers };
            return View(departments);
        }

        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            RegisterViewModel registerViewModel = model;
            _loginService.RegisterUser(registerViewModel);

            return Json(new { url = Url.Action("Index", "Account") });
        }

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
    }
}