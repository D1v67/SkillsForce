using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
        public async Task<IEnumerable<PrerequisiteModel>> GetPrerequisiteByTrainingIDAsync(int TrainingID)
        {
            const string GET_PREREQUISITE_BY_TRAINING_ID_QUERY = @"SELECT P.PrerequisiteID, P.PrerequisiteName
                                                                    FROM TrainingPrerequisite TP
                                                                    JOIN Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID
                                                                    WHERE TP.TrainingID = @TrainingID";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", TrainingID) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_PREREQUISITE_BY_TRAINING_ID_QUERY, parameters))
            {
                while (reader.Read())
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                    };

                    prerequisites.Add(prerequisite);
                }
            }

            return prerequisites.Count > 0 ? prerequisites : null;
        }

        public async Task<IEnumerable<PrerequisiteModel>> GetAllAsync()
        {
            const string GET_ALL_PREREQUISITE_QUERY = @"SELECT * FROM Prerequisite";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_PREREQUISITE_QUERY))
            {
                while (reader.Read())
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                    };

                    prerequisites.Add(prerequisite);
                }
            }

            return prerequisites;
        }

        public IEnumerable<PrerequisiteModel> GetPrerequisiteByTrainingID(int TrainingID)
        {
            const string GET_PREREQUISITE_BY_TRAINING_ID_QUERY = @"SELECT P.PrerequisiteID, P.PrerequisiteName
                                                            FROM TrainingPrerequisite TP
                                                            JOIN Prerequisite P ON TP.PrerequisiteID = P.PrerequisiteID
                                                            WHERE TP.TrainingID = @TrainingID";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@TrainingID", TrainingID) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_PREREQUISITE_BY_TRAINING_ID_QUERY, parameters))
            {
                while (reader.Read())
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                    };
                    prerequisites.Add(prerequisite);
                }
            }
            return prerequisites.Count > 0 ? prerequisites : null;
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            const string GET_ALL_PREREQUISITE_QUERY = @"SELECT * FROM Prerequisite";
            List<PrerequisiteModel> prerequisites = new List<PrerequisiteModel>();

            using (SqlDataReader reader = _dbCommand.GetDataReader(GET_ALL_PREREQUISITE_QUERY))
            {
                while (reader.Read())
                {
                    PrerequisiteModel prerequisite = new PrerequisiteModel
                    {
                        PrerequisiteID = reader.GetByte(reader.GetOrdinal("PrerequisiteID")),
                        PrerequisiteName = reader.GetString(reader.GetOrdinal("PrerequisiteName"))
                    };

                    prerequisites.Add(prerequisite);
                }
            }

            return prerequisites;
        }

    }
}
