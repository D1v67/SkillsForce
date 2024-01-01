using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data;
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
            const string GET_ALL_DEPARTMENT_QUERY = @"SELECT * FROM [dbo].[Department]";
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








        public IEnumerable<DepartmentModel> GetAll()
        {
            const string GET_ALL_DEPARTMENT_QUERY = @"SELECT * FROM [dbo].[Department]";
            List<DepartmentModel> departments = new List<DepartmentModel>();

            using (SqlDataReader reader = _dbCommand.GetDataReader(GET_ALL_DEPARTMENT_QUERY))
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

        public DepartmentModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(DepartmentModel department)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(DepartmentModel department)
        {
            throw new NotImplementedException();
        }
    }
}

//public TrainingModel GetByID(int id)
//{
//    var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };
//    var dt = _dbCommand.GetDataWithConditions(GET_TRAINING_BY_ID_QUERY, parameters);

//    if (dt.Rows.Count > 0)
//    {
//        DataRow row = dt.Rows[0];

//        TrainingModel training = new TrainingModel
//        {
//            TrainingID = int.Parse(row["TrainingID"].ToString()),
//            TrainingName = row["TrainingName"].ToString(),
//            TrainingDescription = row["TrainingDescription"].ToString(),
//            RegistrationDeadline = (DateTime)row["RegistrationDeadline"],
//            Capacity = int.Parse(row["Capacity"].ToString()),
//            DepartmentID = int.Parse(row["DepartmentID"].ToString())
//        };
//        return training;
//    }

//    return null;
//}
//public void Add(TrainingModel training)
//{
//    List<SqlParameter> parameters = new List<SqlParameter>();

//    parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
//    parameters.Add(new SqlParameter("@TrainingDescription", training.TrainingDescription));
//    parameters.Add(new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline));
//    parameters.Add(new SqlParameter("@Capacity", training.Capacity));
//    parameters.Add(new SqlParameter("@DepartmentID", training.DepartmentID));

//    _dbCommand.InsertUpdateData(INSERT_TRAINING_QUERY, parameters);
//}

//public void Delete(int id)
//{
//    var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };
//    _dbCommand.InsertUpdateData(DELETE_TRAINING_QUERY, parameters);
//}

//public void Update(TrainingModel training)
//{
//    List<SqlParameter> parameters = new List<SqlParameter>();

//    parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
//    parameters.Add(new SqlParameter("@TrainingDescription", training.TrainingDescription));
//    parameters.Add(new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline));
//    parameters.Add(new SqlParameter("@Capacity", training.Capacity));
//    parameters.Add(new SqlParameter("@DepartmentID", training.DepartmentID));

//    _dbCommand.InsertUpdateData(INSERT_TRAINING_QUERY, parameters);
//}