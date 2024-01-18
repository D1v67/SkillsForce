using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.Enums;
using Common.SkillsForce.ViewModel;
using MVC.SkillsForce.Custom;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace MVC.SkillsForce.Controllers
{
    [UserSession]
    [UserActivityFilter]
    public class ExportDataController: Controller
    {
        private readonly IExportSelectedEmployeeService _selectedEmployeeService;

        public ExportDataController(IExportSelectedEmployeeService selectedEmployeeService)
        {
            _selectedEmployeeService = selectedEmployeeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthorizePermission(Permissions.ViewSelectedEmployees)]
        public  async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId, string trainingName)
        {
            try
            {
                byte[] dummyExcelData = Encoding.UTF8.GetBytes("Dummy Excel Data");
                byte[] excelData = await _selectedEmployeeService.ExportToExcel(trainingId, trainingName);
                string fileName = $"{trainingName}SelectedEmployees_{DateTime.Now:f}.xlsx";

                Response.Clear();
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment; filename={fileName}");
                return File(excelData, contentType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error exporting data: {ex.Message}");
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }
    }
}