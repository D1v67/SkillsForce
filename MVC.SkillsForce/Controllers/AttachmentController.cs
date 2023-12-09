using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
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
        // GET: Download
        public ActionResult Index()
        {
            //throw new Exception();
            IEnumerable<AttachmentModel> attachments = new List<AttachmentModel>();
            try
            {
                attachments = _attachmentService.GetAll();
                //attachments = _uploaderService
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(attachments);
        }

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

            //return Json(attachments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadAttachmentByAttachmentID(int id)
        {
            //var results = _EvidenceRepositoryBL.GetAll().GetModelList().FirstOrDefault(f => f.FileId == id);
            var result = _attachmentService.GetByAttachmentID(id);

            byte[] binaryData = result.FileData;
            string filename = "dd";
            //// enhancement
            //// - get the extension and look up for the appropriate mime type
            string contentType = "application/pdf";

            return File(binaryData, contentType, filename);
        }

    }
}