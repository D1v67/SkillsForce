using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Common.SkillsForce.ViewModel;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly IDBCommand _dbCommand;

        public EnrollmentDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        #region Add
        public async Task<int> AddAsync(EnrollmentViewModel enrollment)
        {
            const string INSERT_ENROLLMENT_QUERY = @"INSERT INTO [dbo].[Enrollment] ([UserID], [TrainingID]) OUTPUT INSERTED.EnrollmentID VALUES (@UserID, @TrainingID)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", enrollment.UserID),
                new SqlParameter("@TrainingID", enrollment.TrainingID)
            };
            int generatedEnrollmentId = await _dbCommand.InsertDataAndReturnIdentityAsync(INSERT_ENROLLMENT_QUERY, parameters);
            return generatedEnrollmentId;
        }
        #endregion

        #region Get
        public async Task<IEnumerable<EnrollmentViewModel>> GetAllAsync()
        {
            const string GET_ALL_ENROLLMENT_QUERY = @"SELECT [EnrollmentID], [UserID], [TrainingID], [EnrollmentDate], [EnrollmentStatus] FROM [dbo].[Enrollment]";
            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_ENROLLMENT_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus"))
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsAsync()
        {
            const string GET_ALL_ENROLLMENT_WITH_DETAILS_QUERY =
                    @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    AND E.IsActive = 1";

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_ENROLLMENT_WITH_DETAILS_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsByManagerAsync(int managerId)
        {
            const string GET_ALL_ENROLLMENT_WITH_DETAILS_BY_MANAGER_QUERY = @"
            SELECT E.EnrollmentID, U.UserID, U.FirstName, U.LastName, T.TrainingID, T.TrainingName, D.DepartmentName, E.EnrollmentDate, E.EnrollmentStatus
            FROM Enrollment E JOIN [User] U ON E.UserID = U.UserID JOIN Training T ON E.TrainingID = T.TrainingID JOIN Department D ON T.DepartmentID = D.DepartmentID
            WHERE U.ManagerID = @ManagerID AND E.IsActive = 1";

            var parameters = new List<SqlParameter> { new SqlParameter("@ManagerID", managerId) };
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_ENROLLMENT_WITH_DETAILS_BY_MANAGER_QUERY, parameters))
            {
                List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus"))
                    };
                    enrollments.Add(enrollment);
                }
                return enrollments;
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllPendingEnrollmentsAsync(int userId)
        {
            const string GET_PENDING_ENROLMENTS_QUERY =@"
                   SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected,
		                T.RegistrationDeadline,
                        T.StartDate
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.UserID = @UserID
                        AND E.EnrollmentStatus = 'Pending' AND E.IsActive = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_PENDING_ENROLMENTS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                        TrainingRegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        TrainingStartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        /// <summary>
        /// Retrieves enrollment notification details by enrollment ID asynchronously.
        /// </summary>
        /// <param name="id">The enrollment ID to retrieve details for.</param>
        /// <returns>An instance of EnrollmentNotificationViewModel containing the details of the enrollment notification, or null if not found.</returns>
        public async Task<EnrollmentNotificationViewModel> GetEnrollmentNotificationDetailsByIDAsync(int id)
        {
            const string GET_ENROLLMENT_NOTIFICATION_DETAILS_BY_ID =@"
                                                            SELECT
                                                                E.EnrollmentID,
                                                                U.UserID AS AppUserID,
                                                                U.FirstName AS AppUserFirstName,
                                                                U.LastName AS AppUserLastName,
                                                                U.Email AS AppUserEmail,
                                                                E.TrainingID,
                                                                T.TrainingName,
                                                                D.DepartmentName,
                                                                E.EnrollmentDate,
                                                                E.EnrollmentStatus,
                                                                E.DeclineReason,
                                                                U.ManagerID,
                                                                M.Email AS ManagerEmail,
                                                                M.FirstName AS ManagerFirstName,
                                                                M.LastName AS ManagerLastName
                                                             FROM
                                                                Enrollment E
                                                             INNER JOIN [User] U ON E.UserID = U.UserID
                                                             INNER JOIN Training T ON E.TrainingID = T.TrainingID
                                                             LEFT JOIN Department D ON U.DepartmentID = D.DepartmentID
                                                             LEFT JOIN [User] M ON U.ManagerID = M.UserID WHERE E.EnrollmentID = @EnrollmentID";

            var parameters = new List<SqlParameter> { new SqlParameter("@EnrollmentID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ENROLLMENT_NOTIFICATION_DETAILS_BY_ID, parameters))
            {
                if (await reader.ReadAsync())
                {
                    EnrollmentNotificationViewModel enrollment = new EnrollmentNotificationViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        AppUserID = reader.GetInt16(reader.GetOrdinal("AppUserID")),
                        AppUserFirstName = reader.GetString(reader.GetOrdinal("AppUserFirstName")),
                        AppUserLastName = reader.GetString(reader.GetOrdinal("AppUserLastName")),
                        AppUserEmail = reader.GetString(reader.GetOrdinal("AppUserEmail")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? string.Empty : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        ManagerID = reader.GetInt16(reader.GetOrdinal("ManagerID")),
                        ManagerEmail = reader.GetString(reader.GetOrdinal("ManagerEmail")),
                        ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                        ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName")),
                        DeclineReason = reader.IsDBNull(reader.GetOrdinal("DeclineReason")) ? string.Empty : reader.GetString(reader.GetOrdinal("DeclineReason")),
                    };
                    return enrollment;
                }
            }

            return null;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllApprovedEnrollmentsAsync()
        {
            const string GET_ALL_APPROVED_ENROLLMENT_WITH_DETAILS_QUERY =@"
                    SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.EnrollmentStatus = 'Approved'";

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_APPROVED_ENROLLMENT_WITH_DETAILS_QUERY, null))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredEnrollmentsWithDetailsAsync(int trainingId, string statusFilter)
        {
            const string GET_FILTERED_ENROLLMENTS_WITH_DETAILS_QUERY =
                @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                 WHERE E.TrainingID = @TrainingID AND E.EnrollmentStatus = @EnrollmentStatus";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", trainingId),
                new SqlParameter("@EnrollmentStatus", statusFilter)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_FILTERED_ENROLLMENTS_WITH_DETAILS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllFilteredConfirmedEnrollmentsWithDetailsAsync(int trainingId)
        {
            const string GET_FILTERED_CONFIRMED_ENROLLMENTS_WITH_DETAILS_QUERY =
                @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.TrainingID = @TrainingID
                        AND E.IsSelected = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", trainingId)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_FILTERED_CONFIRMED_ENROLLMENTS_WITH_DETAILS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsWithDetailsAsync()
        {
            const string GET_ALL_CONFIRMED_ENROLLMENTS_WITH_DETAILS_QUERY =
                @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE            
                         E.IsSelected = 1";

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_CONFIRMED_ENROLLMENTS_WITH_DETAILS_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        /// <summary>
        /// Retrieves all confirmed enrollments for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve confirmed enrollments.</param>
        /// <returns>
        /// A collection of EnrollmentViewModel representing confirmed enrollments with details,
        /// or an empty collection if no confirmed enrollments are found.
        /// </returns>
        public async Task<IEnumerable<EnrollmentViewModel>> GetAllConfirmedEnrollmentsAsync(int userId)
        {
            const string GET_ALL_CONFIRMED_ENROLLMENTS_WITH_DETAILS_BY_USER_QUERY =
                @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected,
		                T.RegistrationDeadline,
                        T.StartDate
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.UserID = @UserID
                        AND E.IsSelected = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_CONFIRMED_ENROLLMENTS_WITH_DETAILS_BY_USER_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                        TrainingRegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        TrainingStartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsByUserIdAsync(int userId)
        {
            const string GET_ALL_ENROLLMENTS_WITH_DETAILS_BY_USER_QUERY =
                @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected,
                        E.DeclineReason,
		                T.RegistrationDeadline,
                        T.StartDate
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.UserID = @UserID";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_ENROLLMENTS_WITH_DETAILS_BY_USER_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                        TrainingRegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        TrainingStartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        //For Pagination
        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsWithDetailsAsync(int page, int pageSize)
        {
            const string GET_ENROLLMENTS_WITH_DETAILS_PAGINATED_QUERY =
                @"SELECT
                E.EnrollmentID,
                U.UserID,
                U.FirstName,
                U.LastName,
                T.TrainingID,
                T.TrainingName,
                U.DepartmentID AS UserDepartmentID,
                UD.DepartmentName AS UserDepartmentName,
                T.DepartmentID AS TrainingDepartmentID,
                TD.DepartmentName AS TrainingDepartmentName,
                E.EnrollmentDate,
                E.EnrollmentStatus,
                E.IsSelected
            FROM
                Enrollment E
            JOIN
                [User] U ON E.UserID = U.UserID
            JOIN
                Training T ON E.TrainingID = T.TrainingID
            JOIN
                Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
            JOIN
                Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
            ORDER BY E.EnrollmentID
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            int offset = (page - 1) * pageSize;

            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@Offset", offset),
        new SqlParameter("@PageSize", pageSize)
    };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ENROLLMENTS_WITH_DETAILS_PAGINATED_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        IsSelected = reader.GetBoolean(reader.GetOrdinal("IsSelected")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllDeclinedEnrollmentsByUserIDAsync(int userId)
        {
            const string GET_DECLINED_ENROLLMENTS_WITH_DETAILS_QUERY =
                     @"SELECT
                        E.EnrollmentID,
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TrainingID,
                        T.TrainingName,
                        U.DepartmentID AS UserDepartmentID,
                        UD.DepartmentName AS UserDepartmentName,
                        T.DepartmentID AS TrainingDepartmentID,
                        TD.DepartmentName AS TrainingDepartmentName,
                        E.EnrollmentDate,
                        E.EnrollmentStatus,
                        E.IsSelected,
                        E.DeclineReason,
		                T.RegistrationDeadline,
                        T.StartDate
                    FROM
                        Enrollment E
                    JOIN
                        [User] U ON E.UserID = U.UserID
                    JOIN
                        Training T ON E.TrainingID = T.TrainingID
                    JOIN
                        Department UD ON U.DepartmentID = UD.DepartmentID -- User's department
                    JOIN
                        Department TD ON T.DepartmentID = TD.DepartmentID -- Training's department
                    WHERE
                        E.UserID = @UserID
                        AND E.EnrollmentStatus = 'Rejected'
                        AND E.IsActive = 1";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId)
            };

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_DECLINED_ENROLLMENTS_WITH_DETAILS_QUERY, parameters))
            {
                while (await reader.ReadAsync())
                {
                    EnrollmentViewModel enrollment = new EnrollmentViewModel
                    {
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TrainingID = reader.GetByte(reader.GetOrdinal("TrainingID")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("UserDepartmentName")),
                        TrainingDepartmentName = reader.GetString(reader.GetOrdinal("TrainingDepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        DeclineReason = reader.GetString(reader.GetOrdinal("DeclineReason")),
                        TrainingRegistrationDeadline = reader.GetDateTime(reader.GetOrdinal("RegistrationDeadline")),
                        TrainingStartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    };
                    enrollments.Add(enrollment);
                }
            }
            return enrollments;
        }

        #endregion

        #region Enrollment Operations
        public async Task ApproveEnrollmentAsync(int enrollmentId, int approvedByUserId)
        {
            const string UPDATE_STATUS_APPROVED_QUERY = @"UPDATE Enrollment 
                                                           SET EnrollmentStatus = 'Approved', 
                                                              ApprovedByUserId = @ApprovedByUserId,
                                                              LastModifiedTimestamp = GETDATE(),
                                                              LastModifiedUserId = @LastModifiedUserId,
                                                              ApprovedTimestamp = @ApprovedTimestamp
                                                           WHERE EnrollmentID = @EnrollmentID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentID", enrollmentId),
                new SqlParameter("@ApprovedByUserId", (object)approvedByUserId ?? DBNull.Value),
                new SqlParameter("@LastModifiedUserId", approvedByUserId),
                new SqlParameter("@ApprovedTimestamp", DateTime.Now)
            };
            await _dbCommand.InsertUpdateDataAsync(UPDATE_STATUS_APPROVED_QUERY, parameters);
        }
        public async Task RejectEnrollmentAsync(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
            const string UPDATE_STATUS_REJECTED_QUERY = @"UPDATE Enrollment
                                                            SET EnrollmentStatus = 'Rejected',
                                                                DeclineReason = @DeclineReason,
                                                                DeclinedByUserId = @DeclinedByUserId,
                                                                LastModifiedTimestamp = GETDATE(),
                                                                LastModifiedUserId = @LastModifiedUserId,
                                                                RejectedTimestamp = @RejectedTimestamp
                                                            WHERE EnrollmentID = @EnrollmentID";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentID", enrollmentId),
                new SqlParameter("@DeclineReason", (object)rejectionReason ?? DBNull.Value),
                new SqlParameter("@DeclinedByUserId", (object)declinedByUserId ?? DBNull.Value),
                new SqlParameter("@LastModifiedUserId", declinedByUserId),
                new SqlParameter("@RejectedTimestamp", DateTime.Now),
            };
            await _dbCommand.InsertUpdateDataAsync(UPDATE_STATUS_REJECTED_QUERY, parameters);
        }
        public async Task<List<int>> ConfirmEnrollmentsByTrainingIDAsync(int trainingID)
        {
            const string CONFIRM_ENROLLMENTS_BY_TRAINING_ID_QUERY = @"
            BEGIN TRANSACTION;
            DECLARE @EnrollmentIDs TABLE (EnrollmentID INT);

               WITH OrderedEnrollments AS (
                   SELECT
                       E.EnrollmentID,
                       E.UserID,
                       E.TrainingID,
                       E.EnrollmentDate,
                       E.EnrollmentStatus,
                       U.DepartmentID,
                       U.FirstName,
                       U.LastName,
                       E.IsSelected,
                       E.SelectedTimestamp,
                       ROW_NUMBER() OVER (ORDER BY IIF(U.DepartmentID = (SELECT T.DepartmentID FROM Training T WHERE T.TrainingID = E.TrainingID), 0, 1), E.EnrollmentDate) AS RowNum
                   FROM
                       Enrollment E
                   JOIN
                       [User] U ON E.UserID = U.UserID
                   WHERE
                       E.TrainingID = @TrainingID 
                   AND E.EnrollmentStatus = 'Approved'
               )

               UPDATE
                   OrderedEnrollments
               SET
                   IsSelected = 1,
                   SelectedTimestamp = GETDATE()
               OUTPUT
                   INSERTED.EnrollmentID
               INTO
                   @EnrollmentIDs
               WHERE
                   RowNum <= (SELECT Capacity FROM Training WHERE TrainingID = @TrainingID);

                -- Update IsSelectionOver in Training table
                UPDATE
                    Training
                SET
                    IsSelectionOver = 1,
                    LastSelectionTimeStamp = GETDATE()
                WHERE
                    TrainingID = @TrainingID;
                COMMIT;

               SELECT EnrollmentID FROM @EnrollmentIDs AS AffectedEnrollments;
               ";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", trainingID),
            };

            string outputColumnName = "EnrollmentID";

            return await _dbCommand.ExecuteQueryWithOutputAsync(CONFIRM_ENROLLMENTS_BY_TRAINING_ID_QUERY, parameters, outputColumnName);
        }

        /// <summary>
        /// Re-Enroll involves soft deleting the previos Enrollment and insert a new enrollment
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns>Returns the id of the inserted Enrolment</returns>
        public async Task<int> ReEnrollAddAsync(EnrollmentViewModel enrollment)
        {
            const string RE_ENROLL_ENROLLMENT_QUERY = @"BEGIN TRANSACTION
            UPDATE [dbo].[Enrollment]
            SET [IsActive] = 0
            WHERE [EnrollmentID] = @EnrollmentID

            INSERT INTO [dbo].[Enrollment] ([UserID], [TrainingID]) OUTPUT INSERTED.EnrollmentID VALUES (@UserID, @TrainingID)

            COMMIT";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", enrollment.UserID),
                new SqlParameter("@TrainingID", enrollment.TrainingID),
                new SqlParameter("@EnrollmentID", enrollment.rejectedEnrollmentID)
            };
            int generatedEnrollmentId = await _dbCommand.InsertDataAndReturnIdentityAsync(RE_ENROLL_ENROLLMENT_QUERY, parameters);
            return generatedEnrollmentId;
        }
        /// <summary>
        /// Soft Delete is performed by setting IsActive to 0. 
        /// </summary>
        /// <param name="enrollmentId"></param>
        /// <returns></returns>
        public async Task DeleteEnrollmentAsync(int enrollmentId)
        {
            const string SOFT_DELETE_ENROLLMENT_QUERY = @"UPDATE Enrollment 
                                                           SET IsActive = 0,
                                                               SoftDeleteTimestamp = @SoftDeleteTimestamp
                                                           WHERE EnrollmentID = @EnrollmentID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentID", enrollmentId),
                new SqlParameter("@SoftDeleteTimestamp", DateTime.Now)
            };
            await _dbCommand.InsertUpdateDataAsync(SOFT_DELETE_ENROLLMENT_QUERY, parameters);
        }

        #endregion
    }

}
