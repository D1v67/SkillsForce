using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class PrerequisiteController : Controller
    {
        private readonly PrerequisiteService _prerequisiteService;

        public PrerequisiteController(PrerequisiteService prerequisiteService)
        {
            _prerequisiteService = prerequisiteService;
        }
        public ActionResult Index()
        {
            IEnumerable<PrerequisiteModel> prerequisites =  _prerequisiteService.GetAll();
            return View(prerequisites);
        }

        public JsonResult GetListOfPrerequisites()
        {
            IEnumerable<PrerequisiteModel> prerequisites =  _prerequisiteService.GetAll();
            return Json(prerequisites, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPrerequisiteByTrainingID(int TrainigID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = _prerequisiteService.GetPrerequisiteByTrainingID(TrainigID);
            return View(prerequisites);
        }
    }
}