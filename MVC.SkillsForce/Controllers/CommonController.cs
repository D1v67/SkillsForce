using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}