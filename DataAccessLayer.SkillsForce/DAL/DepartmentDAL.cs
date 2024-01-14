using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DepartmentDAL : IDepartmentDAL
    {
        private readonly IDBCommand _dbCommand;

        public DepartmentDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            const string GET_ALL_DEPARTMENT_QUERY = @"SELECT [DepartmentID], [DepartmentName] FROM [dbo].[Department]";
            List<DepartmentModel> departments = new List<DepartmentModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_DEPARTMENT_QUERY))
            {
                while (reader.Read())
                {
                    DepartmentModel department = new DepartmentModel
                    {
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                    };
                    departments.Add(department);
                }
            }
            return departments;
        }

        public Task<DepartmentModel> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(DepartmentModel department)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DepartmentModel department)
        {
            throw new NotImplementedException();
        }
    }
}
