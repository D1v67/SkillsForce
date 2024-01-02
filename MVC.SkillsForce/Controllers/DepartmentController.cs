using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<JsonResult> GetListOfDepartments()
        {
            IEnumerable<DepartmentModel> departments = await _departmentService.GetAllAsync();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

    }
}