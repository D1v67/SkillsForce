﻿using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DepartmentDAL : IDepartmentDAL
    {
        public const string GET_ALL_DEPARTMENT_QUERY = @"SELECT  * FROM [dbo].[Department]";

        public const string GET_TRAINING_BY_ID_QUERY = @"";

        public const string INSERT_TRAINING_QUERY = @"";

        public const string UPDATE_TRAINING_QUERY = @"";

        public const string DELETE_TRAINING_QUERY = @"DELETE FROM [dbo].[Training] WHERE [TrainingID] = @TrainingID";

        private readonly IDBCommand _dbCommand;

        public DepartmentDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public IEnumerable<DepartmentModel> GetAll()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();

            DepartmentModel department;
            var dt = _dbCommand.GetData(GET_ALL_DEPARTMENT_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                department = new DepartmentModel();
                department.DepartmentID = int.Parse(row["DepartmentID"].ToString());
                department.DepartmentName = row["DepartmentName"].ToString();
                departments.Add(department);
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
    }
}
