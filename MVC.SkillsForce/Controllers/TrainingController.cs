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
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;
        public TrainingController(ITrainingService trainingService, IPrerequisiteService prerequisiteService)
        {
            _trainingService = trainingService;
            _prerequisiteService = prerequisiteService;
        }
        //GET ALL TRAINING
        public ActionResult Index()
        {
            //DateTime currentDate = DateTime.Today;
            //DateTime registrationDeadline = new DateTime(2024, 2, 15);
            //IEnumerable<TrainingModel> trainings = _trainingService.GetAllTrainingsByRegistrationDeadline(registrationDeadline);

            IEnumerable<TrainingModel> trainings = new List<TrainingModel>();
            try
            {
                trainings = _trainingService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(trainings);
        }

        public ActionResult GetAllTrainingWithPrerequsiites()
        {

            IEnumerable<TrainingViewModel> trainings = new List<TrainingViewModel>();
            try
            {
                trainings = _trainingService.GetAllTrainingWithPrerequsiites();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        // GET: Training/Details/5

        // GET: Training/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Training/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Training/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Training/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Training/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Training/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}