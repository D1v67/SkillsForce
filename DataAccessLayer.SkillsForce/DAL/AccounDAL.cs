using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
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
        public bool IsUserAuthenticated(AccountModel account)
        {
            const string AUTHENTICATE_USER_QUERY = @"SELECT 1 FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email AND a.Password = @Password";
            try
            {
                if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
                {
                    throw new ArgumentNullException("Email and Password cannot be null or empty.");
                }
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Email", account.Email));
                parameters.Add(new SqlParameter("@Password", account.Password));

                var dt = _dbCommand.GetDataWithConditions(AUTHENTICATE_USER_QUERY, parameters);
                return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        public AccountModel GetUserDetailsWithRoles(AccountModel account)
        {
            const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT u.*, r.* FROM [User] u  INNER JOIN Role r  ON u.RoleID = r.RoleID  WHERE u.Email = @Email";
            AccountModel user = new AccountModel();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", account.Email));

            var dt = _dbCommand.GetDataWithConditions(GET_USER_DETAILS_WITH_ROLE_QUERY, parameters);
            foreach (DataRow row in dt.Rows)
            {
                user.UserID = int.Parse(row["UserID"].ToString());
                user.RoleName = row["RoleType"].ToString();
                user.Email = row["Email"].ToString().Trim();
                user.RoleId = int.Parse(row["RoleId"].ToString());
                user.FirstName = row["FirstName"].ToString();
            }
            return user;
        }
        public void Register(RegisterViewModel registerViewModel)
        {
           const string INSERT_USER_AND_ACCOUNT_REGISTER_QUERY = @"DECLARE @key int
                                                                INSERT INTO [dbo].[User]  ([FirstName] ,[LastName],[Email],[NIC],[MobileNumber], [ManagerID], [DepartmentID]) 
                                                                VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @ManagerID, @DepartmentID)
                                                                SELECT @key = @@IDENTITY
                                                                INSERT INTO [dbo].[Account]([UserID],[Password]) 
                                                                VALUES(@key, @Password)";

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@FirstName", registerViewModel.FirstName));
            parameters.Add(new SqlParameter("@LastName", registerViewModel.LastName));
            parameters.Add(new SqlParameter("@NIC", registerViewModel.NIC));
            parameters.Add(new SqlParameter("@Email", registerViewModel.Email));
            parameters.Add(new SqlParameter("@MobileNumber", registerViewModel.MobileNumber));
            parameters.Add(new SqlParameter("@Password", registerViewModel.Password));
            parameters.Add(new SqlParameter("@DepartmentID", registerViewModel.DepartmentID));
            parameters.Add(new SqlParameter("@ManagerID", registerViewModel.ManagerID));

            _dbCommand.InsertUpdateData(INSERT_USER_AND_ACCOUNT_REGISTER_QUERY, parameters);
        }
        public void AddAccount(AccountModel account)
        {
            const string INSERT_ACCOUNT_QUERY = @"INSERT INTO [dbo].[Account]([UserID], [Password])  VALUES (@UserID, @Password)";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@UserID", account.UserID));
            parameters.Add(new SqlParameter("@Password", account.Password));

            _dbCommand.InsertUpdateData(INSERT_ACCOUNT_QUERY, parameters);
        }

    }
}

//public static UserModel GetEmployeeDetail(LoginModel model)
//{
//    var employee = new UserModel();
//    employee = UserDAL.GetEmployees().FirstOrDefault(emp => emp.EmailAddress == model.EmailAddress);

//    return employee;
//}
