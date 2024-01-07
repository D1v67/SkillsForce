using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [AuthorizePermission(Permissions.GetUser)]
        public async Task<ActionResult> Index()
        {
            IEnumerable<UserModel> users = await _userService.GetAllAsync();
            return View(users);
        }


        public async Task<JsonResult> GetManagers()
        {
            IEnumerable<UserModel> managers = await _userService.GetAllManagerAsync();
            return Json(managers, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(int id)
        {
            UserModel user = await _userService.GetByIDAsync(id);
            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }


    }
}