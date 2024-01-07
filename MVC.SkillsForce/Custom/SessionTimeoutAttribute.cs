//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MVC.SkillsForce.Custom
//{
//    public class SessionTimeoutAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            HttpContext ctx = HttpContext.Current;

//            ////int userId = (int)ctx.Session["userId"]; // Assuming "userId" is stored in the session
//            //int defaultSessionTimeout = 1;

//            //// Set the session timeout
//            //ctx.Session.Timeout = defaultSessionTimeout;


//            if (HttpContext.Current.Session["userID"] == null)
//            {
//                filterContext.Result = new RedirectResult("~/Account/Index");
//                return;
//            }

//            base.OnActionExecuting(filterContext);
//        }
//    }
//}