using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Common.SkillsForce.Enums;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly IDBCommand _dbCommand;
        public UserDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public IEnumerable<UserModel> GetAll()
        {
            const string GET_ALL_USER_QUERY = @"SELECT * FROM [dbo].[User]";
            List<UserModel> users = new List<UserModel>();

            using (SqlDataReader reader = _dbCommand.GetDataReader(GET_ALL_USER_QUERY))
            {
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                        //RoleID = reader.GetByte(reader.GetOrdinal("RoleID")),
                        DepartmentID = reader.GetByte(reader.GetOrdinal("DepartmentID")),
                       // ManagerID = reader.IsDBNull(reader.GetOrdinal("ManagerID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ManagerID"))
                    };

                    users.Add(user);
                }
            }

            return users;
        }


        public UserModel GetByID(int id)
        {
            const string GET_USER_BY_ID_QUERY = @"SELECT * FROM [dbo].[User] WHERE UserID = @UserID";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_USER_BY_ID_QUERY, parameters))
            {
                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        UserID = reader.GetInt16(reader.GetOrdinal("UserID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                        //RoleID = reader.GetByte(reader.GetOrdinal("RoleID")),
                        DepartmentID =  reader.GetByte(reader.GetOrdinal("DepartmentID")),
                        //ManagerID =reader.GetInt32(reader.GetOrdinal("ManagerID"))
                    };
                    return user;
                }
            }
            return null;
        }
        public void Add(UserModel user)
        {
            const string INSERT_USER_QUERY = @"INSERT INTO [dbo].[User] ([FirstName],[LastName],[Email],[NIC],[MobileNumber],[RoleID],[DepartmentID],[ManagerID]) 
                                                 VALUES (@FirstName, @LastName, @Email, @NIC, @MobileNumber, @RoleID, @DepartmentID, @ManagerID)";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@FirstName", user.FirstName));
            parameters.Add(new SqlParameter("@LastName", user.LastName));
            parameters.Add(new SqlParameter("@Email", user.Email));
            parameters.Add(new SqlParameter("@NIC", user.NIC));
            parameters.Add(new SqlParameter("@MobileNumber", user.MobileNumber));
           // parameters.Add(new SqlParameter("@RoleID", user.RoleID));
            parameters.Add(new SqlParameter("@DepartmentID", user.DepartmentID));
            parameters.Add(new SqlParameter("@ManagerID", user.ManagerID));

            _dbCommand.InsertUpdateData(INSERT_USER_QUERY, parameters);
        }
        public void Delete(int id)
        {
            const string DELETE_USER_QUERY = @"DELETE FROM [dbo].[User] WHERE [UserID] = @UserID";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };
            _dbCommand.InsertUpdateData(DELETE_USER_QUERY, parameters);
        }

        public void Update(UserModel user)
        {
            const string UPDATE_USER_QUERY = @"UPDATE [dbo].[User]
                                                 SET [FirstName] = @FirstName,[LastName] = @LastName,[Email] = @Email,[NIC] = @NIC,[MobileNumber] = @MobileNumber,[RoleID] = @RoleID,[DepartmentID] = @DepartmentID,[ManagerID] = @ManagerID
                                                 WHERE [UserID] = @UserID";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@FirstName", user.MobileNumber));
            parameters.Add(new SqlParameter("@LastName", user.LastName));
            parameters.Add(new SqlParameter("@Email", user.Email));
            parameters.Add(new SqlParameter("@NIC", user.NIC));
            parameters.Add(new SqlParameter("@MobileNumber", user.MobileNumber));
           // parameters.Add(new SqlParameter("@RoleID", user.RoleID));
            parameters.Add(new SqlParameter("@DepartmentID", user.DepartmentID));
            parameters.Add(new SqlParameter("@ManagerID", user.ManagerID));

            _dbCommand.InsertUpdateData(UPDATE_USER_QUERY, parameters);
        }

        public IEnumerable<UserModel> GetAllManager()
        {
            const string GET_ALL_MANAGERS = @"SELECT U.* 
                                                FROM [User] U 
                                                JOIN UserRole UR ON U.UserID = UR.UserID
                                                JOIN Role R ON UR.RoleID = R.RoleID 
                                                WHERE R.RoleName = @RoleName";
            List<UserModel> managers = new List<UserModel>();

            var parameters = new List<SqlParameter> { new SqlParameter("@RoleName", RolesEnum.Manager.ToString()) };

            using (SqlDataReader reader = _dbCommand.GetDataWithConditionsReader(GET_ALL_MANAGERS, parameters))
            {
                while (reader.Read())
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

            if (managers.Count > 0)
            {
                return managers;
            }

            return null;
        }

        public bool ApproveRequest(UserModel user, TrainingModel traning)
        {
            throw new NotImplementedException();
        }

        public bool DeclineRequest(UserModel user, TrainingModel traning)
        {
            throw new NotImplementedException();
        }

        public bool Login(UserModel user)
        {
            throw new NotImplementedException();
        }

        public bool Logout(UserModel user)
        {
            throw new NotImplementedException();
        }

        public bool Register(UserModel user)
        {
            throw new NotImplementedException();
        }


    }
}

//public UserModel GetByID(int id)
//{
//    const string GET_USER_BY_ID_QUERY = @"SELECT u.* FROM [User] u WHERE u.UserID = @UserID";
//    var parameters = new List<SqlParameter> { new SqlParameter("@UserID", id) };
//    var dt = _dbCommand.GetDataWithConditions(GET_USER_BY_ID_QUERY, parameters);

//    if (dt.Rows.Count > 0)
//    {
//        DataRow row = dt.Rows[0];
//        UserModel user = new UserModel
//        {
//            UserID = int.Parse(row["UserID"].ToString()),
//            FirstName = row["FirstName"].ToString(),
//            LastName = row["LastName"].ToString(),
//            Email = row["Email"].ToString(),
//            NIC = row["NIC"].ToString(),
//            MobileNumber = row["MobileNumber"].ToString(),
//            RoleID = int.Parse(row["RoleID"].ToString()),
//            DepartmentID = int.Parse(row["DepartmentID"].ToString()),
//            ManagerID = int.Parse(row["ManagerID"].ToString())
//        };
//        return user;
//    }
//    return null;
//}


//const string GET_USER_BY_NIC = @"SELECT * FROM Users WHERE NIC = @NIC";
//const string GET_USER_BY_EMAIL = @"SELECT * FROM Users WHERE Email = @Email;";
//const string GET_USER_BY_MOBILE_NUMBER = @"SELECT * FROM Users WHERE MobileNumber = @MobileNumber";

//const string SET_USER_PASSWORD = "";

//public IEnumerable<UserModel> GetAll()
//{
//    const string GET_ALL_USER_QUERY = @"SELECT * FROM [dbo].[User]";
//    List<UserModel> users = new List<UserModel>();

//    UserModel user;
//    var dt = _dbCommand.GetData(GET_ALL_USER_QUERY);
//    foreach (DataRow row in dt.Rows)
//    {
//        user = new UserModel();
//        user.UserID = int.Parse(row["UserID"].ToString());
//        user.FirstName = row["FirstName"].ToString();
//        user.LastName = row["LastName"].ToString();
//        user.Email = row["Email"].ToString();
//        user.NIC = row["NIC"].ToString();
//        user.MobileNumber = row["MobileNumber"].ToString();
//        user.RoleID = int.Parse(row["RoleID"].ToString());
//        user.DepartmentID = int.Parse(row["DepartmentID"].ToString());
//        // NULL 
//        //user.ManagerID = row["ManagerID"] != DBNull.Value ? (int?)row["ManagerID"] : null;
//        users.Add(user);
//    }
//    return users;
//}


//public IEnumerable<UserModel> GetAllManager()
//{
//    const string GET_ALL_MANAGERS = @"SELECT U.* FROM [User] U JOIN Role R ON U.RoleID = R.RoleID WHERE R.RoleType = @RoleName;";
//    List<UserModel> managers = new List<UserModel>();

//    var parameters = new List<SqlParameter> { new SqlParameter("@RoleName", RolesEnum.Manager.ToString()) };
//    var dt = _dbCommand.GetDataWithConditions(GET_ALL_MANAGERS, parameters);
//    UserModel manager;
//    if (dt.Rows.Count > 0)
//    {
//        foreach (DataRow row in dt.Rows)
//        {
//            manager = new UserModel();
//            manager.UserID = int.Parse(row["UserID"].ToString());
//            manager.FirstName = row["FirstName"].ToString();
//            manager.LastName = row["LastName"].ToString();

//            managers.Add(manager);
//        }
//        return managers;
//    }
//    return null;
//}
