using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DataAccessLayer
    {
        //public const string connectionstring = @"server=localhost;database=EmployeeTrainingDB;uid=wbpoc;pwd=sql@tfs2008";
        public string databaseConnectionstring = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        public SqlConnection _databaseConnection;

        public DataAccessLayer()
        {
            _databaseConnection = new SqlConnection(databaseConnectionstring);
            OpenConnection();
        }
        public void OpenConnection()
        {
            try
            {
                if (_databaseConnection.State == System.Data.ConnectionState.Open)
                {
                    _databaseConnection.Close();
                }
                _databaseConnection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            if (_databaseConnection != null && _databaseConnection.State == System.Data.ConnectionState.Open)
            {
                _databaseConnection.Close();
                _databaseConnection.Dispose();
            }
        }
    }
}
