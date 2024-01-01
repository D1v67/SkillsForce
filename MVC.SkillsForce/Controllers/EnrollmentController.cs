﻿using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly INotificationService _notificationService;
        private readonly IAttachmentService _attachmentService;

        public EnrollmentController(IEnrollmentService enrollmentService, ITrainingService trainingService, IPrerequisiteService prerequisiteService, INotificationService notificationService, IAttachmentService attachmentService)
        {
            _enrollmentService = enrollmentService;
            _trainingService = trainingService;
            _prerequisiteService = prerequisiteService;
            _notificationService = notificationService;
            _attachmentService = attachmentService;
        }

        [AuthorizePermission("GetAllEnrollment")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsAsync();
            return View(enrollments);
        }

        public async Task<ActionResult> RunAutomaticSelectionOfApprovedEnrollments()
        {
            bool isCronJob = false;
            await _enrollmentService.RunAutomaticSelectionOfApprovedEnrollmentsAsync(isCronJob);

            return RedirectToAction("/GetAllApprovedEnrollments");
        }

        [AuthorizePermission("GetAllEnrollment")]
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

        public async Task<ActionResult> GetEnrollmentsForManager(int managerId)
        {
            IEnumerable<EnrollmentViewModel> enrollments = await _enrollmentService.GetAllEnrollmentsWithDetailsByManagerAsync(managerId);
            return View(enrollments);
        }

        public async Task<ActionResult> GetAllApprovedEnrollments()
        {
            var approvedEnrollments = await _enrollmentService.GetAllApprovedEnrollmentsAsync();
            return View(approvedEnrollments);
        }

        public ActionResult ViewTraining()
        {
            return View();

        }

        [HttpPost]
        public async Task<JsonResult> ViewTrainingData(int id)
        {
            IEnumerable<TrainingModel> trainings = await _trainingService.GetAllTrainingsNotEnrolledByUserAsync(id);
            return Json(trainings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetTrainingsAlreadyEnrolledByUser(int id)
        {
            IEnumerable<TrainingModel> trainings = await _trainingService.GetAllTrainingsEnrolledByUserAsync(id);
            return Json(trainings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveEnrollment(EnrollmentViewModel model)
        {
            try
            {
                int generatedEnrollmentId = await _enrollmentService.AddAsync(model);
                return Json(new { success = true, message = "Enrollment successful!", EnrollmentID = generatedEnrollmentId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<JsonResult> GetPrerequisiteByTrainingID(int TrainigID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetPrerequisiteByTrainingIDAsync(TrainigID);
            return Json(prerequisites, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ApproveEnrollment(int enrollmentId)
        {
            try
            {
                await _enrollmentService.ApproveEnrollmentAsync(enrollmentId);
                EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);

                await _notificationService.SendNotificationAsync(enrollment, NotificationType.Approval);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> RejectEnrollment(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
            try
            {
                await _enrollmentService.RejectEnrollmentAsync(enrollmentId, rejectionReason, declinedByUserId);
                EnrollmentNotificationViewModel enrollment = await _enrollmentService.GetEnrollmentNotificationDetailsByIDAsync(enrollmentId);
                await _notificationService.SendNotificationAsync(enrollment, NotificationType.Rejection);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(List<HttpPostedFileBase> files, int EnrollmentID, string PrerequisiteIDs)
        {
            await _attachmentService.UploadFileAsync(files, EnrollmentID, PrerequisiteIDs);
            return Json(new { success = true });
        }
    }
}

