using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.Enums;
using MVC.SkillsForce.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            IEnumerable<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();
            try
            {
                prerequisites = _prerequisiteService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(prerequisites);
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
            return View(prerequisites);

        }
    }
}