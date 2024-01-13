using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class UserRoleDAL : IUserRoleDAL
    {
        private readonly IDBCommand _dbCommand;

        public UserRoleDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public void Add(UserRoleModel userRole)
        {
            const string INSERT_USER_ROLE_QUERY = @"INSERT INTO [dbo].[UserRole] ([UserID],[RoleID]) 
                                                 VALUES (@UserID, @RoleID)";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@UserID", userRole.UserID));
            parameters.Add(new SqlParameter("@RoleID", userRole.RoleID));

            _dbCommand.InsertUpdateData(INSERT_USER_ROLE_QUERY, parameters);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRoleModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserRoleModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserRoleModel prerequisite)
        {
            throw new NotImplementedException();
        }
    }


}
