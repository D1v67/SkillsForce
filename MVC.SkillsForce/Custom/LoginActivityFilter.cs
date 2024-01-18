using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Custom
{
    public class LoginActivityFilter : ActionFilterAttribute
    {

        private IUserActivityService _userActivityService = DependencyResolver.Current.GetService<IUserActivityService>();

        public LoginActivityFilter()
        {

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result;
            if (result is JsonResult jsonResult)
            {            
                var data = jsonResult.Data.ToString();

                var match = Regex.Match(data, @"result\s*=\s*(\w+)", RegexOptions.IgnoreCase);

                if (match.Success)
                {
 
                    var resultValue = match.Groups[1].Value;
                    // Check if it's equal to "True" 
                    if (bool.TryParse(resultValue, out var isLoginSuccessful) && isLoginSuccessful)
                    {
                        Debug.WriteLine("Login Successu");
                        int userId = GetUserIdFromSession(filterContext.HttpContext);
                        var ipAddress = filterContext.HttpContext.Request.UserHostAddress;
                        var userAgent = filterContext.HttpContext.Request.UserAgent;
                        var isMobileDevice = filterContext.HttpContext.Request.Browser.IsMobileDevice;
                       // var sessionId = filterContext.HttpContext.Session?.SessionID;
                        var userActivityModel = new UserActivityModel
                        {
                            UserID = userId,
                            IpAddress = ipAddress,
                            EventType = "Login",
                            EventTime = DateTime.Now,
                            UserAgent = userAgent,
                            IsMobileDevice = isMobileDevice,
                        };
                        _userActivityService.AddUserLoginActivity(userActivityModel);
                    }
                    else
                    {
                        Debug.WriteLine("Login faileds");
                    }
                }
            }
            else
            {
                Debug.WriteLine("sth faileds");
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

    }
}