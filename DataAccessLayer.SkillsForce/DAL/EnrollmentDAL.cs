﻿using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.SkillsForce.ViewModel;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly IDBCommand _dbCommand;

        public EnrollmentDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public int Add(EnrollmentViewModel enrollment)
        {
            const string INSERT_ENROLLMENT_QUERY = @"INSERT INTO [dbo].[Enrollment] ([UserID], [TrainingID]) OUTPUT INSERTED.EnrollmentID VALUES (@UserID, @TrainingID)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", enrollment.UserID),
                new SqlParameter("@TrainingID", enrollment.TrainingID)
            };
            int generatedEnrollmentId = _dbCommand.InsertDataAndReturnIdentity(INSERT_ENROLLMENT_QUERY, parameters);
            return generatedEnrollmentId;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EnrollmentViewModel> GetAll()
        {
            const string GET_ALL_ENROLLMENT_QUERY = @"SELECT * FROM [dbo].[Enrollment]";
            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            EnrollmentViewModel enrollment;
            var dt = _dbCommand.GetData(GET_ALL_ENROLLMENT_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                enrollment = new EnrollmentViewModel();
                enrollment.EnrollmentID = int.Parse(row["EnrollmentID"].ToString());
                enrollment.UserID = int.Parse(row["UserID"].ToString());
                enrollment.TrainingID = int.Parse(row["TrainingID"].ToString());
                enrollment.EnrollmentDate = (DateTime)row["EnrollmentDate"];
                enrollment.EnrollmentStatus = row["EnrollmentStatus"].ToString();
                enrollments.Add(enrollment);
            }
            return enrollments;
        }
        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetails()
        {
            const string GET_ALL_ENROLLMENT_WITH_DETAILS_QUERY =
            @"SELECT E.EnrollmentID, U.UserID, U.FirstName,U.LastName,T.TrainingID, TrainingName, D.DepartmentName,E.EnrollmentDate, E.EnrollmentStatus
              FROM Enrollment E JOIN [User] U ON E.UserID = U.UserID JOIN Training T ON E.TrainingID = T.TrainingID JOIN Department D ON T.DepartmentID = D.DepartmentID";

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            EnrollmentViewModel enrollment;
            var dt = _dbCommand.GetData(GET_ALL_ENROLLMENT_WITH_DETAILS_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                enrollment = new EnrollmentViewModel();
                enrollment.EnrollmentID = int.Parse(row["EnrollmentID"].ToString());
                enrollment.UserID = int.Parse(row["UserID"].ToString());
                enrollment.FirstName = row["FirstName"].ToString();
                enrollment.LastName = row["LastName"].ToString();
                enrollment.TrainingID = int.Parse(row["TrainingID"].ToString());
                enrollment.TrainingName = row["TrainingName"].ToString();
                enrollment.DepartmentName = row["DepartmentName"].ToString();
                enrollment.EnrollmentDate = (DateTime)row["EnrollmentDate"];
                enrollment.EnrollmentStatus = row["EnrollmentStatus"].ToString();
                enrollments.Add(enrollment);
            }
            return enrollments;
        }
        public IEnumerable<EnrollmentViewModel> GetAllEnrollmentsWithDetailsByManager(int managerId)
        {
            const string GET_ALL_ENROLLMENT_WITH_DETAILS_BY_MANAGER_QUERY =
            @"SELECT E.EnrollmentID, U.UserID,U.FirstName, U.LastName, T.TrainingID, T.TrainingName, D.DepartmentName, E.EnrollmentDate,E.EnrollmentStatus
              FROM Enrollment E JOIN [User] U ON E.UserID = U.UserID JOIN Training T ON E.TrainingID = T.TrainingID JOIN Department D ON T.DepartmentID = D.DepartmentID
              WHERE U.ManagerID = @ManagerID";

            var parameters = new List<SqlParameter> { new SqlParameter("@ManagerID", managerId) };
            var dt = _dbCommand.GetDataWithConditions(GET_ALL_ENROLLMENT_WITH_DETAILS_BY_MANAGER_QUERY, parameters);

            List<EnrollmentViewModel> enrollments = new List<EnrollmentViewModel>();

            EnrollmentViewModel enrollment;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    enrollment = new EnrollmentViewModel();
                    enrollment.EnrollmentID = int.Parse(row["EnrollmentID"].ToString());
                    enrollment.UserID = int.Parse(row["UserID"].ToString());
                    enrollment.FirstName = row["FirstName"].ToString();
                    enrollment.LastName = row["LastName"].ToString();
                    enrollment.TrainingID = int.Parse(row["TrainingID"].ToString());
                    enrollment.TrainingName = row["TrainingName"].ToString();
                    enrollment.DepartmentName = row["DepartmentName"].ToString();
                    enrollment.EnrollmentDate = (DateTime)row["EnrollmentDate"];
                    enrollment.EnrollmentStatus = row["EnrollmentStatus"].ToString();
                    enrollments.Add(enrollment);
                }
                return enrollments;
            }
            return null;
        }

        public EnrollmentViewModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(EnrollmentViewModel enrollment)
        {
            throw new NotImplementedException();
        }

        public void ApproveEnrollment(int enrollmentId)
        {
            const string UPDATE_STATUS_APPROVED_QUERY = @"UPDATE Enrollment SET EnrollmentStatus = 'Approved' WHERE EnrollmentID = @EnrollmentID";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentID", enrollmentId));
  
            _dbCommand.InsertUpdateData(UPDATE_STATUS_APPROVED_QUERY, parameters);
        }

        public void RejectEnrollment(int enrollmentId, string rejectionReason, int declinedByUserId)
        {
            const string UPDATE_STATUS_REJECTED_QUERY = @"
                UPDATE Enrollment
                SET EnrollmentStatus = 'Rejected',
                    DeclineReason = @DeclineReason,
                    DeclinedByUserId = @DeclinedByUserId,
                    LastModifiedTimestamp = GETDATE(),
                    LastModifiedUserId = @LastModifiedUserId
                WHERE EnrollmentID = @EnrollmentID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentID", enrollmentId),
                new SqlParameter("@DeclineReason", (object)rejectionReason ?? DBNull.Value),
                new SqlParameter("@DeclinedByUserId", (object)declinedByUserId ?? DBNull.Value),
                new SqlParameter("@LastModifiedUserId", declinedByUserId)
            };

            _dbCommand.InsertUpdateData(UPDATE_STATUS_REJECTED_QUERY, parameters);
        }

        public EnrollmentNotificationViewModel GetEnrollmentNotificationDetailsByID(int id)
        {
            const string GET_ENROLLMENT_NOTIFICATION_DETAILS_BY_ID =
                @"SELECT
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
            var dt = _dbCommand.GetDataWithConditions(GET_ENROLLMENT_NOTIFICATION_DETAILS_BY_ID, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                EnrollmentNotificationViewModel enrollment = new EnrollmentNotificationViewModel
                {
                    EnrollmentID = int.Parse(row["EnrollmentID"].ToString()),
                    AppUserID = int.Parse(row["AppUserID"].ToString()),
                    AppUserFirstName = row["AppUserFirstName"].ToString(),
                    AppUserLastName = row["AppUserLastName"].ToString(),
                    AppUserEmail = row["AppUserEmail"].ToString(),
                    TrainingID = int.Parse(row["TrainingID"].ToString()),
                    TrainingName = row["TrainingName"].ToString(),
                    DepartmentName = row["DepartmentName"].ToString(),
                    EnrollmentDate = DateTime.Parse(row["EnrollmentDate"].ToString()),
                    EnrollmentStatus = row["EnrollmentStatus"].ToString(),
                    ManagerID = int.Parse(row["ManagerID"].ToString()),
                    ManagerEmail = row["ManagerEmail"].ToString(),
                    ManagerFirstName = row["ManagerFirstName"].ToString(),
                    ManagerLastName = row["ManagerLastName"].ToString(),
                    DeclineReason = row["DeclineReason"].ToString(),
                };
                return enrollment;
            }
            return null;
        }
    }
}
