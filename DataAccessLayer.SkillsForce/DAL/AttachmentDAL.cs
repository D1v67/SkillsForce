using Common.SkillsForce.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    }
}
