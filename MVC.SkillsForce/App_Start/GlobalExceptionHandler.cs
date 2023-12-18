﻿using Common.SkillsForce.AppLogger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.App_Start
{
    public class GlobalExceptionHandler : HandleErrorAttribute
    {
        private readonly ILogger _logger;
        public GlobalExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var logger = DependencyResolver.Current.GetService<ILogger>();
            logger.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                TempData = filterContext.Controller.TempData
            };
        }
    }
}