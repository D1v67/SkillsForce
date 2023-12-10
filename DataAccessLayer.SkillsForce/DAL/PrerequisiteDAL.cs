using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{

    public class PrerequisiteDAL : IPrerequisiteDAL
    {
        private readonly IDBCommand _dbCommand;

        public PrerequisiteDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public IEnumerable<PrerequisiteModel> GetPrerequisiteByTrainingID(int TrainingID)
        {
            const string GET_PREREQUISITE_BY_TRAINING_ID_QUERY = @"SELECT P.PrerequisiteID, TP.TrainingID, T.TrainingName, P.PrerequisiteName
                                                                    FROM TrainingPrerequisite TP
                                                                    JOIN Training T ON TP.TrainingID = T.TrainingID
                                                                    JOIN Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID
                                                                    WHERE T.TrainingID = @TrainingID";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();
            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", TrainingID) };
            var dt = _dbCommand.GetDataWithConditions(GET_PREREQUISITE_BY_TRAINING_ID_QUERY, parameters);
            PrerequisiteModel prerequisite;

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    prerequisite = new PrerequisiteModel();
                    prerequisite.PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString());
                    prerequisite.PrerequisiteName = row["PrerequisiteName"].ToString();

                    prerequisites.Add(prerequisite);
                }
                return prerequisites;
            }
            return null;
        }
        public void Add(PrerequisiteModel prerequisite)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            const string GET_ALL_PREREQUISITE_QUERY = @"SELECT * FROM Prerequisite";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            PrerequisiteModel prerequisite;
            var dt = _dbCommand.GetData(GET_ALL_PREREQUISITE_QUERY);
            foreach (DataRow row in dt.Rows)
            {
                prerequisite = new PrerequisiteModel();
                prerequisite.PrerequisiteID = int.Parse(row["PrerequisiteID"].ToString());
                prerequisite.PrerequisiteName = row["PrerequisiteName"].ToString();

                prerequisites.Add(prerequisite);
            }
            return prerequisites;
        }

        public PrerequisiteModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PrerequisiteModel prerequisite)
        {
            throw new NotImplementedException();
        }
    }
}
