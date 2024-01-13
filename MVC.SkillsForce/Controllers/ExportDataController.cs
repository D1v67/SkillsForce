using BusinessLayer.SkillsForce.Interface;
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
        public  async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId, string trainingName)
        {
            try
            {
                byte[] dummyExcelData = Encoding.UTF8.GetBytes("Dummy Excel Data");
                byte[] excelData = await _selectedEmployeeService.ExportToExcel(trainingId, trainingName);
                //string fileName = $"ExportedSelectedEmployees_{DateTime.Now:f}.csv";
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


        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {

        //        byte[] excelData = await _selectedEmployeeService.ExportToExcel(trainingId);
        //        Response.AddHeader("content-disposition", "attachment;filename=ExportedSelectedUsers.xslx");
        //        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedSelectedUsers.xslx");
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.WriteLine($"Error exporting data: {ex.Message}");


        //        return new HttpStatusCodeResult(500, "Internal Server Error");
        //    }
        //}
        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {

        //        byte[] excelData = await _selectedEmployeeService.ExportToExcel(trainingId);
        //        Response.AddHeader("content-disposition", "attachment;filename=ExportedSelectedUsers.xslx");
        //        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedSelectedUsers.xslx");
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.WriteLine($"Error exporting data: {ex.Message}");


        //        return new HttpStatusCodeResult(500, "Internal Server Error");
        //    }
        //}

        //public async Task<ActionResult> DownloadAttachmentByAttachmentID(int id)
        //{
        //    var result = await _attachmentService.GetByAttachmentIDAsync(id);

        //    byte[] binaryData = result.FileData;
        //    string filename = result.FileName;
        //    string contentType = "application/pdf";

        //    return File(binaryData, contentType, filename);
        //}        //public async Task<ActionResult> DownloadAttachmentByAttachmentID(int id)
        //{
        //    var result = await _attachmentService.GetByAttachmentIDAsync(id);

        //    byte[] binaryData = result.FileData;
        //    string filename = result.FileName;
        //    string contentType = "application/pdf";

        //    return File(binaryData, contentType, filename);
        //}

        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {
        //        var list = await _selectedEmployeeService.GetSelectedEmployeeByTrainingAsync(trainingId);

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (StreamWriter strwApproved = new StreamWriter(memoryStream))
        //            {
        //                strwApproved.WriteLine("Approved");
        //                strwApproved.WriteLine("FirstName, LastName,TotalPoints,Status");

        //                foreach (var item in list)
        //                {
        //                    strwApproved.WriteLine(string.Format("{0},  {1},   {2},  {3}", item.FirstName, item.LastName, item.DepartmentName, item.MobileNumber));
        //                }

        //                // Don't write an empty line here, if not needed
        //            }

        //            Response.ClearContent();
        //            Response.AddHeader("content-disposition", string.Format("attachment;filename=Summary{0}.csv", DateTime.Now));
        //            Response.ContentType = "text/csv";
        //            memoryStream.Position = 0;
        //            memoryStream.CopyTo(Response.OutputStream);
        //        }

        //        // Return the file path
        //        return Json(new { success = true, message = "Excel exported successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it accordingly
        //        return Json(new { success = false, message = "Error exporting Excel" });
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {
        //        var list = await _selectedEmployeeService.GetSelectedEmployeeByTrainingAsync(trainingId);

        //        // Create a new list with the desired data structure
        //        var exportData = new List<ExportEmployeeEnrollmentViewModel>();
        //        // Populate exportData with data from 'list' or modify as needed
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context
        //        // Create Excel package
        //        using (var package = new ExcelPackage())
        //        {
        //            var worksheet = package.Workbook.Worksheets.Add("EmployeeEnrollment");

        //            // Add headers
        //            worksheet.Cells[1, 1].Value = "First Name";
        //            worksheet.Cells[1, 2].Value = "Last Name";
        //            worksheet.Cells[1, 3].Value = "Email";
        //            // Add other headers as needed

        //            // Add data
        //            for (int i = 0; i < exportData.Count; i++)
        //            {
        //                worksheet.Cells[i + 2, 1].Value = exportData[i].FirstName;
        //                worksheet.Cells[i + 2, 2].Value = exportData[i].LastName;
        //                worksheet.Cells[i + 2, 3].Value = exportData[i].Email;
        //                // Add other data as needed
        //            }

        //            // Set column headers style
        //            using (var range = worksheet.Cells[1, 1, 1, 3]) // Adjust the range based on the number of columns
        //            {
        //                range.Style.Font.Bold = true;
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
        //            }

        //            // Save the Excel package to a physical path
        //            //  var filePath = "path/to/save/EmployeeEnrollmentExport.xlsx";

        //            var relativePath = "~/App_Data/ExcelExports/"; // Adjust this path as needed
        //            var absolutePath = Server.MapPath(relativePath);

        //            var fileName = "EmployeeEnrollmentExport.xlsx";
        //            var filePath = Path.Combine(absolutePath, fileName);


        //            package.SaveAs(new FileInfo(filePath));

        //            // Return the file path
        //            return Json(new { success = true, message = "Excel exported successfully", filePath = filePath });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it accordingly
        //        return Json(new { success = false, message = "Error exporting Excel" });
        //    }
        //}        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {
        //        var list = await _selectedEmployeeService.GetSelectedEmployeeByTrainingAsync(trainingId);

        //        // Create a new list with the desired data structure
        //        var exportData = new List<ExportEmployeeEnrollmentViewModel>();
        //        // Populate exportData with data from 'list' or modify as needed
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context
        //        // Create Excel package
        //        using (var package = new ExcelPackage())
        //        {
        //            var worksheet = package.Workbook.Worksheets.Add("EmployeeEnrollment");

        //            // Add headers
        //            worksheet.Cells[1, 1].Value = "First Name";
        //            worksheet.Cells[1, 2].Value = "Last Name";
        //            worksheet.Cells[1, 3].Value = "Email";
        //            // Add other headers as needed

        //            // Add data
        //            for (int i = 0; i < exportData.Count; i++)
        //            {
        //                worksheet.Cells[i + 2, 1].Value = exportData[i].FirstName;
        //                worksheet.Cells[i + 2, 2].Value = exportData[i].LastName;
        //                worksheet.Cells[i + 2, 3].Value = exportData[i].Email;
        //                // Add other data as needed
        //            }

        //            // Set column headers style
        //            using (var range = worksheet.Cells[1, 1, 1, 3]) // Adjust the range based on the number of columns
        //            {
        //                range.Style.Font.Bold = true;
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
        //            }

        //            // Save the Excel package to a physical path
        //            //  var filePath = "path/to/save/EmployeeEnrollmentExport.xlsx";

        //            var relativePath = "~/App_Data/ExcelExports/"; // Adjust this path as needed
        //            var absolutePath = Server.MapPath(relativePath);

        //            var fileName = "EmployeeEnrollmentExport.xlsx";
        //            var filePath = Path.Combine(absolutePath, fileName);


        //            package.SaveAs(new FileInfo(filePath));

        //            // Return the file path
        //            return Json(new { success = true, message = "Excel exported successfully", filePath = filePath });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it accordingly
        //        return Json(new { success = false, message = "Error exporting Excel" });
        //    }
        //}


        //[HttpPost]
        //public async Task<ActionResult> ExportSelectedEmployeeByTrainingCSV(int trainingId)
        //{
        //    try
        //    {
        //        var list = await _selectedEmployeeService.GetSelectedEmployeeByTrainingAsync(trainingId);

        //        // TODO: Logic for exporting to CSV

        //        // Assuming the CSV export is successful, you can return a success message
        //        return Json(new { success = true, message = "CSV exported successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it accordingly
        //        return Json(new { success = false, message = "Error exporting CSV" });
        //    }
        //}


    }
}