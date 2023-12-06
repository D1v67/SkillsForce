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
        public const string AUTHENTICATE_USER_QUERY = @"SELECT u.* FROM [User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email AND a.Password = @Password";

        public const string GET_USER_DETAILS_WITH_ROLE_QUERY = @"SELECT u.*, r.* FROM [User] u WITH(NOLOCK) INNER JOIN Role r WITH(NOLOCK) ON u.RoleID = r.RoleID 
                                                                 WHERE u.Email = @Email";

        public const string INSERT_ACCOUNT_QUERY = @"INSERT INTO [dbo].[Account]([UserID], [Password])
                                                     VALUES (@UserID, @Password)";

        public string INSERT_USER_AND_ACCOUNT_REGISTER_QUERY = @"DECLARE @key int
                                                                INSERT INTO [dbo].[User]  ([FirstName] ,[LastName],[Email],[NIC],[MobileNumber], [ManagerID], [DepartmentID]) 
                                                                VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @ManagerID, @DepartmentID)
                                                                SELECT @key = @@IDENTITY
                                                                INSERT INTO [dbo].[Account]([UserID],[Password]) 
                                                                VALUES(@key, @Password)";
        private readonly IDBCommand _dbCommand;
        public AccountDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }
        public bool IsUserAuthenticated(AccountModel account)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            //if(String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password)) return false;//throw arg null ex
            parameters.Add(new SqlParameter("@Email", account.Email));
            parameters.Add(new SqlParameter("@Password", account.Password));
            var dt = _dbCommand.GetDataWithConditions(AUTHENTICATE_USER_QUERY, parameters);

            return dt.Rows.Count > 0;
        }

        public AccountModel GetUserDetailsWithRoles(AccountModel account)
        {
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
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@UserID", account.UserID));
            parameters.Add(new SqlParameter("@Password", account.Password));

            _dbCommand.InsertUpdateData(INSERT_ACCOUNT_QUERY, parameters);
        }


        //public static UserModel GetEmployeeDetail(LoginModel model)
        //{
        //    var employee = new UserModel();
        //    employee = UserDAL.GetEmployees().FirstOrDefault(emp => emp.EmailAddress == model.EmailAddress);

        //    return employee;
        //}

    }
}
