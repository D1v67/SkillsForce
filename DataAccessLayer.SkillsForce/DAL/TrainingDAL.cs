using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Common.SkillsForce.ViewModel;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class TrainingDAL : ITrainingDAL
    {
        private readonly IDBCommand _dbCommand;
        public TrainingDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }
        public IEnumerable<TrainingModel> GetAll()
        {
            const string GET_ALL_TRAINING_QUERY = @"SELECT  * FROM [dbo].[Training]";
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
            const string GET_TRAINING_BY_ID_QUERY = @"SELECT t.* FROM Training t WITH(NOLOCK)
                                                        LEFT JOIN Department d WITH(NOLOCK) ON t.DepartmentID = d.DepartmentID
                                                        WHERE t.TrainingID = @TrainingID";
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
        public void Add(TrainingViewModel training)
        {
            const string INSERT_TRAINING_QUERY = @"
            INSERT INTO [dbo].[Training] ([TrainingName],[RegistrationDeadline],[TrainingDescription],[StartDate],[Capacity],[DepartmentID])
            VALUES (@TrainingName, @RegistrationDeadline, @TrainingDescription, @StartDate, @Capacity, @DepartmentID);";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TrainingName", training.TrainingName),
                    new SqlParameter("@TrainingDescription", training.TrainingDescription),
                    new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline),
                    new SqlParameter("@StartDate", training.StartDate),
                    new SqlParameter("@Capacity", training.Capacity),
                    new SqlParameter("@DepartmentID", training.DepartmentID)
                };
            // Retrieve the generated 
            int trainingID = _dbCommand.InsertDataAndReturnIdentity(INSERT_TRAINING_QUERY, parameters);
            // If TrainingID is obtained, add prerequisites to TrainingPrerequisite table
            if (trainingID > 0)
            {
                foreach (PrerequisiteModel prerequisite in training.Prerequisites)
                {
                    const string INSERT_TRAINING_PREREQUISITE_QUERY = @"INSERT INTO [dbo].[TrainingPrerequisite] (TrainingID, PrerequisiteID)
                                                                        VALUES (@TrainingID, @PrerequisiteID);";

                    List<SqlParameter> prerequisiteParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@TrainingID", trainingID),
                        new SqlParameter("@PrerequisiteID", prerequisite.PrerequisiteID)
                    };
                    _dbCommand.InsertUpdateData(INSERT_TRAINING_PREREQUISITE_QUERY, prerequisiteParameters);
                }
            }
        }

        public bool Delete(int trainingId)
        {
            const string DELETE_TRAINING_QUERY = @"
               BEGIN TRANSACTION;
            IF NOT EXISTS (
                SELECT 1
                FROM Enrollment
                WHERE TrainingID = @TrainingID
            )
            BEGIN
                DELETE FROM TrainingPrerequisite
                WHERE TrainingID = @TrainingID;

                DELETE FROM Training
                WHERE TrainingID = @TrainingID;

                COMMIT;
            END
            ELSE
            BEGIN
                ROLLBACK; 
            END";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) };
            //  get the number of affected rows
            int affectedRows = _dbCommand.InsertUpdateData(DELETE_TRAINING_QUERY, parameters);
            // If no rows were affected, it means there are enrollments
            return affectedRows > 0;
        }


        public void Update(TrainingViewModel training)
        {
            const string UPDATE_TRAINING_QUERY = @"
        UPDATE [dbo].[Training]
        SET [TrainingName] = @TrainingName,
            [RegistrationDeadline] = @RegistrationDeadline,
            [StartDate] = @StartDate,
            [TrainingDescription] = @TrainingDescription,
            [Capacity] = @Capacity,
            [DepartmentID] = @DepartmentID
        WHERE [TrainingID] = @TrainingID;
        
        DELETE FROM [dbo].[TrainingPrerequisite] WHERE [TrainingID] = @TrainingID;";

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingID", training.TrainingID));
            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@TrainingDescription", training.TrainingDescription));
            parameters.Add(new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline));
            parameters.Add(new SqlParameter("@StartDate", training.StartDate));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));
            parameters.Add(new SqlParameter("@DepartmentID", training.DepartmentID));

            _dbCommand.InsertUpdateData(UPDATE_TRAINING_QUERY, parameters);

            // Insert new prerequisites
            if (training.Prerequisites != null && training.Prerequisites.Any())
            {
                const string INSERT_PREREQUISITE_QUERY = @"INSERT INTO [dbo].[TrainingPrerequisite] ([TrainingID], [PrerequisiteID]) VALUES (@TrainingID, @PrerequisiteID);";

                foreach (var prerequisite in training.Prerequisites)
                {
                    var prerequisiteParameters = new List<SqlParameter>
                {
                    new SqlParameter("@TrainingID", training.TrainingID),
                    new SqlParameter("@PrerequisiteID", prerequisite.PrerequisiteID)
                };

                    _dbCommand.InsertUpdateData(INSERT_PREREQUISITE_QUERY, prerequisiteParameters);
                }
            }         
        }

        public TrainingViewModel GetTrainingWithPrerequisites(int trainingId)
        {
            const string GET_TRAINING_QUERY = @"
                    SELECT
                        T.TrainingID,
                        T.TrainingName,
                        T.RegistrationDeadline,
                        T.TrainingDescription,
                        T.Capacity,
                        T.StartDate,
                        T.DepartmentID,
                        P.PrerequisiteID,
                        P.PrerequisiteName
                        
                    FROM
                        Training T
                    LEFT JOIN
                        TrainingPrerequisite TP ON T.TrainingID = TP.TrainingID
                    LEFT JOIN
                        Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID
                    WHERE
                        T.TrainingID = @TrainingID;
                ";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) }; ;

            var dt = _dbCommand.GetDataWithConditions(GET_TRAINING_QUERY, parameters);

            TrainingViewModel training = null;

            foreach (DataRow row in dt.Rows)
            {
                if (training == null)
                {
                    // Create a new TrainingViewModel for the first row
                    training = new TrainingViewModel
                    {
                        TrainingID = trainingId,
                        TrainingName = row["TrainingName"].ToString(),
                        TrainingDescription = row["TrainingDescription"].ToString(),
                        RegistrationDeadline = ((DateTime)row["RegistrationDeadline"]).ToString("MM/dd/yyyy"),
                        Capacity = int.Parse(row["Capacity"].ToString()),
                        DepartmentID = int.Parse(row["DepartmentID"].ToString()),
                        Prerequisites = new List<PrerequisiteModel>(),
                        StartDate = ((DateTime)row["StartDate"]).ToString("MM/dd/yyyy"),
                    };
                }

                // Check if the prerequisite columns are not null (indicating a match in the JOIN)
                if (row["PrerequisiteID"] != DBNull.Value)
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString()),
                        PrerequisiteName = row["PrerequisiteName"].ToString()
                    };

                    // Add the prerequisite to the existing list in TrainingViewModel
                    training.Prerequisites.Add(prerequisite);
                }
            }

            return training;
        }


        public IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsiites()
        {
            const string GET_ALL_TRAINING_QUERY = @"
                                SELECT
                        T.TrainingID,
                        T.TrainingName,
                        T.RegistrationDeadline,
                        T.TrainingDescription,
                        T.Capacity,
                        T.DepartmentID,
                        P.PrerequisiteID,
                        P.PrerequisiteName
                    FROM
                        Training T
                    LEFT JOIN
                        TrainingPrerequisite TP ON T.TrainingID = TP.TrainingID
                    LEFT JOIN
                        Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID;
                    ";

            List<TrainingViewModel> trainings = new List<TrainingViewModel>();

            TrainingViewModel training = null;
            var dt = _dbCommand.GetData(GET_ALL_TRAINING_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                int trainingId = int.Parse(row["TrainingID"].ToString());

                // Check if we already have the training in the list
                training = trainings.FirstOrDefault(t => t.TrainingID == trainingId);

                if (training == null)
                {
                    // If the training doesn't exist in the list, create a new one
                    training = new TrainingViewModel
                    {
                        TrainingID = trainingId,
                        TrainingName = row["TrainingName"].ToString(),
                        TrainingDescription = row["TrainingDescription"].ToString(),
                        RegistrationDeadline = ((DateTime)row["RegistrationDeadline"]).ToString("MM/dd/yyyy"),
                        Capacity = int.Parse(row["Capacity"].ToString()),
                        DepartmentID = int.Parse(row["DepartmentID"].ToString()),
                        Prerequisites = new List<PrerequisiteModel>()
                    };

                    trainings.Add(training);
                }

                // Check if the prerequisite columns are not null (indicating a match in the JOIN)
                if (row["PrerequisiteID"] != DBNull.Value)
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString()),
                        PrerequisiteName = row["PrerequisiteName"].ToString()
                    };

                    // Add the prerequisite to the existing list in TrainingViewModel
                    training.Prerequisites.Add(prerequisite);
                }
            }

            return trainings;
        }

        public int GetCapacityID(int trainingID)
        {
            const string GET_CAPACITY_BY_ID_QUERY = @"SELECT Capacity FROM Training WHERE TrainingID = @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingID) };
            var dt = _dbCommand.GetDataWithConditions(GET_CAPACITY_BY_ID_QUERY, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return int.Parse(row["Capacity"].ToString());
            }
            return -1;
        }

        public int GetRemainingCapacityID(int trainingID)
        {
            const string GET_REMAINING_CAPACITY_BY_ID_QUERY = @"SELECT
                                                                    T.TrainingID,
                                                                    T.TrainingName,
                                                                    T.Capacity - COUNT(E.EnrollmentID) AS RemainingCapacity
                                                                FROM
                                                                    Training T
                                                                LEFT JOIN
                                                                    Enrollment E ON T.TrainingID = E.TrainingID AND E.IsSelected = 1
                                                                WHERE
                                                                    T.TrainingID = @TrainingID
                                                                GROUP BY
                                                                    T.TrainingID, T.TrainingName, T.Capacity";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingID) };
            var dt = _dbCommand.GetDataWithConditions(GET_REMAINING_CAPACITY_BY_ID_QUERY, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return int.Parse(row["RemainingCapacity"].ToString());
            }
            return -1;
        }

        public IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline, bool isCronJob)
        {
            const string GET_TRAININGS_BY_DEADLINE_QUERY = @"SELECT TrainingID,TrainingName,TrainingDescription,RegistrationDeadline,StartDate,Capacity,DepartmentID
                                                             FROM [dbo].[Training] WHERE RegistrationDeadline = @RegistrationDeadline";

            const string GET_TRAININGS_UPTO_DEADLINE_QUERY = @"SELECT TrainingID,TrainingName,TrainingDescription,RegistrationDeadline,StartDate,Capacity,DepartmentID
                                                          FROM [dbo].[Training] WHERE RegistrationDeadline <= @RegistrationDeadline";

            string query = isCronJob ? GET_TRAININGS_BY_DEADLINE_QUERY : GET_TRAININGS_UPTO_DEADLINE_QUERY;

            var parameters = new List<SqlParameter> { new SqlParameter("@RegistrationDeadline", registrationDeadline) };

            var dt = _dbCommand.GetDataWithConditions(query, parameters);

            List<TrainingModel> trainings = new List<TrainingModel>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var training = new TrainingModel
                    {
                        TrainingID = int.Parse(row["TrainingID"].ToString()),
                        TrainingName = (row["TrainingName"].ToString()),
                        TrainingDescription = row["TrainingDescription"].ToString(),
                        RegistrationDeadline = (DateTime)row["RegistrationDeadline"],
                        Capacity = int.Parse(row["Capacity"].ToString()),
                        DepartmentID = int.Parse(row["DepartmentID"].ToString())
                    };

                    trainings.Add(training);
                }
                return trainings;
            }
            return null;
        }

        public IEnumerable<TrainingModel> GetAllTrainingsEnrolledByUser(int id)
        {
            const string GET_ENROLLED_TRAININGS_QUERY = @"SELECT T.*
                                                        FROM Training T
                                                        WHERE  EXISTS (
                                                            SELECT 1
                                                            FROM Enrollment E
                                                            WHERE E.TrainingID = T.TrainingID
                                                              AND E.UserID = @UserID 	
                                                        )
                                                        Order by T.TrainingName";
            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };
            var dt = _dbCommand.GetDataWithConditions(GET_ENROLLED_TRAININGS_QUERY, parameters);
            TrainingModel training;

            if (dt.Rows.Count > 0)
            {
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
            return null;
        }

        public IEnumerable<TrainingModel> GetAllTrainingsNotEnrolledByUser(int id)
        {
            const string GET_ENROLLED_TRAININGS_QUERY = @"SELECT T.*
                                                        FROM Training T
                                                        WHERE  NOT EXISTS (
                                                            SELECT 1
                                                            FROM Enrollment E
                                                            WHERE E.TrainingID = T.TrainingID
                                                              AND E.UserID = @UserID 	
                                                        )
                                                        Order by T.TrainingName";
            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };
            var dt = _dbCommand.GetDataWithConditions(GET_ENROLLED_TRAININGS_QUERY, parameters);
            TrainingModel training;

            if (dt.Rows.Count > 0)
            {
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
            return null;
        }


        public bool IsTrainingNameAlreadyExists(string trainingName)
        {
            const string IS_TRAINING_NAME_ALREADY_EXIST_QUERY = "SELECT 1 FROM [Training] WHERE TrainingName = @TrainingName";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingName", trainingName) };
            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(IS_TRAINING_NAME_ALREADY_EXIST_QUERY, parameters))
            {
                return reader.Read();
            }
        }

        public bool IsTrainingNameAlreadyExistsOnUpdate(int trainingId, string newTrainingName)
        {
            const string IS_TRAINING_NAME_ALREADY_EXIST_QUERY = " SELECT 1 FROM [dbo].[Training] WHERE [TrainingName] = @TrainingName AND [TrainingID] != @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingName", newTrainingName), new SqlParameter("@TrainingID", trainingId) };
            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(IS_TRAINING_NAME_ALREADY_EXIST_QUERY, parameters))
            {
                return reader.Read();
            }
        }
    }
}


//const string GET_TRAINING_BY_MANAGER_ID = @"SELECT T.TrainingID, T.TrainingName, T.RegistrationDeadline,T.TrainingDescription, T.Capacity,D.DepartmentName, U.FirstName, U.LastName, E.EnrollmentDate, E.EnrollmentStatus
//                                                            FROM Training T JOIN Department D ON T.DepartmentID = D.DepartmentID JOIN Enrollment E ON T.TrainingID = E.TrainingID JOIN [User] U ON E.UserID = U.UserID
//                                                            WHERE U.ManagerID = @ManagerID
//                                                            ORDER BY T.TrainingID, U.UserID";

//const string GET_TRAINING_BY_DEPARTMENT_ID = @"SELECT T.TrainingID,T.TrainingName, T.RegistrationDeadline,T.TrainingDescription,T.Capacity,D.DepartmentName
//                                                            FROM Training T JOIN Department D ON T.DepartmentID = D.DepartmentID
//                                                            WHERE T.DepartmentID = @DepartmentID";



//    public IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline)
//    {
//        const string GET_TRAININGS_BY_DEADLINE_QUERY = @"
//    SELECT
//        TrainingID,
//        TrainingName,
//        TrainingDescription,
//        RegistrationDeadline,
//        StartDate,
//        Capacity,
//        DepartmentID
//    FROM
//        [dbo].[Training]
//    WHERE
//        RegistrationDeadline = @RegistrationDeadline";

//        var parameters = new List<SqlParameter> { new SqlParameter("@RegistrationDeadline", registrationDeadline) };
//        var dt = _dbCommand.GetDataWithConditions(GET_TRAININGS_BY_DEADLINE_QUERY, parameters);

//        List<TrainingModel> trainings = new List<TrainingModel>();

//        if (dt.Rows.Count > 0)
//        {
//            foreach (DataRow row in dt.Rows)
//            {
//                var training = new TrainingModel
//                {
//                    TrainingID = int.Parse(row["TrainingID"].ToString()),
//                    TrainingName = (row["TrainingName"].ToString()),
//                    TrainingDescription = row["TrainingDescription"].ToString(),
//                    RegistrationDeadline = (DateTime)row["RegistrationDeadline"],
//                    Capacity = int.Parse(row["Capacity"].ToString()),
//                    DepartmentID = int.Parse(row["DepartmentID"].ToString())
//                };

//                trainings.Add(training);
//            }
//            return trainings;
//        }
//        return null;
//    }