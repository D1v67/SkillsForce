using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;
using Common.SkillsForce.Helpers;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class TrainingDAL : ITrainingDAL
    {
        private readonly IDBCommand _dbCommand;
        public TrainingDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        #region Add
        public async Task AddAsync(TrainingViewModel training)
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

            int trainingID = await _dbCommand.InsertDataAndReturnIdentityAsync(INSERT_TRAINING_QUERY, parameters);

            if (trainingID > 0 && training.Prerequisites != null)
            {
                foreach (PrerequisiteModel prerequisite in training.Prerequisites)
                {
                    if (prerequisite != null)
                    {
                        const string INSERT_TRAINING_PREREQUISITE_QUERY = @"INSERT INTO [dbo].[TrainingPrerequisite] (TrainingID, PrerequisiteID)
                        VALUES (@TrainingID, @PrerequisiteID);";

                        List<SqlParameter> prerequisiteParameters = new List<SqlParameter>
                        {
                            new SqlParameter("@TrainingID", trainingID),
                            new SqlParameter("@PrerequisiteID", prerequisite.PrerequisiteID)
                        };

                        await _dbCommand.InsertUpdateDataAsync(INSERT_TRAINING_PREREQUISITE_QUERY, prerequisiteParameters);
                    }
                }
            }
        }
        #endregion

        #region Get
        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            const string GET_ALL_TRAINING_QUERY = @"SELECT * FROM [dbo].[Training]";
            List<TrainingModel> trainings = new List<TrainingModel>();

            try
            {
                using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_TRAINING_QUERY))
                {
                    while (await reader.ReadAsync())
                    {
                        //TrainingModel training = new TrainingModel
                        //{
                        //    TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        //    TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        //    TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        //    RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        //    Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        //    DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                        //};
                        TrainingModel training = DataReaderMapper.MapToObject<TrainingModel>(reader);
                        trainings.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return trainings;
        }

        public async Task<TrainingModel> GetByIDAsync(int id)
        {
            const string GET_TRAINING_BY_ID_QUERY = @"SELECT t.TrainingID, t.TrainingName, t.TrainingDescription, t.RegistrationDeadline,t.Capacity,t.DepartmentID 
            FROM Training t LEFT JOIN Department d ON t.DepartmentID = d.DepartmentID
            WHERE t.TrainingID = @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", id) };

                using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_TRAINING_BY_ID_QUERY, parameters))
                {
                    if (await reader.ReadAsync())
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

        public async Task<TrainingViewModel> GetTrainingWithPrerequisitesAsync(int trainingId)
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
                                                    T.TrainingID = @TrainingID;";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_TRAINING_QUERY, parameters))
            {
                TrainingViewModel training = null;

                while (await reader.ReadAsync())
                {
                    if (training == null)
                    {
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

                    if (!reader.IsDBNull(reader.GetOrdinal("PrerequisiteID")))
                    {
                        PrerequisiteModel prerequisite = new PrerequisiteModel
                        {
                            PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                            PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                        };
                        training.Prerequisites.Add(prerequisite);
                    }
                }
                return training;
            }
        }

        public async Task<IEnumerable<TrainingViewModel>> GetAllTrainingWithPrerequisitesAsync()
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
                Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID;";

            List<TrainingViewModel> trainings = new List<TrainingViewModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_TRAINING_QUERY, null))
            {
                while (await reader.ReadAsync())
                {
                    int trainingId = reader.GetByte(reader.GetOrdinal("TrainingID"));

                    // Check if already have the training in the list
                    TrainingViewModel training = trainings.FirstOrDefault(t => t.TrainingID == trainingId);

                    if (training == null)
                    {
                        // If the training doesn't exist  create  new one
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
                    // Check if the prerequisite columns are not DBNull.Value--- a match in the JOIN)
                    if (reader["PrerequisiteID"] != DBNull.Value)
                    {
                        PrerequisiteModel prerequisite = new PrerequisiteModel
                        {
                            PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                            PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                        };
                        // Add the prerequisite
                        training.Prerequisites.Add(prerequisite);
                    }
                }
            }

            return trainings;
        }

        public async Task<int> GetCapacityIDAsync(int trainingID)
        {
            const string GET_CAPACITY_BY_ID_QUERY = @"SELECT Capacity FROM Training WHERE TrainingID = @TrainingID";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingID) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_CAPACITY_BY_ID_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return reader.GetByte(reader.GetOrdinal("Capacity"));
                }
            }
            return -1;
        }

        public async Task<int> GetRemainingCapacityIDAsync(int trainingID)
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

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_REMAINING_CAPACITY_BY_ID_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return reader.GetInt32(reader.GetOrdinal("RemainingCapacity"));
                }
            }

            return -1;
        }

        /// <summary>
        /// Retrieves a list of training models based on the registration deadline.
        /// A list of trainings less than registration deadline are retrieved. Hence selection is performed after deadline is over. i.e. the next day
        /// </summary>
        /// <param name="registrationDeadline">The deadline for registration.</param>
        /// <param name="isCronJob">A flag indicating whether the method is called by a cron job or by admin clicking the Automatic selection button.</param>
        /// <returns>An enumerable collection of TrainingModel objects that macthes the criteria.</returns>
        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsByRegistrationDeadlineAsync(DateTime registrationDeadline, bool isCronJob)
        {
            const string GET_TRAININGS_BY_DEADLINE_QUERY = @"
            SELECT TrainingID, TrainingName, TrainingDescription, RegistrationDeadline, StartDate, Capacity, DepartmentID
            FROM [dbo].[Training]
            WHERE RegistrationDeadline < @RegistrationDeadline AND  IsSelectionOver = 0";

            const string GET_TRAININGS_UPTO_DEADLINE_QUERY = @"
            SELECT TrainingID, TrainingName, TrainingDescription, RegistrationDeadline, StartDate, Capacity, DepartmentID
            FROM [dbo].[Training]
            WHERE RegistrationDeadline < @RegistrationDeadline AND IsSelectionOver = 0";

            string query = isCronJob ? GET_TRAININGS_BY_DEADLINE_QUERY : GET_TRAININGS_UPTO_DEADLINE_QUERY;

            var parameters = new List<SqlParameter> { new SqlParameter("@RegistrationDeadline", registrationDeadline) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(query, parameters))
            {
                List<TrainingModel> trainings = new List<TrainingModel>();

                while (await reader.ReadAsync())
                {
                    var training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID"))
                    };

                    trainings.Add(training);
                }

                return trainings;
            }
        }

        public async Task<IEnumerable<TrainingEnrollmentViewModel>> GetAllTrainingsEnrolledByUserAsync(int id)
        {
            const string GET_ENROLLED_TRAININGS_QUERY = @"SELECT T.TrainingID, T.TrainingName, T.TrainingDescription, T.RegistrationDeadline, T.StartDate, T.Capacity, T.DepartmentID, E.EnrollmentID
            FROM Training T INNER JOIN Enrollment E ON T.TrainingID = E.TrainingID
            WHERE E.UserID = @UserID AND E.IsActive = 1 ORDER BY T.TrainingName";

            List<TrainingEnrollmentViewModel> trainings = new List<TrainingEnrollmentViewModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ENROLLED_TRAININGS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    TrainingEnrollmentViewModel training = new TrainingEnrollmentViewModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                    };

                    trainings.Add(training);
                }
            }
            return trainings.Count > 0 ? trainings : null;
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingsNotEnrolledByUserAsync(int id)
        {
            const string GET_NOT_ENROLLED_TRAININGS_QUERY = @"
             SELECT T.TrainingID, T.TrainingName, T.TrainingDescription, T.RegistrationDeadline, T.StartDate, T.Capacity, T.DepartmentID, D.DepartmentName 
             FROM Training T 
             LEFT JOIN Department D ON T.DepartmentID = D.DepartmentID 
             WHERE NOT EXISTS (
                 SELECT 1
                 FROM Enrollment E
                 WHERE E.TrainingID = T.TrainingID
                 AND E.UserID = @UserID AND E.IsActive = 1                
             )
             ORDER BY T.TrainingName";

            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_NOT_ENROLLED_TRAININGS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                    };

                    trainings.Add(training);
                }
            }

            return trainings.Count > 0 ? trainings : null;
        }

        public async Task<IEnumerable<TrainingModel>> GetAllTrainingByTrainerIDAsync(int id)
        {
            const string GET_ENROLLED_TRAININGS_QUERY = @"SELECT T.TrainingID, T.TrainingName, T.TrainingDescription, T.RegistrationDeadline, T.StartDate, T.Capacity, T.IsSelectionOver
            FROM Training T WHERE TrainerID = @TrainerID AND IsActive = 1";

            List<TrainingModel> trainings = new List<TrainingModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainerID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ENROLLED_TRAININGS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    TrainingModel training = new TrainingModel
                    {
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingDescription = reader.GetString(reader.GetOrdinal("TrainingDescription")),
                        RegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        Capacity = reader.GetByte(reader.GetOrdinal("Capacity")),
                        IsSelectionOver = reader.GetBoolean(reader.GetOrdinal("IsSelectionOver")),
                    };

                    trainings.Add(training);
                }
            }

            return trainings.Count > 0 ? trainings : null;
        }

        #endregion

        #region Training Operations
        public async Task<bool> DeleteAsync(int trainingId)
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
                  RETURN 1;  -- Success
              END
              ELSE
              BEGIN
                  ROLLBACK;
                  RETURN 0;  -- Enrollments exist
              END";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) };
            int result = await _dbCommand.InsertUpdateDataAsync(DELETE_TRAINING_QUERY, parameters);
            return result == 1;
        }

        public async Task UpdateAsync(TrainingViewModel training)
        {
            const string UPDATE_TRAINING_QUERY = @"
            BEGIN TRANSACTION;
            UPDATE [dbo].[Training]
            SET [TrainingName] = @TrainingName,
                [RegistrationDeadline] = @RegistrationDeadline,
                [StartDate] = @StartDate,
                [TrainingDescription] = @TrainingDescription,
                [Capacity] = @Capacity,
                [DepartmentID] = @DepartmentID
            WHERE [TrainingID] = @TrainingID;
        
            DELETE FROM [dbo].[TrainingPrerequisite] WHERE [TrainingID] = @TrainingID;
            COMMIT;";

            bool isTrainingHaveEnrollments = await IsTrainingHaveEnrollment(training.TrainingID);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", training.TrainingID),
                new SqlParameter("@TrainingName", training.TrainingName),
                new SqlParameter("@TrainingDescription", training.TrainingDescription),
                new SqlParameter("@RegistrationDeadline", training.RegistrationDeadline),
                new SqlParameter("@StartDate", training.StartDate),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@DepartmentID", training.DepartmentID)
            };

            await _dbCommand.InsertUpdateDataAsync(UPDATE_TRAINING_QUERY, parameters);

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

                    await _dbCommand.InsertUpdateDataAsync(INSERT_PREREQUISITE_QUERY, prerequisiteParameters);
                }
            }
        }

        public async Task<bool> IsTrainingNameAlreadyExistsAsync(string trainingName)
        {
            const string IS_TRAINING_NAME_ALREADY_EXIST_QUERY = @"SELECT 1 FROM [Training]
            WHERE UPPER(REPLACE(TRIM(TrainingName), ' ', '')) = UPPER(REPLACE(TRIM(@TrainingName), ' ', ''))";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingName", trainingName) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(IS_TRAINING_NAME_ALREADY_EXIST_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task<bool> IsTrainingNameAlreadyExistsOnUpdateAsync(int trainingId, string newTrainingName)
        {
            const string IS_TRAINING_NAME_ALREADY_EXIST_QUERY = @" SELECT 1 FROM [dbo].[Training]
            WHERE UPPER(REPLACE(TRIM([TrainingName]), ' ', '')) = UPPER(REPLACE(TRIM(@TrainingName), ' ', '')) AND [TrainingID] != @TrainingID";

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingName", newTrainingName), new SqlParameter("@TrainingID", trainingId) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(IS_TRAINING_NAME_ALREADY_EXIST_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task<bool> IsTrainingHaveEnrollment(int trainingId)
        {
            const string CHECK_ENROLLMENTS_QUERY = @"SELECT 1 FROM [dbo].[Enrollment] WHERE [TrainingID] = @TrainingID;";
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", trainingId) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(CHECK_ENROLLMENTS_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        /// <summary>
        /// Soft Delete - Deletes a training by setting its IsActive flag to 0 if there are no associated enrollments.
        /// If there are enrollments, the training deletion is rolled back.
        /// </summary>
        /// <param name="trainingId">The ID of the training to be deleted.</param>
        /// <returns>
        /// Returns true if the training is deleted (IsActive set to 0) or false if there are associated enrollments.
        /// </returns>
        public bool Delete(int trainingId)
        {
            const string DELETE_TRAINING_QUERY = @"
               BEGIN TRANSACTION;
                IF NOT EXISTS (
                    SELECT 1
                    FROM Enrollment
                    WHERE TrainingID = @TrainingID AND IsActive = 1
                )
                BEGIN
                    UPDATE Training
                    SET IsActive = 0
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

        #endregion
    }
}

