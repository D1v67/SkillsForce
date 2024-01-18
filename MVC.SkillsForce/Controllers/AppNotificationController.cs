using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    [UserActivityFilter]
    public class AppNotificationController : Controller
    {
        private readonly AppNotificationService _appNotificationService;

        public AppNotificationController(AppNotificationService appNotificationService)
        {
            _appNotificationService = appNotificationService;
        }

        [AuthorizePermission(Permissions.ViewNotification)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizePermission(Permissions.ViewNotification)]
        public async Task<JsonResult> GetAll()
        {
            IEnumerable<AppNotificationModel> notifications = await _appNotificationService.GetAllAsync();
            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.ViewNotification)]
        public async Task<JsonResult> GetByUserId(int id)
        {
            IEnumerable<AppNotificationModel> notifications = await _appNotificationService.GetByUserIdAsync(id);
            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewNotification)]
        public async Task<ActionResult> MarkNotificationAsRead(int notificationId, int userId)
        {
             await _appNotificationService.MarkNotificationAsReadAsync(notificationId);
            int unreadNotificationCount = await _appNotificationService.GetUnreadNotificationCountAsync(userId);
            Session["UnreadNotificationCount"] = unreadNotificationCount;

            return Json(new { redirectUrl = Url.Action("Index", "AppNotification"), success = true, message = "Notification marked as read" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.ViewNotification)]
        public async Task<JsonResult> GetUnreadNotificationCount(int id)
        {
            int capacity = await _appNotificationService.GetUnreadNotificationCountAsync(id);

            var success = capacity != -1;
            var message = success ? null : $"No notifications found for Training ID {id}";

            return Json(new { success, capacity, message }, JsonRequestBehavior.AllowGet);
        }
    }
}