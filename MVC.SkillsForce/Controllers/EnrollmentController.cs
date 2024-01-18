using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    [UserActivityFilter]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly INotificationService _notificationService;
        private readonly IAttachmentService _attachmentService;

        private readonly INotificationHandler _notificationHandler;

        public EnrollmentController(IEnrollmentService enrollmentService, ITrainingService trainingService, IPrerequisiteService prerequisiteService, INotificationService notificationService, IAttachmentService attachmentService, INotificationHandler notificationHandler)
        {
            _enrollmentService = enrollmentService;
            _trainingService = trainingService;
            _prerequisiteService = prerequisiteService;
            _notificationService = notificationService;
            _attachmentService = attachmentService;
            _notificationHandler = notificationHandler;
        }

        [AuthorizePermission(Permissions.GetEnrollment)]
        public async Task<ActionResult> Index()
        {
            IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsAsync();
            return View(enrollments);
        }

        [HttpPost]
        public async Task<ActionResult> RunAutomaticSelectionOfApprovedEnrollments(int userId)
        {
            var result = await _enrollmentService.RunAutomaticSelectionOfApprovedEnrollmentsAsync(userId, false);

            return Json(new
            {
                success = result.IsSuccessful,
                url = result.IsSuccessful ? Url.Action("Enrollments", "Admin") : null,
                messages = result.IsSuccessful ? result.SuccessMessages : null,
                errors = result.IsSuccessful ? null : result.Errors
            });
        }

        [AuthorizePermission(Permissions.GetEnrollmentByManager)]
        public async Task<ActionResult> GetEnrollments()
        {
            if (Session == null || Session["UserID"] == null || Session["CurrentRole"] == null)
            {
                return RedirectToAction("/Index");
            }
            var userId = Convert.ToInt32(Session["UserID"]);
            var currentRole = Session["CurrentRole"].ToString();

            IEnumerable<EnrollmentViewModel> enrollments = currentRole == "Manager"
                ? await _enrollmentService.GetAllEnrollmentsWithDetailsByManagerAsync(userId)
                : await _enrollmentService.GetAllEnrollmentsWithDetailsAsync();

            return View(enrollments);
        }

        [AuthorizePermission(Permissions.GetEnrollmentByManager)]
        public async Task<ActionResult> GetEnrollmentsData()
        {
            if (Session == null || Session["UserID"] == null || Session["CurrentRole"] == null)
            {
                return RedirectToAction("/Index");
            }
            var userId = Convert.ToInt32(Session["UserID"]);
            var currentRole = Session["CurrentRole"].ToString();

            IEnumerable<EnrollmentViewModel> enrollments = currentRole == "Manager"
                ? await _enrollmentService.GetAllEnrollmentsWithDetailsByManagerAsync(userId)
                : await _enrollmentService.GetAllEnrollmentsWithDetailsAsync();

            return Json(enrollments ?? Enumerable.Empty<EnrollmentViewModel>(), JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.GetEnrollmentByManager)]
        public async Task<ActionResult> GetEnrollmentsForManager(int managerId)
        {
            IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsByManagerAsync(managerId);
            return View(enrollments);
        }

        [AuthorizePermission(Permissions.GetEnrollment)]
        public async Task<ActionResult> GetAllApprovedEnrollments()
        {
            var approvedEnrollments = await _enrollmentService.GetAllApprovedEnrollmentsAsync();
            return View(approvedEnrollments);
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public ActionResult ViewTraining()
        {
            return View();
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> ViewTrainingData(int id)
        {
            IEnumerable<TrainingModel> trainings = await _trainingService.GetAllTrainingsNotEnrolledByUserAsync(id);
            return Json(trainings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> GetTrainingsAlreadyEnrolledByUser(int id)
        {
            IEnumerable<TrainingEnrollmentViewModel> trainings = await _trainingService.GetAllTrainingsEnrolledByUserAsync(id);
            return Json(trainings ?? Enumerable.Empty<TrainingEnrollmentViewModel>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<ActionResult> SaveEnrollment(EnrollmentViewModel model)
        {
            try
            {
                int generatedEnrollmentId = await _enrollmentService.AddAsync(model);
                EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(generatedEnrollmentId);
                await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Enrollment);
                return Json(new { success = true, message = "Enrollment successful!", EnrollmentID = generatedEnrollmentId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<ActionResult> ReEnrollSaveEnrollment(EnrollmentViewModel model)
        {
            try
            {
                int generatedEnrollmentId = await _enrollmentService.ReEnrollAddAsync(model);
                EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(generatedEnrollmentId);
                await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Enrollment);
                return Json(new { success = true, message = "Enrollment successful!", EnrollmentID = generatedEnrollmentId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> GetPrerequisiteByTrainingID(int TrainigID)
        {
            var prerequisites = await _prerequisiteService.GetPrerequisiteByTrainingIDAsync(TrainigID);
            return Json(prerequisites ?? new List<PrerequisiteModel>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ApproveEnrollment)]
        public async Task<JsonResult> ApproveEnrollment(int enrollmentId, int approvedByUserId)
        {
             await _enrollmentService.ApproveEnrollmentAsync(enrollmentId, approvedByUserId);
             EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);
             await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Approval);
             return Json(new { success = true });
        }

        [HttpPost]
        [AuthorizePermission(Permissions.RejectEnrollment)]
        public async Task<JsonResult> RejectEnrollment(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
                await _enrollmentService.RejectEnrollmentAsync(enrollmentId, rejectionReason, declinedByUserId);
                EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);
                await _notificationHandler.NotifyHandlersAsync(enrollment, NotificationType.Rejection);
                return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
        {
            var result = await _attachmentService.UploadFileAsync(files, EnrollmentID, PrerequisiteIDs);

            if (result.IsSuccessful)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, errors = result.Errors });
            }
        }

        [HttpGet]
        [AuthorizePermission(Permissions.GetEnrollment)]
        public JsonResult GetEnrollmentStatusOptions()
        {
            var enrollmentStatusOptions = Enum.GetNames(typeof(EnrollmentStatusEnum));
            var enrollmentStatusList = new List<string>(enrollmentStatusOptions);
            return Json(enrollmentStatusList, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.GetEnrollment)]
        public async Task<ActionResult> GetAllEnrollmentsData()
        {
            IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsAsync();
            return Json(enrollments ?? Enumerable.Empty<EnrollmentViewModel>(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AuthorizePermission(Permissions.GetEnrollment)]
        public async Task<JsonResult> FilterEnrollments(int trainingId, string status)
        {
                IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllFilteredEnrollmentsWithDetailsAsync(trainingId, status);
                return Json(enrollments);
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public ActionResult ViewEnrollmentsByUser()
        {
            return View();
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public ActionResult ViewConfirmedEnrollments()
        {
            return View();
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> ViewConfirmedEnrollmentsData(int id)
        {
            IEnumerable<EnrollmentViewModel> confirmedEnrollments = await _enrollmentService.GetAllConfirmedEnrollmentsAsync(id);
            return Json(confirmedEnrollments, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public ActionResult ViewRejectedEnrollments()
        {
            return View();
        }
        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> ViewRejectedEnrollmentsData(int id)
        {
            IEnumerable<EnrollmentViewModel> rejectedEnrollments = await _enrollmentService.GetAllDeclinedEnrollmentsByUserIDAsync(id);
            return Json(rejectedEnrollments, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public ActionResult ViewPendingEnrollments()
        {
            return View();
        }
        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<JsonResult> ViewPendingEnrollmentsData(int id)
        {
            IEnumerable<EnrollmentViewModel> rejectedEnrollments = await _enrollmentService.GetAllPendingEnrollmentsAsync(id);
            return Json(rejectedEnrollments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<ActionResult> UnEnroll(int EnrollmentId)
        {
            await _enrollmentService.UnEnrollAsync(EnrollmentId);
            return Json(new { success = true, message = "Unenrollment successful!" });

        }
    }
}

