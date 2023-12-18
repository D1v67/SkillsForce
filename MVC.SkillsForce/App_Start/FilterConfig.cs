using Common.SkillsForce.AppLogger;
using MVC.SkillsForce.Custom;
using System.Web.Mvc;

namespace MVC.SkillsForce
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            var mylogger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new GlobalExceptionHandler(mylogger));
        }
    }
}
