using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BusinessLayer.SkillsForce.Helpers;
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

        public async Task<bool> IsUserAuthenticatedAsync(AccountModel account)
        {
            const string AUTHENTICATE_USER_QUERY = @"SELECT a.HashedPassword, a.SaltValue FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email";

            if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
            {
                throw new ArgumentNullException("Email and Password cannot be null or empty.");
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", account.Email)
            };

            using (SqlDataReader reader = await _dbCommand.GetDataWithConditionsReaderAsync(AUTHENTICATE_USER_QUERY, parameters))
            {
                if (await reader.ReadAsync())
                {
                    byte[] storedHash = (byte[])reader["HashedPassword"];
                    byte[] storedSalt = (byte[])reader["SaltValue"];

                    return PasswordHasher.VerifyPassword(account.Password, storedHash, storedSalt);
                }
            }
            return false;
        }

        public async Task<AccountModel> GetUserDetailsWithRolesAsync(AccountModel account)
        {
            const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT u.UserID, u.FirstName, u.LastName, u.Email, u.NIC, u.MobileNumber, 
                                                              u.DepartmentID, d.DepartmentName, u.ManagerID, r.RoleName, r.RoleID
                                                                FROM [User] u
                                                                LEFT JOIN Department d ON u.DepartmentID = d.DepartmentID
                                                                LEFT JOIN UserRole ur ON u.UserID = ur.UserID
                                                                LEFT JOIN Role r ON ur.RoleID = r.RoleID
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

        public async Task RegisterAsync(RegisterViewModel registerViewModel)
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

            await _dbCommand.InsertUpdateDataAsync(INSERT_INTO_USER_AND_ACCOUNT_REGISTER_QUERY, parameters);
        }
    }
}











//public void Register(RegisterViewModel registerViewModel)
//{
//    const string INSERT_USER_AND_ACCOUNT_REGISTER_QUERY = @"DECLARE @key int
//                                                        INSERT INTO [dbo].[User]  ([FirstName] ,[LastName],[Email],[NIC],[MobileNumber], [ManagerID], [DepartmentID]) 
//                                                        VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @ManagerID, @DepartmentID)
//                                                        SELECT @key = @@IDENTITY
//                                                        INSERT INTO [dbo].[Account]([UserID],[Password]) 
//                                                        VALUES(@key, @Password)";

//    List<SqlParameter> parameters = new List<SqlParameter>();

//    parameters.Add(new SqlParameter("@FirstName", registerViewModel.FirstName));
//    parameters.Add(new SqlParameter("@LastName", registerViewModel.LastName));
//    parameters.Add(new SqlParameter("@NIC", registerViewModel.NIC));
//    parameters.Add(new SqlParameter("@Email", registerViewModel.Email));
//    parameters.Add(new SqlParameter("@MobileNumber", registerViewModel.MobileNumber));
//    parameters.Add(new SqlParameter("@Password", registerViewModel.Password));
//    parameters.Add(new SqlParameter("@DepartmentID", registerViewModel.DepartmentID));
//    parameters.Add(new SqlParameter("@ManagerID", registerViewModel.ManagerID));

//    _dbCommand.InsertUpdateData(INSERT_USER_AND_ACCOUNT_REGISTER_QUERY, parameters);
//}

//public static UserModel GetEmployeeDetail(LoginModel model)
//{
//    var employee = new UserModel();
//    employee = UserDAL.GetEmployees().FirstOrDefault(emp => emp.EmailAddress == model.EmailAddress);

//    return employee;
//}


//public bool IsUserAuthenticated(AccountModel account)
//{
//    const string AUTHENTICATE_USER_QUERY = @"SELECT 1 FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email AND a.Password = @Password";
//    try
//    {
//        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
//        {
//            throw new ArgumentNullException("Email and Password cannot be null or empty.");
//        }
//        List<SqlParameter> parameters = new List<SqlParameter>();
//        parameters.Add(new SqlParameter("@Email", account.Email));
//        parameters.Add(new SqlParameter("@Password", account.Password));

//        var dt = _dbCommand.GetDataWithConditions(AUTHENTICATE_USER_QUERY, parameters);
//        return dt.Rows.Count > 0;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception: {ex.Message}");
//        return false;
//    }
//}

//public bool IsUserAuthenticated(AccountModel account)
//{
//    const string AUTHENTICATE_USER_QUERY = @"SELECT 1 FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email AND a.Password = @Password";

//    try
//    {
//        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
//        {
//            throw new ArgumentNullException("Email and Password cannot be null or empty.");
//        }

//        List<SqlParameter> parameters = new List<SqlParameter>
//{
//    new SqlParameter("@Email", account.Email),
//    new SqlParameter("@Password", account.Password)
//};

//        using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(AUTHENTICATE_USER_QUERY, parameters))
//        {
//            return reader.HasRows;
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception: {ex.Message}");
//        return false;
//    }

//}



//public bool IsUserAuthenticated(AccountModel account)
//{
//    const string AUTHENTICATE_USER_QUERY = @"SELECT u.UserID, a.HashedPassword, a.SaltValue FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email";
//    try
//    {
//        if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
//        {
//            throw new ArgumentNullException("Email and Password cannot be null or empty.");
//        }

//        List<SqlParameter> parameters = new List<SqlParameter>();
//        parameters.Add(new SqlParameter("@Email", account.Email));

//        var user = _dbCommand.GetDataWithConditions(AUTHENTICATE_USER_QUERY, parameters);

//        if (user != null)
//        {
//            // Retrieve hashed password and salt from the database
//            byte[] storedHash = (byte[])user["HashedPassword"];
//            byte[] storedSalt = (byte[])user["SaltValue"];

//            // Use PasswordHasher to verify the password
//            return PasswordHasher.VerifyPassword(account.Password, storedHash, storedSalt);
//        }

//        return false;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception: {ex.Message}");
//        return false;
//    }

//}
//public AccountModel GetUserDetailsWithRoles(AccountModel account)
//{
//    const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT
//                                                        u.UserID,
//                                                        u.FirstName,
//                                                        u.LastName,
//                                                        u.Email,
//                                                        u.NIC,
//                                                        u.MobileNumber,
//                                                        u.DepartmentID,
//                                                        d.DepartmentName,
//                                                        u.ManagerID,
//                                                        r.RoleName,
//                                                        r.RoleID
//                                                    FROM
//                                                        [User] u
//                                                    LEFT JOIN
//                                                        Department d ON u.DepartmentID = d.DepartmentID
//                                                    LEFT JOIN
//                                                        UserRole ur ON u.UserID = ur.UserID
//                                                    LEFT JOIN
//                                                        Role r ON ur.RoleID = r.RoleID
//                                                    WHERE u.Email = @Email";
//    AccountModel user = null;
//    List<SqlParameter> parameters = new List<SqlParameter>();
//    parameters.Add(new SqlParameter("@Email", account.Email));

//    using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_USER_DETAILS_WITH_ROLE_QUERY, parameters))
//    {
//        if (reader.Read())
//        {
//            user = new AccountModel
//            {
//                UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
//                RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
//                Email = reader.GetString(reader.GetOrdinal("Email")).Trim(),
//                RoleId = reader.GetByte(reader.GetOrdinal("RoleId")),
//                FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
//            };
//        }
//    }

//    return user;
//}


//public AccountModel GetUserDetailsWithRoles(AccountModel account)
//{
//    const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT u.*, r.* FROM [User] u  INNER JOIN Role r  ON u.RoleID = r.RoleID  WHERE u.Email = @Email";
//    AccountModel user = new AccountModel();
//    List<SqlParameter> parameters = new List<SqlParameter>();
//    parameters.Add(new SqlParameter("@Email", account.Email));

//    var dt = _dbCommand.GetDataWithConditions(GET_USER_DETAILS_WITH_ROLE_QUERY, parameters);
//    foreach (DataRow row in dt.Rows)
//    {
//        user.UserID = int.Parse(row["UserID"].ToString());
//        user.RoleName = row["RoleType"].ToString();
//        user.Email = row["Email"].ToString().Trim();
//        user.RoleId = int.Parse(row["RoleId"].ToString());
//        user.FirstName = row["FirstName"].ToString();
//    }
//    return user;
//}

//public void AddAccount(AccountModel account)
//{
//    const string INSERT_ACCOUNT_QUERY = @"INSERT INTO [dbo].[Account]([UserID], [Password])  VALUES (@UserID, @Password)";
//    List<SqlParameter> parameters = new List<SqlParameter>();
//    parameters.Add(new SqlParameter("@UserID", account.UserID));
//    parameters.Add(new SqlParameter("@Password", account.Password));

//    _dbCommand.InsertUpdateData(INSERT_ACCOUNT_QUERY, parameters);
//}