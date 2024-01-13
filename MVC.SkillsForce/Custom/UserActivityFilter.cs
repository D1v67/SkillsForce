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
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserActivityFilter : ActionFilterAttribute
    {

        private IUserActivityService _userActivityService = DependencyResolver.Current.GetService<IUserActivityService>();

        public UserActivityFilter()
        {
          
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get the user ID from the session
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            int userId = GetUserIdFromSession(filterContext.HttpContext);
            var url = $"{controllerName}/{actionName}";

            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            var ipAddress = filterContext.HttpContext.Request.UserHostAddress;

            var currentRole = GetUserCurrentRoleFromSession(filterContext.HttpContext);

            var actionParameters = GetActionParameters(filterContext);

            var userAgent = filterContext.HttpContext.Request.UserAgent;

            var sessionId = filterContext.HttpContext.Session?.SessionID;
            var referer = filterContext.HttpContext.Request.UrlReferrer?.AbsoluteUri;
            var statusCode = filterContext.HttpContext.Response.StatusCode;

            //svar responseSize = filterContext.HttpContext.Response.Filter.Length;

            var isMobileDevice = filterContext.HttpContext.Request.Browser.IsMobileDevice;

            //var requestHeaders = filterContext.HttpContext.Request.Headers;
            //var responseHeaders = filterContext.HttpContext.Response.Headers;


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

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var userActivityModel = filterContext.HttpContext.Items["UserActivityModel"] as UserActivityModel;

        //    if (userActivityModel != null)
        //    {
        //        var result = filterContext.Result;

        //        if (result is JsonResult jsonResult)
        //        {
        //            // Extract the data from the JsonResult and determine if it's a successful login
        //            var isLoginSuccessful = jsonResult.Data;

        //            // Save login activity separately with the login result
        //           // SaveLoginActivity(userActivityModel, isLoginSuccessful);
        //        }
        //        else
        //        {
        //            // If it's not a JsonResult, assume it's another type of action and save the user activity
        //            Add(userActivityModel);
        //        }
        //    }
        //}

        private bool Add(UserActivityModel model)
        {
            return Task.Run(async () => await _userActivityService.AddUserActivity(model)).Result;
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