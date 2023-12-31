using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class UserAuthorizationDAL : IUserAuthorizationDAL
    {
        private readonly IDBCommand _dbCommand;
        public UserAuthorizationDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public bool IsUserHavePermission(int userID, string permission)
        {
            const string IS_USER_HAVE_PERMISSION_QUERY = @"SELECT 1
                                                    FROM [User] U
                                                    JOIN UserRole UR ON U.UserID = UR.UserID
                                                    JOIN Role R ON UR.RoleID = R.RoleID
                                                    JOIN RolePermission RP ON R.RoleID = RP.RoleID
                                                    JOIN Permission P ON RP.PermissionID = P.PermissionID
                                                    WHERE U.UserID = @UserID AND P.PermissionName = @PermissionName";

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserID", userID), 
                    new SqlParameter("@PermissionName", permission)
                };

                using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(IS_USER_HAVE_PERMISSION_QUERY, parameters))
                {
                    return reader.HasRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}
