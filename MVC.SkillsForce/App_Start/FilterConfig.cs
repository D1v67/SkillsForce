using Common.SkillsForce.AppLogger;
using MVC.SkillsForce.Custom;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new GlobalExceptionFilter(logger));
        }
    }
}
