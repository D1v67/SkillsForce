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
            IEnumerable<UserModel> users = new List<UserModel>();
            try
            {
                users = _userService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(users);
        }

        public JsonResult GetManagers()
        {
            IEnumerable<UserModel> managers = _userService.GetAllManager();
            return Json(managers, JsonRequestBehavior.AllowGet);
        }


        //TODO
        // GET: UserDefault/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserDefault/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: UserDefault/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDefault/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserDefault/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDefault/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserDefault/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}