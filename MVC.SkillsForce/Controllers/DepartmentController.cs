using BusinessLayer.SkillsForce.Interface;
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
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [CustomAuthorization(RolesEnum.Admin, RolesEnum.Manager, RolesEnum.Employee)]
        public ActionResult Index()
        {
            IEnumerable<DepartmentModel> departments = GetListOfDepartments();
            return View(departments);
        }

         [CustomAuthorization(RolesEnum.Admin, RolesEnum.Manager, RolesEnum.Employee)]
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