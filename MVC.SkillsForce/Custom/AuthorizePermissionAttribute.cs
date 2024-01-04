using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.AppLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC.SkillsForce.Custom
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizePermissionAttribute:  ActionFilterAttribute
    {
        private readonly string _permission;

        private IUserAuthorizationService _authorizationService= DependencyResolver.Current.GetService<IUserAuthorizationService>();

        public AuthorizePermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get the user ID from the session
            var userID = GetUserIdFromSession(filterContext.HttpContext);

            // Check if the user has the required permission
            if (IsUserHavePermission(userID, _permission))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                // Redirect to the custom error action in your controller
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "AccessDenied" }));
            }
        }

        private int GetUserIdFromSession(HttpContextBase httpContext)
        {
            // Replace "YourUserIdSessionKey" with the key you use to store the user ID in the session
            return (int)httpContext.Session["UserID"];
        }


        private bool IsUserHavePermission(int userID, string permission)
        {
            return Task.Run(async () => await _authorizationService.IsUserHavePermissionAsync(userID, permission)).Result;
        }

        //private void RedirectToErrorAction(ActionExecutingContext filterContext)
        //{
        //    // Redirect to a custom error action in your controller
        //    var controllerName = "YourController"; // Adjust the controller name
        //    var actionName = "PermissionError";    // Adjust the action name
        //    var routeValues = new { controller = controllerName, action = actionName };

        //    filterContext.Result = new RedirectToRouteResult(routeValues);
        //}

    }
}