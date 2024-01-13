using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SkillsForce.DAL
{
    public class UserActivityDAL : IUserActivityDAL
    {
        private readonly IDBCommand _dbCommand;
        public UserActivityDAL(IDBCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        public async Task<bool> AddUserActivity(UserActivityModel userActivity)
        {
            const string INSERT_USER_ACTIVITY_QUERY = @"
            INSERT INTO [dbo].[UserActivity] (
                [UserID], [CurrentRole], [UrlVisited], [HttpMethod], [ActionParameters], 
                [IpAddress], [UrlVisitedTimestamp], [UserAgent], [SessionID], [Referer], 
                [StatusCode], [IsMobileDevice]
            )
            VALUES (
                @UserID, @CurrentRole, @UrlVisited, @HttpMethod, @ActionParameters, 
                @IpAddress, @UrlVisitedTimestamp, @UserAgent, @SessionID, @Referer, 
                @StatusCode, @IsMobileDevice
            )";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
            new SqlParameter("@UserID", userActivity.UserID),
            new SqlParameter("@CurrentRole", userActivity.CurrentRole ?? (object)DBNull.Value),
            new SqlParameter("@UrlVisited", userActivity.UrlVisited),
            new SqlParameter("@HttpMethod", userActivity.HttpMethod),
            new SqlParameter("@ActionParameters", userActivity.ActionParameters ?? (object)DBNull.Value),
            new SqlParameter("@IpAddress", userActivity.IpAddress),
            new SqlParameter("@UrlVisitedTimestamp", userActivity.UrlVisitedTimestamp),
            new SqlParameter("@UserAgent", userActivity.UserAgent),
            new SqlParameter("@SessionID", userActivity.SessionID ?? (object)DBNull.Value),
            new SqlParameter("@Referer", userActivity.Referer ?? (object)DBNull.Value),
            new SqlParameter("@StatusCode", userActivity.StatusCode ?? (object)DBNull.Value),
            new SqlParameter("@UserLocation", userActivity.UserLocation ?? (object)DBNull.Value),
            new SqlParameter("@IsMobileDevice", userActivity.IsMobileDevice ?? (object)DBNull.Value),
            };

            return await _dbCommand.InsertUpdateDataAsync(INSERT_USER_ACTIVITY_QUERY, parameters)>0;
        }

        public async Task<bool> AddUserLoginActivity(UserActivityModel userActivity)
        {
            const string INSERT_LOGIN_HISTORY_QUERY = @"
            INSERT INTO [dbo].[LoginHistory] (
                [UserID], [LoginTime], [IPAddress], [UserAgent], [IsMobileDevice]
            )
            VALUES (
                @UserID, @LoginTime, @IPAddress, @UserAgent, @IsMobileDevice
            )";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userActivity.UserID),
                new SqlParameter("@LoginTime", userActivity.LoginTimestamp),
                new SqlParameter("@IpAddress", userActivity.IpAddress),
                new SqlParameter("@UserAgent", userActivity.UserAgent),
                new SqlParameter("@IsMobileDevice", userActivity.IsMobileDevice ?? (object)DBNull.Value),
            };

            return await _dbCommand.InsertUpdateDataAsync(INSERT_LOGIN_HISTORY_QUERY, parameters) > 0;
        }
    }
}
