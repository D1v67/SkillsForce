using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.SkillsForce.Interface
{
    public interface IExportSelectedEmployeeService
    {
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetAllAsync();
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetSelectedEmployeeByTrainingAsync(int trainingId);
    }
}
