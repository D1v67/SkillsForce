using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Common.SkillsForce.Enums;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly IDBCommand _dbCommand;
        public UserDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        #region Add
        public async Task AddAsync(UserModel user)
        {
            const string INSERT_USER_QUERY = @"INSERT INTO [dbo].[User] ([FirstName],[LastName],[Email],[NIC],[MobileNumber],[RoleID],[DepartmentID],[ManagerID]) 
                                           VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @RoleID, @DepartmentID, @ManagerID)";
            List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@FirstName", user.FirstName),
            new SqlParameter("@LastName", user.LastName),
            new SqlParameter("@Email", user.Email),
            new SqlParameter("@NIC", user.NIC),
            new SqlParameter("@MobileNumber", user.MobileNumber),
            new SqlParameter("@DepartmentID", user.DepartmentID),
            new SqlParameter("@ManagerID", user.ManagerID)
        };

            await _dbCommand.InsertUpdateDataAsync(INSERT_USER_QUERY, parameters);
        }
        #endregion

        #region Get
        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            const string GET_ALL_USER_QUERY = @"SELECT * FROM [dbo].[User]";
            List<UserModel> users = new List<UserModel>();

            using (SqlDataReader reader = await _dbCommand.GetDataReaderAsync(GET_ALL_USER_QUERY))
            {
                while (await reader.ReadAsync())
                {
                    UserModel user = new UserModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        ManagerID = reader.IsDBNull(reader.GetOrdinal("ManagerID"))
                        ? (int?)null
                        : reader.GetInt16(reader.GetOrdinal("ManagerID"))
                        };
                    users.Add(user);
                }
            }

            return users;
        }

        public async Task<UserModel> GetByIDAsync(int id)
        {
            const string GET_USER_BY_ID_QUERY = @"SELECT * FROM [dbo].[User] WHERE UserID = @UserID";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_USER_BY_ID_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    UserModel user = new UserModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        //ManagerID =reader.GetInt32(reader.GetOrdinal("ManagerID"))
                    };
                    return user;
                }
            }

            return null;
        }

        public async Task<IEnumerable<UserModel>> GetAllManagerAsync()
        {
            const string GET_ALL_MANAGERS = @"SELECT U.UserID, U.FirstName, U.LastName FROM [User] U JOIN UserRole UR ON U.UserID = UR.UserID JOIN Role R ON UR.RoleID = R.RoleID WHERE R.RoleName = @RoleName";
            List<UserModel> managers = new List<UserModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@RoleName", RolesEnum.Manager.ToString()) };
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_ALL_MANAGERS, parameters))
            {
                while (await reader.ReadAsync())
                {
                    UserModel manager = new UserModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    };
                    managers.Add(manager);
                }
            }

            return managers.Count > 0 ? managers : null;
        }

        #endregion

        #region User Operations
        public async Task DeleteAsync(int id)
        {
            const string DELETE_USER_QUERY = @"DELETE FROM [dbo].[User] WHERE [UserID] = @UserID";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };
            await _dbCommand.InsertUpdateDataAsync(DELETE_USER_QUERY, parameters);
        }

        public async Task UpdateAsync(UserModel user)
        {
            const string UPDATE_USER_QUERY = @"UPDATE [dbo].[User] SET [FirstName] = @FirstName,[LastName] = @LastName,[Email] = @Email,[NIC] = @NIC,[MobileNumber] = @MobileNumber,[RoleID] = @RoleID,[DepartmentID] = @DepartmentID,[ManagerID] = @ManagerID WHERE [UserID] = @UserID";
            List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@FirstName", user.MobileNumber),
            new SqlParameter("@LastName", user.LastName),
            new SqlParameter("@Email", user.Email),
            new SqlParameter("@NIC", user.NIC),
            new SqlParameter("@MobileNumber", user.MobileNumber),
            new SqlParameter("@DepartmentID", user.DepartmentID),
            new SqlParameter("@ManagerID", user.ManagerID)
        };

            await _dbCommand.InsertUpdateDataAsync(UPDATE_USER_QUERY, parameters);
        }

        public async Task<bool> IsEmailAlreadyExistsAsync(string email)
        {
            const string IS_EMAIL_ALREADY_EXIST_QUERY = "SELECT 1 FROM [User] WHERE Email = @Email";
            var parameters = new List<SqlParameter> { new SqlParameter("@Email", email) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(IS_EMAIL_ALREADY_EXIST_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task<bool> IsNICExistsAsync(string nic)
        {
            const string IS_NIC_ALREADY_EXIST_QUERY = "SELECT 1 FROM [User] WHERE NIC = @NIC";
            var parameters = new List<SqlParameter> { new SqlParameter("@NIC", nic) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(IS_NIC_ALREADY_EXIST_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task<bool> IsMobileNumberExistsAsync(string mobileNumber)
        {
            const string IS_MOBILE_NUMBER_ALREADY_EXIST_QUERY = "SELECT 1 FROM [User] WHERE MobileNumber = @MobileNumber";
            var parameters = new List<SqlParameter> { new SqlParameter("@MobileNumber", mobileNumber) };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(IS_MOBILE_NUMBER_ALREADY_EXIST_QUERY, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        #endregion
    }
}
