using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.SkillsForce.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public ActionResult Index()
        {
            IEnumerable<DepartmentModel> departments = GetListOfDepartments();
            return View(departments);
        }

        private IEnumerable<DepartmentModel> GetListOfDepartments()
        {
            try
            {
                return _departmentService.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<DepartmentModel>();
            }
        }

    }
}