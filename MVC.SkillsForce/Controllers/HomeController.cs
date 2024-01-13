using MVC.SkillsForce.Custom;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserActivityFilter]
    public class HomeController : Controller
    {      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}