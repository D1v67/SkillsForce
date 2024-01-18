using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    public class UserActivityFilter : ActionFilterAttribute
    {
        private IUserActivityService _userActivityService = DependencyResolver.Current.GetService<IUserActivityService>();

        public UserActivityFilter()
        {
          
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int userId = GetUserIdFromSession(filterContext.HttpContext);

            if (userId == -1)
            {
                return;
            }

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            var url = $"{controllerName}/{actionName}";
            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            var ipAddress = filterContext.HttpContext.Request.UserHostAddress;
            var currentRole = GetUserCurrentRoleFromSession(filterContext.HttpContext);
            var actionParameters = GetActionParameters(filterContext);
            var userAgent = filterContext.HttpContext.Request.UserAgent;
            var sessionId = filterContext.HttpContext.Session?.SessionID;
            var referer = filterContext.HttpContext.Request.UrlReferrer?.AbsoluteUri;
            var statusCode = filterContext.HttpContext.Response.StatusCode;
            var isMobileDevice = filterContext.HttpContext.Request.Browser.IsMobileDevice;

            var userActivityModel = new UserActivityModel
            {
                UserID = userId,
                CurrentRole = currentRole,
                UrlVisited = url,
                HttpMethod = httpMethod,
                ActionParameters = actionParameters,
                IpAddress = ipAddress,
                UrlVisitedTimestamp = DateTime.Now,
                UserAgent = userAgent,
                SessionID = sessionId,
                Referer = referer,
                StatusCode = statusCode,
                IsMobileDevice = isMobileDevice,
            };

            Add(userActivityModel);
        }

        private bool Add(UserActivityModel model)
        {
            return Task.Run(async () => await _userActivityService.AddUserActivity(model)).Result;
        }

        private string GetActionParameters(ActionExecutingContext filterContext)
        {
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

        private string GetUserCurrentRoleFromSession(HttpContextBase httpContext)
        {
            if (httpContext.Session != null && httpContext.Session["UserID"] != null)
            {
                return httpContext.Session["CurrentRole"].ToString();
            }
            else
            {
                return null;
            }
        }

    }
}