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
    public class PrerequisiteController : Controller
    {
        private readonly PrerequisiteService _prerequisiteService;

        public PrerequisiteController(PrerequisiteService prerequisiteService)
        {
            _prerequisiteService = prerequisiteService;
        }

        [AuthorizePermission(Permissions.GetPrerequisite)]
        public async Task<ActionResult> Index()
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetAllAsync();
            return View(prerequisites);
        }

        [AuthorizePermission(Permissions.GetPrerequisite)]
        public async Task<JsonResult> GetListOfPrerequisites()
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetAllAsync();
            return Json(prerequisites, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePermission(Permissions.ViewTraining)]
        public async Task<ActionResult> GetPrerequisiteByTrainingID(int TrainigID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = await _prerequisiteService.GetPrerequisiteByTrainingIDAsync(TrainigID);
            return View(prerequisites);
        }
    }
}