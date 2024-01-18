using Common.SkillsForce.AppLogger;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC.SkillsForce.Custom
{
    /// <summary>
    /// Custom exception filter for handling unhandled exceptions in the application.
    /// It Extends the default exception handling provided by HandleErrorAttribute.
    /// Logs exceptions, marks them as handled, sets the HTTP status code to 500, and renders
    /// a user-friendly error page with a generic message instead of exposing detailed exception information.
    /// </summary>
    public class GlobalExceptionHandler : HandleErrorAttribute
    {
        private readonly ILogger _logger;
        public GlobalExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            _logger.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Common" },
                { "action", "InternalServerError" },
                { "message", filterContext.Exception.Message }
            });

        }
    }
}