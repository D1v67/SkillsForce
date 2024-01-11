using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserActivityFilter : ActionFilterAttribute
    {


        public UserActivityFilter()
        {
          
        }

        //public void OnActionExecuted(ActionExecutedContext context)
        //{

        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get the user ID from the session
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            int userId = GetUserIdFromSession(filterContext.HttpContext);
            var url = $"{controllerName}/{actionName}";

            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            var ipAddress = filterContext.HttpContext.Request.UserHostAddress;

            var actionParameters = GetActionParameters(filterContext);

        }

        private string GetActionParameters(ActionExecutingContext filterContext)
        {
            // Extract data from action parameters
            var parameters = filterContext.ActionParameters;

            if (parameters != null && parameters.Count > 0)
            {
                var parameterData = parameters.Select(kv => $"{kv.Key}: {kv.Value?.ToString()}");
                return string.Join(", ", parameterData);
            }

            return null;
        }



        private int GetUserIdFromSession(HttpContextBase httpContext)
        {
            if (httpContext.Session != null && httpContext.Session["UserID"] != null)
            {
                return (int)httpContext.Session["UserID"];
            }
            else
            {
                return -1;
            }
        }

    }
}