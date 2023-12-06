using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    public class UserSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Email"] ==null || filterContext.HttpContext.Session["CurrentRole"]==null)
            {
                filterContext.Result = new RedirectResult("~/Account/Index");
            }
        }
    }
}