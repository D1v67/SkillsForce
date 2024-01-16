using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class AccountDAL : IAccountDAL
    {
        private readonly IDBCommand _dbCommand;

        public AccountDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public async Task<AccountModel> GetUserCredentialsAsync(string email)
        {
            const string GET_USER_CREDENTIALS_QUERY = @"SELECT a.HashedPassword, a.SaltValue FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email";

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email cannot be null or empty.");
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_USER_CREDENTIALS_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    byte[] storedHash = (byte[])reader["HashedPassword"];
                    byte[] storedSalt = (byte[])reader["SaltValue"];

                    return new AccountModel
                    {
                        HashedPassword = storedHash,
                        SaltValue = storedSalt
                    };
                }
            }
            return null;
        }


        public async Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel account)
        {
            const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT u.UserID, u.FirstName, u.LastName, u.Email, r.RoleName, r.RoleID
            FROM [User] u LEFT JOIN Department d ON u.DepartmentID = d.DepartmentID LEFT JOIN UserRole ur ON u.UserID = ur.UserID LEFT JOIN Role r ON ur.RoleID = r.RoleID
            WHERE u.Email = @Email";

            AccountModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", account.Email));
            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(GET_USER_DETAILS_WITH_ROLE_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    user = new AccountModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")).Trim(),
                        listOfRoles = new List<UserRoleModel>()
                    };
                    do
                    {
                        UserRoleModel role = new UserRoleModel
                        {
                            UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                            RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                            RoleID = reader.GetByte(reader.GetOrdinal("RoleID"))
                        };
                        user.listOfRoles.Add(role);
                    } while (await reader.ReadAsync());
                }
            }
            return user;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel)
        {
            const string INSERT_INTO_USER_AND_ACCOUNT_REGISTER_QUERY = @"
            BEGIN TRANSACTION
            DECLARE @key INT
            INSERT INTO [dbo].[User] ([FirstName], [LastName], [Email], [NIC], [MobileNumber], [ManagerID], [DepartmentID])
            VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @ManagerID, @DepartmentID)
            SELECT @key = @@IDENTITY
            INSERT INTO [dbo].[Account] ([UserID], [HashedPassword], [SaltValue]) 
            VALUES (@key, @HashedPassword, @SaltValue)
            INSERT INTO [dbo].[UserRole] ([UserID])
            VALUES (@key)
            COMMIT";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FirstName", registerViewModel.FirstName));
            parameters.Add(new SqlParameter("@LastName", registerViewModel.LastName));
            parameters.Add(new SqlParameter("@NIC", registerViewModel.NIC));
            parameters.Add(new SqlParameter("@Email", registerViewModel.Email));
            parameters.Add(new SqlParameter("@MobileNumber", registerViewModel.MobileNumber));
            parameters.Add(new SqlParameter("@DepartmentID", registerViewModel.DepartmentID));
            parameters.Add(new SqlParameter("@ManagerID", registerViewModel.ManagerID));
            parameters.Add(new SqlParameter("@HashedPassword", registerViewModel.HashedPassword)); // Item1 is the hashed password
            parameters.Add(new SqlParameter("@SaltValue", registerViewModel.SaltValue)); // Item2 is the salt value

           return  await _dbCommand.InsertUpdateDataAsync(INSERT_INTO_USER_AND_ACCOUNT_REGISTER_QUERY, parameters)>0;
        }
    }
}