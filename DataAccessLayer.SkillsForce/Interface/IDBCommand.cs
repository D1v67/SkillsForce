using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.Interface
{
    public interface IDBCommand
    {
        DataTable GetData(string query);
        DataTable GetDataWithConditions(string query, List<SqlParameter> parameters);
        int InsertUpdateData(string query, List<SqlParameter> parameters);
    }
}
