using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IExportSelectedEmployeeDAL
    {
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetAllAsync();
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetSelectedEmployeeByTrainingAsync(int trainingId);
    }
}
