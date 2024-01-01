using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IDBCommand
    {
        Task<SqlDataReader> GetDataReaderAsync(string query);
        Task<SqlDataReader> GetDataWithConditionsReaderAsync(string query, List<SqlParameter> parameters);
        Task<int> InsertUpdateDataAsync(string query, List<SqlParameter> parameters);
        Task<List<int>> ExecuteQueryWithOutputAsync(string query, List<SqlParameter> parameters, string outputColumnName);
        Task<int> InsertDataAndReturnIdentityAsync(string query, List<SqlParameter> parameters);

        DataTable GetData(string query);
        DataTable GetDataWithConditions(string query, List<SqlParameter> parameters);
        int InsertUpdateData(string query, List<SqlParameter> parameters);
        int InsertDataAndReturnIdentity(string query, List<SqlParameter> parameters);


        SqlDataReader GetDataReader(string query);
        SqlDataReader GetDataWithConditionsReader(string query, List<SqlParameter> parameters);

        List<int> ExecuteQueryWithOutput(string query, List<SqlParameter> parameters, string outputColumnName);
    }
}
