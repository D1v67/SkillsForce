using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            IEnumerable<UserModel> users = _userService.GetAll();
            return View(users);
        }

        public JsonResult GetManagers()
        {
            IEnumerable<UserModel> managers = _userService.GetAllManager();
            return Json(managers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}