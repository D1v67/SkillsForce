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

            AttachmentModel attachment;
            var dt = _dbCommand.GetData(GET_ALL_ATTACHMENTS_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                attachment = new AttachmentModel();
                attachment.EnrollmentID = int.Parse(row["EnrollmentID"].ToString());
                attachment.PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString());
              //  attachment.AttachmentURL = row["AttachmentURL"].ToString();
                attachment.FileData = row["FileData"] as byte[];
                attachment.FileName = row["FileName"].ToString();

                attachments.Add(attachment);
            }
            return attachments;
        }

        public IEnumerable<AttachmentModel> GetAllByEnrollmentID(int id)
        {
            const string GET_ALL_BY_ENROLLMENTID = @"SELECT* FROM Attachment A WHERE A.EnrollmentID = @EnrollmentID";
            List<AttachmentModel> attachments = new List<AttachmentModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@EnrollmentID", id) };
            var dt = _dbCommand.GetDataWithConditions(GET_ALL_BY_ENROLLMENTID, parameters);
            AttachmentModel attachment;

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    attachment = new AttachmentModel();
                    attachment.AttachmentID = int.Parse(row["AttachmentID"].ToString());
                    attachment.EnrollmentID = int.Parse(row["EnrollmentID"].ToString());
                    attachment.PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString());
                    attachment.FileName = row["FileName"].ToString();
                    //attachment.FileData = row["FileData"] as byte[];
                    attachments.Add(attachment);
                }
                return attachments;
            }
            return null;
        }

        public AttachmentModel GetByAttachmentID(int id)
        {
            const string GET_BY_ATTACHMENTID_QUERY = @"SELECT * FROM Attachment A WHERE A.AttachmentID = @AttachmentID";
            var parameters = new List<SqlParameter> { new SqlParameter("@AttachmentID", id) };
            var dt = _dbCommand.GetDataWithConditions(GET_BY_ATTACHMENTID_QUERY, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                AttachmentModel attachment = new AttachmentModel
                {
                    AttachmentID = int.Parse(row["AttachmentID"].ToString()),
                    EnrollmentID = int.Parse(row["EnrollmentID"].ToString()),
                    PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString()),
                    FileName = row["FileName"].ToString(),
                // AttachmentURL = row["AttachmentURL"].ToString(),
                    FileData = row["FileData"] as byte[],
                };
                return attachment;
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
