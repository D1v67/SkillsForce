using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class AttachmentDAL : IAttachmentDAL
    {
        private readonly DBCommand _dbCommand;
        public AttachmentDAL(DBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public async Task<IEnumerable<AttachmentModel>> GetAllAsync()
        {
            const string GET_ALL_ATTACHMENTS_QUERY = @"SELECT * FROM [dbo].[Attachment]";
            List<AttachmentModel> attachments = new List<AttachmentModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_ATTACHMENTS_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        FileData = (byte[])reader["FileData"],
                        FileName = reader.GetString(reader.GetOrdinal("FileName"))
                    };

                    attachments.Add(attachment);
                }
            }

            return attachments;
        }

        public async Task<IEnumerable<AttachmentModel>> GetAllByEnrollmentIDAsync(int id)
        {
            const string GET_ALL_BY_ENROLLMENTID = @"SELECT * FROM Attachment A WHERE A.EnrollmentID = @EnrollmentID";
            List<AttachmentModel> attachments = new List<AttachmentModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@EnrollmentID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_BY_ENROLLMENTID, parameters))
            {
                while (await reader.ReadAsync())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        FileName = reader.GetString(reader.GetOrdinal("FileName")),
                    };

                    attachments.Add(attachment);
                }
            }

            return attachments.Count > 0 ? attachments : null;
        }

        public async Task<AttachmentModel> GetByAttachmentIDAsync(int id)
        {
            const string GET_BY_ATTACHMENTID_QUERY = @"SELECT * FROM Attachment A WHERE A.AttachmentID = @AttachmentID";
            var parameters = new List<SqlParameter> { new SqlParameter("@AttachmentID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_BY_ATTACHMENTID_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    AttachmentModel attachment = new AttachmentModel
                    {
                        AttachmentID = reader.GetInt32(reader.GetOrdinal("AttachmentID")),
                        EnrollmentID = reader.GetInt16(reader.GetOrdinal("EnrollmentID")),
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        FileName = reader.GetString(reader.GetOrdinal("FileName")),
                        FileData = reader["FileData"] as byte[]
                    };

                    return attachment;
                }
            }

            return null;
        }

        public async Task AddAsync(AttachmentModel attachment)
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

            await _dbCommand.InsertUpdateDataAsync(INSERT_EVIDENCE_QUERY, parameters);
        }

        public Task<AttachmentModel> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }




    }
}
