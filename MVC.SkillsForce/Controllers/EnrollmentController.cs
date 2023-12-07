﻿using BusinessLayer.SkillsForce.Interface;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MVC.SkillsForce.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly INotificationService _notificationService;
        public EnrollmentController(IEnrollmentService enrollmentService, ITrainingService trainingService, IPrerequisiteService prerequisiteService, INotificationService notificationService)
        {
            _enrollmentService = enrollmentService;
            _trainingService = trainingService;
            _prerequisiteService = prerequisiteService;
            _notificationService = notificationService;
        }

        [CustomAuthorization(RolesEnum.Admin)]
        public ActionResult Index()
        {
            IEnumerable<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            try
            {
                enrollments = _enrollmentService.GetAllEnrollmentsWithDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(enrollments);
        }

        public ActionResult GetEnrollments()
        {
            try
            {
                if (Session == null || Session["UserID"] == null || Session["CurrentRole"] == null)
                {
                    return RedirectToAction("/Index"); 
                }
                var userId = Convert.ToInt32(Session["UserID"]);
                var currentRole = Session["CurrentRole"].ToString();

                IEnumerable<EnrollmentViewModel> enrollments = currentRole == "Manager"
                    ? _enrollmentService.GetAllEnrollmentsWithDetailsByManager(userId)
                    : _enrollmentService.GetAllEnrollmentsWithDetails();

                return View(enrollments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error"); 
            }
        }

        public ActionResult GetEnrollmentsForManager(int managerId)
        {
            IEnumerable<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            try
            {
                enrollments = _enrollmentService.GetAllEnrollmentsWithDetailsByManager(managerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(enrollments);
        }

        public ActionResult ViewTraining()
        {
            return View();
        }

        public JsonResult ViewTrainingData()
        {

            IEnumerable<TrainingModel> trainings = new List<TrainingModel>();
            try
            {
                trainings = _trainingService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(trainings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnrollTraining(int? trainingID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            prerequisites = _prerequisiteService.GetPrerequisiteByTrainingID((int)trainingID);
            ViewBag.ListOfPrerequisiteByTrainingID = prerequisites;
            return View();
        }

        [HttpPost]
        public ActionResult SaveEnrollment(EnrollmentViewModel model)
        {
            //EnrollmentViewModel enrollment = model;
            try
            {
                _enrollmentService.Add(model);
                // Console.WriteLine("Enrolled");
                return Json(new { success = true, message = "Enrollment successful!" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult GetPrerequisiteByTrainingID(int TrainigID)
        {
            IEnumerable<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();
            try
            {
                prerequisites = _prerequisiteService.GetPrerequisiteByTrainingID(TrainigID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(prerequisites, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ApproveEnrollment(int enrollmentId)
        {
            try
            {
                // Update the enrollment status to "Approved" in the database
                _enrollmentService.ApproveEnrollment(enrollmentId);
                EnrollmentNotificationViewModel enrollment= _enrollmentService.GetEnrollmentNotificationDetailsByID(enrollmentId);

                _notificationService.SendNotification(enrollment);
                // Return a success status
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/App_Data/Input"), Path.GetFileName(file.FileName));

                    //string[] lists = System.IO.File.ReadAllLines(Server.MapPath(path));

                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file or file is empty.";
            }

            //var items = FileManipulation.GetFiles(Server.MapPath("~/App_Data/Input"));

             return Json(new { success = true});


        }
        public ActionResult ModalView()
        {
 
            return View();

        }
        //TODO
        // GET: Enrollment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enrollment/Create
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

        // GET: Enrollment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Enrollment/Edit/5
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

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Enrollment/Delete/5
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

//public ActionResult TrainingTest()
//{
//    IEnumerable<TrainingModel> trainings = new List<TrainingModel>();

//    trainings = _trainingService.GetAll();


//    return View(trainings);


//}


//public ActionResult GetPrerequisiteByTrainingIDTest(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    IEnumerable<PrerequisiteModel> prerequisites = _prerequisiteService.GetPrerequisiteByTrainingID((int)id);
//    if (id == null)
//    {
//        return HttpNotFound();
//    }
//    ViewBag.list = prerequisites;
//    // return View(prerequisites);
//    // return Json(prerequisites, JsonRequestBehavior.AllowGet);
//    return View(prerequisites);
//}