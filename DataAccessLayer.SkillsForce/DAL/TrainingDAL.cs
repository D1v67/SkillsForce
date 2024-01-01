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
            const string GET_ALL_TRAINING_QUERY = @"SELECT * FROM [dbo].[Training]";
            List<TrainingModel> trainings = new List<TrainingModel>();

            using (SqlDataReader reader = _dbCommand.GetDataReader(GET_ALL_TRAINING_QUERY))
            {
                while (reader.Read())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    trainings.Add(training);
                }
            }

            return trainings;
        }
        public TrainingModel GetByID(int id)
        {
            const string GET_TRAINING_BY_ID_QUERY = @"SELECT t.* FROM Training t WITH(NOLOCK)
                                              LEFT JOIN Department d WITH(NOLOCK) ON t.DepartmentID = d.DepartmentID
                                              WHERE t.TrainingID = @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_TRAINING_BY_ID_QUERY, parameters))
            {
                if (reader.Read())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    return training;
                }
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

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_TRAINING_QUERY, parameters))
            {
                TrainingViewModel training = null;

                while (reader.Read())
                {
                    if (training == null)
                    {
                        // Create a new TrainingViewModel for the first row
                        training = new TrainingViewModel
                        {
                            TrainingID = trainingId,
                            TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                            TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                            RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")).ToString("MM/dd/yyyy"),
                            Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                            DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                            Prerequisites = new List<PrerequisiteModel>(),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")).ToString("MM/dd/yyyy"),
                        };
                    }

                    // Check if the prerequisite columns are not null (indicating a match in the JOIN)
                    if (!reader.IsDBNull(reader.GetOrdinal("PrerequisiteID")))
                    {
                        PrerequisiteModel prerequisite = new PrerequisiteModel
                        {
                            PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                            PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                        };

                        // Add the prerequisite to the existing list in TrainingViewModel
                        training.Prerequisites.Add(prerequisite);
                    }
                }

                return training;
            }
        }

        public IEnumerable<TrainingViewModel> GetAllTrainingWithPrerequsites()
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

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_ALL_TRAINING_QUERY, null))
            {
                while (reader.Read())
                {
                    int trainingId = reader.GetByte(reader.GetOrdinal("TrainingID"));

                    // Check if we already have the training in the list
                    TrainingViewModel training = trainings.FirstOrDefault(t => t.TrainingID == trainingId);

                    if (training == null)
                    {
                        // If the training doesn't exist in the list, create a new one
                        training = new TrainingViewModel
                        {
                            TrainingID = trainingId,
                            TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                            TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                            RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")).ToString("MM/dd/yyyy"),
                            Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                            DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                            Prerequisites = new List<PrerequisiteModel>()
                        };

                        trainings.Add(training);
                    }

                    // Check if the prerequisite columns are not DBNull.Value (indicating a match in the JOIN)
                    if (reader["PrerequisiteID"] != DBNull.Value)
                    {
                        PrerequisiteModel prerequisite = new PrerequisiteModel
                        {
                            PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                            PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                        };

                        // Add the prerequisite to the existing list in TrainingViewModel
                        training.Prerequisites.Add(prerequisite);
                    }
                }
            }

            return trainings;
        }


        public int GetCapacityID(int trainingID)
        {
            const string GET_CAPACITY_BY_ID_QUERY = @"SELECT Capacity FROM Training WHERE TrainingID = @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingID) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_CAPACITY_BY_ID_QUERY, parameters))
            {
                if (reader.Read())
                {
                    return reader.GetByte(reader.GetOrdinal("Capacity"));
                }
            }

            return -1;
        }

        public int GetRemainingCapacityID(int trainingID)
        {
            const string GET_REMAINING_CAPACITY_BY_ID_QUERY = @"
        SELECT
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

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_REMAINING_CAPACITY_BY_ID_QUERY, parameters))
            {
                if (reader.Read())
                {
                    return reader.GetInt32(reader.GetOrdinal("RemainingCapacity"));
                }
            }

            return -1;
        }

        public IEnumerable<TrainingModel> GetAllTrainingsByRegistrationDeadline(DateTime registrationDeadline, bool isCronJob)
        {
            const string GET_TRAININGS_BY_DEADLINE_QUERY = @"
        SELECT TrainingID, TrainingName, TrainingDescription, RegistrationDeadline, StartDate, Capacity, DepartmentID
        FROM [dbo].[Training]
        WHERE RegistrationDeadline = @RegistrationDeadline";

            const string GET_TRAININGS_UPTO_DEADLINE_QUERY = @"
        SELECT TrainingID, TrainingName, TrainingDescription, RegistrationDeadline, StartDate, Capacity, DepartmentID
        FROM [dbo].[Training]
        WHERE RegistrationDeadline <= @RegistrationDeadline";

            string query = isCronJob ? GET_TRAININGS_BY_DEADLINE_QUERY : GET_TRAININGS_UPTO_DEADLINE_QUERY;

            var parameters = new List<SqlParameter> { new SqlParameter("@RegistrationDeadline", registrationDeadline) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(query, parameters))
            {
                List<TrainingModel> trainings = new List<TrainingModel>();

                while (reader.Read())
                {
                    var training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        //StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    trainings.Add(training);
                }

                return trainings;
            }
        }

        public IEnumerable<TrainingModel> GetAllTrainingsEnrolledByUser(int id)
        {
            const string GET_ENROLLED_TRAININGS_QUERY = @"
        SELECT T.*
        FROM Training T
        WHERE EXISTS (
            SELECT 1
            FROM Enrollment E
            WHERE E.TrainingID = T.TrainingID
              AND E.UserID = @UserID
        )
        ORDER BY T.TrainingName";

            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_ENROLLED_TRAININGS_QUERY, parameters))
            {
                while (reader.Read())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    trainings.Add(training);
                }
            }

            return trainings.Count > 0 ? trainings : null;
        }

        public IEnumerable<TrainingModel> GetAllTrainingsNotEnrolledByUser(int id)
        {
            const string GET_NOT_ENROLLED_TRAININGS_QUERY = @"
        SELECT T.*
        FROM Training T
        WHERE NOT EXISTS (
            SELECT 1
            FROM Enrollment E
            WHERE E.TrainingID = T.TrainingID
              AND E.UserID = @UserID
        )
        ORDER BY T.TrainingName";

            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_NOT_ENROLLED_TRAININGS_QUERY, parameters))
            {
                while (reader.Read())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    trainings.Add(training);
                }
            }

            return trainings.Count > 0 ? trainings : null;
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