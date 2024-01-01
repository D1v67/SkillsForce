using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetAllAsync();
            return View(prerequisites);
        }

        public async Task<JsonResult> GetListOfPrerequisites()
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetAllAsync();
            return Json(prerequisites, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPrerequisiteByTrainingID(int TrainigID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetPrerequisiteByTrainingIDAsync(TrainigID);
            return View(prerequisites);
        }
    }
}