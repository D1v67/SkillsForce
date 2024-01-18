using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public SessionTimeoutAttribute() { }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase ctx = filterContext.HttpContext;

            int defaultSessionTimeout = 1;
            ctx.Session.Timeout = defaultSessionTimeout;

            if (ctx.Session["userID"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}