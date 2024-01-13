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
        private readonly Permissions _permission;
        private IUserAuthorizationService _authorizationService= DependencyResolver.Current.GetService<IUserAuthorizationService>();

        public AuthorizePermissionAttribute(Permissions permission)
        {
            _permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userID = GetUserIdFromSession(filterContext.HttpContext);

            // Fail fast if user ID is -1
            if (userID == -1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "NotFound" }));
                return;
            }
            if (!IsUserHavePermission(userID, _permission))
            {
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
    }
}