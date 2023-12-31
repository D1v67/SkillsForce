using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System.Collections.Generic;
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

        public JsonResult GetListOfDepartments()
        {
            IEnumerable<DepartmentModel> departments = _departmentService.GetAll();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

    }
}