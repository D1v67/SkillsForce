using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.AppLogger;
using Common.SkillsForce.Enums;
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
        //private readonly string _permission;
        private readonly Permissions _permission;

        private IUserAuthorizationService _authorizationService= DependencyResolver.Current.GetService<IUserAuthorizationService>();

        public AuthorizePermissionAttribute(Permissions permission)
        {
            _permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get the user ID from the session
            var userID = GetUserIdFromSession(filterContext.HttpContext);

            // Fail fast if user ID is -1
            if (userID == -1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "NotFound" }));
                return;
            }

            // Check if the user doesn't have the required permission
            if (!IsUserHavePermission(userID, _permission))
            {
                // Redirect to the custom error action in your controller
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "AccessDenied" }));
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
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


        private bool IsUserHavePermission(int userID, Permissions permission)
        {
            return Task.Run(async () => await _authorizationService.IsUserHavePermissionAsync(userID, permission.ToString())).Result;
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