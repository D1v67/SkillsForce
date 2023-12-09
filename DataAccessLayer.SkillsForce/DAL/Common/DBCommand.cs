using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.SkillsForce.Interface;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DBCommand : IDBCommand
    {
        //private readonly IDataAccessLayer _dal;
        //public DBCommand(IDataAccessLayer dal)
        //{
        //    _dal = dal;
        //}
        public DataTable GetData(string query)
        {
            DataAccessLayer dal = new DataAccessLayer();
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
            dal.CloseConnection();

            return dt;
        }

        public int InsertUpdateData(string query, List<SqlParameter> parameters)
        {
            int numRows = 0;
            DataAccessLayer dal = new DataAccessLayer();

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                numRows = cmd.ExecuteNonQuery();
            }
            dal.CloseConnection();

            return numRows;

        }

        public int InsertDataAndReturnIdentity(string query, List<SqlParameter> parameters)
        {
            int generatedId = 0;
            DataAccessLayer dal = new DataAccessLayer();

            using (SqlCommand cmd = new SqlCommand(query + "; SELECT SCOPE_IDENTITY();", dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                generatedId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            dal.CloseConnection();

            return generatedId;
        }

        public DataTable GetDataWithConditions(string query, List<SqlParameter> parameters)
        {
            DataAccessLayer dal = new DataAccessLayer();
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }

            dal.CloseConnection();

            return dt;
        }
    }
}
