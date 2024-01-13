using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    [UserActivityFilter]
    public class AttachmentController : Controller
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [AuthorizePermission(Permissions.GetAttachment)]
        public async Task<ActionResult> Index()
        {
            IEnumerable<AttachmentModel> attachments = await _attachmentService.GetAllAsync();
            return View(attachments);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllAttachmentByEnrollmentID(int enrollmentID)
        {
            IEnumerable<AttachmentModel> attachments = await _attachmentService.GetAllByEnrollmentIDAsync(enrollmentID);
            return Json(new { result = attachments }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DownloadAttachmentByAttachmentID(int id)
        {
            var result = await _attachmentService.GetByAttachmentIDAsync(id);

            byte[] binaryData = result.FileData;
            string filename = result.FileName;
            string contentType = "application/pdf";

            return File(binaryData, contentType, filename);
        }

        public async Task<ActionResult> ViewAttachmentByAttachmentID(int id)
        {
            var result = await _attachmentService.GetByAttachmentIDAsync(id);

            byte[] binaryData = result.FileData;
            string contentType = "application/pdf";

            return new FileContentResult(binaryData, contentType);
        }

    }
}