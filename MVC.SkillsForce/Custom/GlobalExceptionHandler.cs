using Common.SkillsForce.AppLogger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace MVC.SkillsForce.Custom
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

            _logger.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
           // filterContext.Result = new ViewResult { ViewName = "InternalServerError", TempData = new TempDataDictionary() { { "Message", filterContext.Exception.Message } } };

           // filterContext.Result = Redirectto("InternalServerError", new { message = filterContext.Exception.Message });
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Common" },
                { "action", "InternalServerError" },
                { "message", filterContext.Exception.Message }
            });

        }
    }
}