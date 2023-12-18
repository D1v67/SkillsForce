using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [CustomAuthorization(RolesEnum.Admin)]
        public ActionResult Index()
        {
            IEnumerable<AttachmentModel> attachments = new List<AttachmentModel>();
            try
            {
                attachments = _attachmentService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(attachments);
        }

        [CustomAuthorization(RolesEnum.Admin, RolesEnum.Manager)]
        [HttpGet]
        public JsonResult GetAllAttachmentByEnrollmentID(int enrollmentID)
        {         
            IEnumerable<AttachmentModel> attachments = new List<AttachmentModel>();
            try
            {
                attachments = _attachmentService.GetAllByEnrollmentID(enrollmentID);
                //attachments = _uploaderService
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(new { result = attachments }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization(RolesEnum.Admin, RolesEnum.Manager)]
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