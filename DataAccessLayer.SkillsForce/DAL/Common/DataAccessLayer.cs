using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DataAccessLayer
    {
        public string databaseConnectionstring = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        public SqlConnection _databaseConnection;

        public DataAccessLayer()
        {
            _databaseConnection = new SqlConnection(databaseConnectionstring);
            OpenConnection();
        }
        public async Task OpenConnectionAsync()
        {
            try
            {
                if (_databaseConnection.State == System.Data.ConnectionState.Open)
                {
                    _databaseConnection.Close();
                }

                await _databaseConnection.OpenAsync();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void OpenConnection()
        {
            //Task.Run(() => OpenConnectionAsync()).Wait();
            if (_databaseConnection.State == System.Data.ConnectionState.Open)
            {
                _databaseConnection.Close();
            }
            _databaseConnection.Open();
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


