﻿using MVC.SkillsForce.Custom;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    // [UserSession]

    //[SessionTimeout]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        //[SessionTimeout]
       // [UserActivityFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}