using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class TrainingDAL : ITrainingDAL
    {
        public const string GET_ALL_TRAINING_QUERY = @"SELECT  * FROM [dbo].[Training]";

        public const string GET_TRAINING_BY_ID_QUERY = @"SELECT t.* FROM Training t WITH(NOLOCK)
                                                        LEFT JOIN Department d WITH(NOLOCK) ON t.DepartmentID = d.DepartmentID
                                                        WHERE t.TrainingID = @TrainingID";

        public const string INSERT_TRAINING_QUERY = @"INSERT INTO [dbo].[Training] ([TrainingName],[RegistrationDeadline] ,[TrainingDescription],[Capacity],[DepartmentID])
                                                     VALUES ( @TrainingName, @RegistrationDeadline, @TrainingDescription, @Capacity ,@DepartmentID)";

        public const string UPDATE_TRAINING_QUERY = @"UPDATE [dbo].[Training] SET [TrainingName] = @TrainingName,[RegistrationDeadline] = @RegistrationDeadline,[TrainingDescription] = @TrainingDescription
                                                     ,[Capacity] = @Capacity,[DepartmentID] = @DepartmentID
                                                     WHERE [TrainingID] = @TrainingID";

        public const string DELETE_TRAINING_QUERY = @"DELETE FROM [dbo].[Training] WHERE [TrainingID] = @TrainingID";

        public const string GET_TRAINING_BY_MANAGER_ID = @"SELECT T.TrainingID, T.TrainingName, T.RegistrationDeadline,T.TrainingDescription, T.Capacity,D.DepartmentName, U.FirstName, U.LastName, E.EnrollmentDate, E.EnrollmentStatus
                                                            FROM Training T JOIN Department D ON T.DepartmentID = D.DepartmentID JOIN Enrollment E ON T.TrainingID = E.TrainingID JOIN [User] U ON E.UserID = U.UserID
                                                            WHERE U.ManagerID = @ManagerID
                                                            ORDER BY T.TrainingID, U.UserID";

        public const string GET_TRAINING_BY_DEPARTMENT_ID = @"SELECT T.TrainingID,T.TrainingName, T.RegistrationDeadline,T.TrainingDescription,T.Capacity,D.DepartmentName
                                                            FROM Training T JOIN Department D ON T.DepartmentID = D.DepartmentID
                                                            WHERE T.DepartmentID = @DepartmentID";

        private readonly IDBCommand _dbCommand;

        public TrainingDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }
        public IEnumerable<TrainingModel> GetAll()
        {
            List<TrainingModel> trainings = new List<TrainingModel>();

            TrainingModel training;
            var dt = _dbCommand.GetData(GET_ALL_TRAINING_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();
                training.TrainingID = int.Parse(row["TrainingID"].ToString());
                training.TrainingName = row["TrainingName"].ToString();
                training.TrainingDescription = row["TrainingDescription"].ToString();
                training.RegistrationDeadline = (DateTime)row["RegistrationDeadline"];
                training.Capacity = int.Parse(row["Capacity"].ToString());
                training.DepartmentID = int.Parse(row["DepartmentID"].ToString());

                trainings.Add(training);
            }
            return trainings;
        }
        public TrainingModel GetByID(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };
            var dt = _dbCommand.GetDataWithConditions(GET_TRAINING_BY_ID_QUERY, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                TrainingModel training = new TrainingModel
                {
                    TrainingID = int.Parse(row["TrainingID"].ToString()),
                    TrainingName = row["TrainingName"].ToString(),
                    TrainingDescription = row["TrainingDescription"].ToString(),
                    RegistrationDeadline = (DateTime)row["RegistrationDeadline"],
                    Capacity = int.Parse(row["Capacity"].ToString()),
                    DepartmentID = int.Parse(row["DepartmentID"].ToString())
                };
                return training;
            }
            return null;
        }
        public void Add(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@TrainingDescription", training.TrainingDescription));
            parameters.Add(new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));
            parameters.Add(new SqlParameter("@DepartmentID", training.DepartmentID));

            _dbCommand.InsertUpdateData(INSERT_TRAINING_QUERY, parameters);
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };
            _dbCommand.InsertUpdateData(DELETE_TRAINING_QUERY, parameters);
        }

        public void Update(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@TrainingDescription", training.TrainingDescription));
            parameters.Add(new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));
            parameters.Add(new SqlParameter("@DepartmentID", training.DepartmentID));

            _dbCommand.InsertUpdateData(INSERT_TRAINING_QUERY, parameters);
        }
    }
}
