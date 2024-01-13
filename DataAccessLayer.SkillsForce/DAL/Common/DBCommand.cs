using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.SkillsForce.Interface;
using System.Threading.Tasks;
using System.Reflection;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class DBCommand : IDBCommand
    {
        #region ASync DB Commands
        public async Task<SqlDataReader> GetDataReaderAsync(string query)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            SqlCommand cmd = new SqlCommand(query, dataAccess._databaseConnection)
            {
                CommandType = CommandType.Text
            };

            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task<SqlDataReader> GetDataWithConditionsReaderAsync(string query, List<SqlParameter> parameters)
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

            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task<int> InsertUpdateDataAsync(string query, List<SqlParameter> parameters)
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

                affectedRows = await command.ExecuteNonQueryAsync();
            }

            dataAccess.CloseConnection();
            return affectedRows;
        }

        public async Task<List<int>> ExecuteQueryWithOutputAsync(string query, List<SqlParameter> parameters, string outputColumnName)
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

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int affectedId = Convert.ToInt32(reader[outputColumnName]);
                        affectedIds.Add(affectedId);
                    }
                }
            }

            dataAccess.CloseConnection();
            return affectedIds;
        }

        public async Task<int> InsertDataAndReturnIdentityAsync(string query, List<SqlParameter> parameters)
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

                var result = await command.ExecuteScalarAsync();
                generatedIdentity = Convert.ToInt32(result);
            }

            dataAccess.CloseConnection();
            return generatedIdentity;
        }

        #endregion

        #region Sync DB Commands
        public SqlDataReader GetDataReader(string query)
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            SqlCommand cmd = new SqlCommand(query, dataAccess._databaseConnection)
            {
                CommandType = CommandType.Text
            };
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader GetDataWithConditionsReader(string query, List<SqlParameter> parameters)
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

        #endregion

        #region Mapper Helper methods
        public static T MapToObject<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);
                object value = reader[i];

                SetProperty(obj, propertyName, value);
            }

            return obj;
        }

        private static void SetProperty(object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property != null && value != DBNull.Value)
            {
                Type targetType = property.PropertyType;

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                object convertedValue = Convert.ChangeType(value, targetType);
                property.SetValue(obj, convertedValue, null);
            }
        }

        #endregion
    }
}
