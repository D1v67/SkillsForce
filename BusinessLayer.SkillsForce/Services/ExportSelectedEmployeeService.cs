using BusinessLayer.SkillsForce.Interface;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
