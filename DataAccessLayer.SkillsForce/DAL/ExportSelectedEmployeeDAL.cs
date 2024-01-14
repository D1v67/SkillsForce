using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class ExportSelectedEmployeeDAL : IExportSelectedEmployeeDAL
    {
        private readonly DBCommand _dbCommand;
        public ExportSelectedEmployeeDAL(DBCommand dbCommand) 
        {
             _dbCommand = dbCommand;
        }
        public async Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExportEmployeeEnrollmentViewModel>> GetSelectedEmployeeByTrainingAsync(int trainingId)
        {
            const string GET_ALL_SELECTED_EMPLOYEES_BY_TRAINING_ID_QUERY =
            @"SELECT
                U.FirstName,
                U.LastName,
                U.Email,
                U.MobileNumber,
                U.ManagerID,
                M.FirstName AS ManagerFirstName,
                M.LastName AS ManagerLastName,
                U.DepartmentID,
                D.DepartmentName
            FROM
                [User] U
            JOIN
                Enrollment E ON U.UserID = E.UserID
            JOIN
                [User] M ON U.ManagerID = M.UserID
            JOIN
                Department D ON U.DepartmentID = D.DepartmentID
            WHERE
                E.TrainingID = @TrainingID
                AND E.IsSelected = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", trainingId)
            };

            List<ExportEmployeeEnrollmentViewModel> selectedEmployees = new List<ExportEmployeeEnrollmentViewModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_SELECTED_EMPLOYEES_BY_TRAINING_ID_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    ExportEmployeeEnrollmentViewModel employee = new ExportEmployeeEnrollmentViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                        ManagerID = reader.GetInt16(reader.GetOrdinal("ManagerID")),
                        ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                        ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                    };
                    selectedEmployees.Add(employee);
                }
            }

            return selectedEmployees;
        }
    }
}
