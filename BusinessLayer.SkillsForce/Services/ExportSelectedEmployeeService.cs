using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Services
{
    public class ExportSelectedEmployeeService : IExportSelectedEmployeeService
    {
        private readonly IExportSelectedEmployeeDAL _exportSelectedEmployeeDAL;
        public ExportSelectedEmployeeService(IExportSelectedEmployeeDAL exportSelectedEmployeeDAL)
        {
            _exportSelectedEmployeeDAL = exportSelectedEmployeeDAL;
        }
        public  Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async  Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetSelectedEmployeeByTrainingAsync(int trainingId)
        {
            return await _exportSelectedEmployeeDAL.GetSelectedEmployeeByTrainingAsync(trainingId);
        }

        public async Task<byte[]> ExportToExcel(int trainingId, string trainingName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"{trainingName}SelectedEmployees_{DateTime.Now:f}.xlsx");

                // Add Training Name at the top
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1"].Value = $"Training Name: {trainingName}";
                worksheet.Cells["A1:D1"].Style.Font.Bold = true;
                worksheet.Cells["A1:D1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                worksheet.Cells["A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var headers = new string[] { "Employee_FullName", "Employee_MobileNumber", "Employee_Email", "Manager_FullName" };
                for (byte i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[3, i + 1].Value = headers[i];
                    worksheet.Cells[3, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[3, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray); 
                }

                // Adding Data to worksheet
                List<ExportEmployeeEnrollmentViewModel> exportSelectedEmployees =
                    (await GetSelectedEmployeeByTrainingAsync(trainingId)).ToList();
                byte rowIndex = 4;
                foreach (var employee in exportSelectedEmployees)
                {
                    // Alternate row colors
                    if (rowIndex % 2 == 0)
                    {
                        var range = worksheet.Cells[rowIndex, 1, rowIndex, 4];
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                    }

  
                    worksheet.Cells[rowIndex, 1].Value = $"{employee.FirstName} {employee.LastName}";
                    worksheet.Cells[rowIndex, 2].Value = employee.MobileNumber;
                    worksheet.Cells[rowIndex, 3].Value = employee.Email;  
                    worksheet.Cells[rowIndex, 4].Value = $"{employee.ManagerFirstName} {employee.ManagerLastName}";

                    rowIndex++;
                }


                var tableRange = worksheet.Cells[3, 1, rowIndex - 1, 4];
                tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                tableRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                tableRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
