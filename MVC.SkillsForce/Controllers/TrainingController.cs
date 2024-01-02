using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;
        public TrainingController(ITrainingService trainingService, IPrerequisiteService prerequisiteService)
        {
            _trainingService = trainingService;
            _prerequisiteService = prerequisiteService;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<TrainingModel> trainings = await _trainingService.GetAllAsync();
            return View(trainings);
        }

        public async Task<ActionResult> GetAllTrainingWithPrerequisites()
        {
            IEnumerable<TrainingViewModel> trainings = await _trainingService.GetAllTrainingWithPrerequisitesAsync();
            return View(trainings);
        }

        public async Task<JsonResult> GetCapacityById(int id)
        {
            int capacity = await _trainingService.GetCapacityIDAsync(id);

            if (capacity != -1)
            {
                return Json(new { success = true, capacity = capacity }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = $"No capacity found for Training ID {id}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetRemainingCapacityById(int id)
        {
            int capacity = await _trainingService.GetRemainingCapacityIDAsync(id);

            if (capacity != -1)
            {
                return Json(new { success = true, capacity = capacity }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = $"No capacity found for Training ID {id}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateTraining()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateTraining(TrainingViewModel model)
        {
            var result = await _trainingService.AddAsync(model);
            if (result.IsSuccessful)
            {
                return Json(new { url = Url.Action("Index", "Training") });
            }
            AddErrorsToModelState(result.Errors);
            var errors = GetModelStateErrors();
            return Json(new { errorMessage = errors });
        }

        public async Task<ActionResult> Edit(int id)
        {
            var training = await _trainingService.GetTrainingWithPrerequisitesAsync(id);
            return View(training);
        }

        public async Task<JsonResult> GetTrainingDetails(int id)
        {
            var training = await _trainingService.GetTrainingWithPrerequisitesAsync(id);
            return Json(training, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Edit(TrainingViewModel model)
        {
            var result = await _trainingService.UpdateAsync(model);
            if (result.IsSuccessful)
            {
                return Json(new { url = Url.Action("Index", "Training") });
            }
            AddErrorsToModelState(result.Errors);
            var errors = GetModelStateErrors();
            return Json(new { errorMessage = errors });


        }

        public ActionResult Delete(int id)
        {
            bool isDeletionSuccessful =  _trainingService.Delete(id);

            if (isDeletionSuccessful)
            {
                TempData["SuccessMessage"] = "Training deleted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = $"Cannot delete the training because it has enrolled users: ";
                return RedirectToAction("Index");
            }
        }

        private void AddErrorsToModelState(List<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private List<string> GetModelStateErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        }

    }
}