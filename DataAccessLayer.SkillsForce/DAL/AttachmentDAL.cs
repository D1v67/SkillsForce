using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
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
                   // attachment.AttachmentURL = row["AttachmentURL"].ToString();

                    // Assuming the FileData column is of type byte[] in the database
                    attachment.FileData = row["FileData"] as byte[];

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
                   // AttachmentURL = row["AttachmentURL"].ToString(),

                    // Assuming the FileData column is of type byte[] in the database
                    FileData = row["FileData"] as byte[],

                    //FileName = row["FileName"].ToString()
                };
                return attachment;
            }
            return null;
        }

        public void Add(AttachmentModel attachment)
        {
            const string INSERT_EVIDENCE_QUERY = @"
            INSERT INTO Attachment (EnrollmentID, PrerequisiteID, FileData)
            VALUES (@EnrollmentID, @PrerequisiteID, @FileData)";

            List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@EnrollmentID", attachment.EnrollmentID),
            new SqlParameter("@PrerequisiteID", attachment.PrerequisiteID),
            new SqlParameter("@FileData", attachment.FileData),

        };

            _dbCommand.InsertUpdateData(INSERT_EVIDENCE_QUERY, parameters);
        }

        public AttachmentModel GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
