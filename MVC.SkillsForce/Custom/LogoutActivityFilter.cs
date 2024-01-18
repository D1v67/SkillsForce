using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    public class LogoutActivityFilter : ActionFilterAttribute
    {
        private IUserActivityService _userActivityService = DependencyResolver.Current.GetService<IUserActivityService>();

        public LogoutActivityFilter()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            int userId = GetUserIdFromSession(filterContext.HttpContext);
            var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            var ipAddress = filterContext.HttpContext.Request.UserHostAddress;
            var userAgent = filterContext.HttpContext.Request.UserAgent;
            var sessionId = filterContext.HttpContext.Session?.SessionID;
            var isMobileDevice = filterContext.HttpContext.Request.Browser.IsMobileDevice;

            var userActivityModel = new UserActivityModel
            {
                UserID = userId,
                IpAddress = ipAddress,
                EventType = "Logout",
                EventTime = DateTime.Now,
                UserAgent = userAgent,
                IsMobileDevice = isMobileDevice,
            };
            _userActivityService.AddUserLoginActivity(userActivityModel);
         
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