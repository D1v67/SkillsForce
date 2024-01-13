using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    [UserActivityFilter]
    public class AdminController : Controller
    {
        [AuthorizePermission(Permissions.AdminDashboard)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizePermission(Permissions.GetEnrollment)]
        public ActionResult Enrollments()
        {
            return View();
        }
    }
}