﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}