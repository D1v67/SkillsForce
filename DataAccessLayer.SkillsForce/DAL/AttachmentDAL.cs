using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class AttachmentDAL : IAttachmentDAL
    {
        private readonly DBCommand _dbCommand;
        public AttachmentDAL(DBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }
        public IEnumerable<AttachmentModel> GetAll()
        {
            const string GET_ALL_ATTACHMENTS_QUERY = @"SELECT * FROM [dbo].[Attachment]";
            List<AttachmentModel> attachments = new List<AttachmentModel>();

            using (SqlDataReader reader = _dbCommand.GetDataReader(GET_ALL_ATTACHMENTS_QUERY))
            {
                while (reader.Read())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        // AttachmentURL = reader["AttachmentURL"] == DBNull.Value ? null : reader["AttachmentURL"].ToString(),
                        FileData = (byte[])reader["FileData"],
                        FileName = reader.GetString(reader.GetOrdinal("FileName"))
                    };

                    attachments.Add(attachment);
                }
            }
            return attachments;
        }

        public IEnumerable<AttachmentModel> GetAllByEnrollmentID(int id)
        {
            const string GET_ALL_BY_ENROLLMENTID = @"SELECT * FROM Attachment A WHERE A.EnrollmentID = @EnrollmentID";
            List<AttachmentModel> attachments = new List<AttachmentModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@EnrollmentID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_ALL_BY_ENROLLMENTID, parameters))
            {
                while (reader.Read())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        FileName = reader.GetString(reader.GetOrdinal("FileName")),
                        // FileData = reader["FileData"] == DBNull.Value ? null : (byte[])reader["FileData"]
                    };

                    attachments.Add(attachment);
                }
            }

            return attachments.Count > 0 ? attachments : null;
        }


        public AttachmentModel GetByAttachmentID(int id)
        {
            const string GET_BY_ATTACHMENTID_QUERY = @"SELECT * FROM Attachment A WHERE A.AttachmentID = @AttachmentID";
            var parameters = new List<SqlParameter> { new SqlParameter("@AttachmentID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_BY_ATTACHMENTID_QUERY, parameters))
            {
                if (reader.Read())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        FileName = reader.GetString(reader.GetOrdinal("FileName")),
                        // AttachmentURL = reader["AttachmentURL"] == DBNull.Value ? null : reader["AttachmentURL"].ToString(),
                        FileData = reader["FileData"] as byte[]
                    };

                    return attachment;
                }
            }

            return null;
        }

        public void Add(AttachmentModel attachment)
        {
            const string INSERT_EVIDENCE_QUERY = @"INSERT INTO Attachment (EnrollmentID, PrerequisiteID, FileName, FileData)
                                                   VALUES (@EnrollmentID, @PrerequisiteID, @FileName, @FileData)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentID", attachment.EnrollmentID),
                new SqlParameter("@PrerequisiteID", attachment.PrerequisiteID),
                new SqlParameter("@FileData", attachment.FileData),
                new SqlParameter("@FileName", attachment.FileName),
            };
            _dbCommand.InsertUpdateData(INSERT_EVIDENCE_QUERY, parameters);
        }

        public AttachmentModel GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
