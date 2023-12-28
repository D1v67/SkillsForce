﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.SkillsForce.Interface;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DBCommand : IDBCommand
    {
        public DataTable GetData(string query)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            DataTable resultTable = new DataTable();
            using (SqlCommand command = new SqlCommand(query, dataAccess._databaseConnection))
            {
                command.CommandType = CommandType.Text;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(resultTable);
                }
            }
            dataAccess.CloseConnection();
            return resultTable;
        }
        public  SqlDataReader GetDataReader(string query)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            SqlCommand cmd = new SqlCommand(query, dataAccess._databaseConnection)
            {
                CommandType = CommandType.Text
            };

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int InsertUpdateData(string query, List<SqlParameter> parameters)
        {
            int affectedRows = 0;
            DataAccessLayer dataAccess = new DataAccessLayer();
            using (SqlCommand command = new SqlCommand(query, dataAccess._databaseConnection))
            {
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                affectedRows = command.ExecuteNonQuery();
            }
            dataAccess.CloseConnection();
            return affectedRows;

        }

        public List<int> ExecuteQueryWithOutput(string query, List<SqlParameter> parameters, string outputColumnName)
        {
            List<int> affectedIds = new List<int>();
            DataAccessLayer dataAccess = new DataAccessLayer();

            using (SqlCommand command = new SqlCommand(query, dataAccess._databaseConnection))
            {
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int affectedId = Convert.ToInt32(reader[outputColumnName]);
                        affectedIds.Add(affectedId);
                    }
                }
            }

            dataAccess.CloseConnection();
            return affectedIds;
        }



        public int InsertDataAndReturnIdentity(string query, List<SqlParameter> parameters)
        {
            int generatedIdentity = 0;
            DataAccessLayer dataAccess = new DataAccessLayer();
            using (SqlCommand command = new SqlCommand(query + "; SELECT SCOPE_IDENTITY();", dataAccess._databaseConnection))
            {
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                generatedIdentity = Convert.ToInt32(command.ExecuteScalar());
            }
            dataAccess.CloseConnection();
            return generatedIdentity;
        }

        public DataTable GetDataWithConditions(string query, List<SqlParameter> parameters)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            DataTable resultTable = new DataTable();
            using (SqlCommand command = new SqlCommand(query, dataAccess._databaseConnection))
            {
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(resultTable);
                }
            }
            dataAccess.CloseConnection();
            return resultTable;
        }

        public  SqlDataReader GetDataWithConditionsReader(string query, List<SqlParameter> parameters)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            SqlCommand cmd = new SqlCommand(query, dataAccess._databaseConnection);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                parameters.ForEach(parameter =>
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                });
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
