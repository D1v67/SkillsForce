using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Index()
        {
            IEnumerable<TrainingModel> trainings = _trainingService.GetAll();
            return View(trainings);
        }

        public ActionResult GetAllTrainingWithPrerequsiites()
        {
            IEnumerable<TrainingViewModel> trainings = _trainingService.GetAllTrainingWithPrerequsites();
            return View(trainings);
        }
        // 

        public JsonResult GetCapacityById(int id)
        {
            int capacity = _trainingService.GetCapacityID(id);

            if (capacity != -1) 
            {
                return Json(new { success = true, capacity = capacity }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = $"No capacity found for Training ID {id}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingCapacityById(int id)
        {
            int capacity = _trainingService.GetRemainingCapacityID(id);

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
        public JsonResult CreateTraining(TrainingViewModel model)
        {
            if (_trainingService.IsTrainingNameAlreadyExists(model.TrainingName))
            {
                ModelState.AddModelError("Email", "TrainingName  already exists.");
            }

            if (ModelState.IsValid)
            {
                _trainingService.Add(model);

                return Json(new { url = Url.Action("Index", "Training") });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();

            return Json(new { errorMessage = errors });
        }


        public ActionResult Edit(int id)
        {
            var training = _trainingService.GetTrainingWithPrerequisites(id);
            return View(training);
        }

        public ActionResult GetTrainingDetails(int id)
        {
            var training = _trainingService.GetTrainingWithPrerequisites(id);
            return Json(training, JsonRequestBehavior.AllowGet);

        }
      
        [HttpPost]
        public JsonResult Edit(TrainingViewModel model)
        {
            if (_trainingService.IsTrainingNameAlreadyExistsOnUpdate(model.TrainingID, model.TrainingName))
            {
                ModelState.AddModelError("Email", "Training already exists with same name.");
            }

            if (ModelState.IsValid)
            {
                _trainingService.Update(model);

                return Json(new { url = Url.Action("Index", "Training") });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();

            return Json(new { errorMessage = errors });
        }


        public ActionResult Delete(int id)
        {
            bool isDeletionSuccessful = _trainingService.Delete(id);

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

    }
}