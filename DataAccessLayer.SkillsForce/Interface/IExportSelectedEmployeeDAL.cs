using Common.SkillsForce.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IExportSelectedEmployeeDAL
    {
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetAllAsync();
        Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetSelectedEmployeeByTrainingAsync(int trainingId);
    }
}
