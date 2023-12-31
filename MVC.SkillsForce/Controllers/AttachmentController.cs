using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        public ActionResult Index()
        {
            IEnumerable<AttachmentModel> attachments =  _attachmentService.GetAll();
            return View(attachments);
        }

        [HttpGet]
        public JsonResult GetAllAttachmentByEnrollmentID(int enrollmentID)
        {         
            IEnumerable<AttachmentModel> attachments = _attachmentService.GetAllByEnrollmentID(enrollmentID);  
            return Json(new { result = attachments }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadAttachmentByAttachmentID(int id)
        {
            var result = _attachmentService.GetByAttachmentID(id);

            byte[] binaryData = result.FileData;
            string filename = result.AttachmentURL;
            string contentType = "application/pdf";

            return File(binaryData, contentType, filename);
        }

    }
}