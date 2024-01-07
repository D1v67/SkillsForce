using System.Diagnostics;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult UnauthorizedAccessAttempt()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult InternalServerError(string message)
        {
            TempData["Message"] = message;
            Debug.WriteLine(TempData["Message"]);
            return View();
        }
    }
}