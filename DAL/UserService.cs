using Helpers;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public static class UserService
    {
        public static uint SelectIDByUsername(String Username)
        {
            string sql = "select UserID from user where Username = @Username";
            object obj = SqlHelper.ExecuteScalar(sql, new MySqlParameter("@Username", Username));
            return obj == DBNull.Value ? 0 : Convert.ToUInt32(obj);
        }
        public static string SelectUsernameByID(uint ID)
        {
            string sql = "select Username from user where UserID = @UserID";
            object obj = SqlHelper.ExecuteScalar(sql, new MySqlParameter("@UserID", ID));
            return obj == DBNull.Value ? null : obj.ToString();
        }
        public static User SelectByUsername(String Username)
        {
            string sql = "select * from user where Username = @Username";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@Username", Username));
            User user = new User();
            if (dt.Rows.Count != 1)
            {
                user.UserID = 0;
            }
            else
            {
                user.UserID = (uint)dt.Rows[0]["UserID"];
                user.Username = (string)dt.Rows[0]["Username"];
                user.Password = (string)dt.Rows[0]["Password"];
                user.Nickname = (string)dt.Rows[0]["Nickname"];
                user.Type = (string)dt.Rows[0]["Type"];
            }
            return user;
        }
        public static User SelectByUserID(uint ID)
        {
            string sql = "select * from user where UserID = @UserID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@UserID", ID));
            User user = new User();
            if (dt.Rows.Count != 1)
            {
                user.UserID = 0;
            }
            else
            {
                user.UserID = (uint)dt.Rows[0]["UserID"];
                user.Username = (string)dt.Rows[0]["Username"];
                user.Password = (string)dt.Rows[0]["Password"];
                user.Nickname = (string)dt.Rows[0]["Nickname"];
                user.Type = (string)dt.Rows[0]["Type"];
            }
            return user;
        }
        public static uint Insert(User user)
        {
            string sql = "insert into user (Username, Password, Nickname, Type) values(@Username, @Password, @Nickname, @Type);select @@identity";
            object obj = SqlHelper.ExecuteScalar(sql,
                new MySqlParameter("@Username", user.Username),
                new MySqlParameter("@Password", user.Password),
                new MySqlParameter("@Nickname", user.Nickname),
                new MySqlParameter("@Type", user.Type)
                );
            return Convert.ToUInt32(obj);
        }
        public static void Update(User user)
        {
            string sql = "update user set Password = @Password, Nickname = @Nickname where UserID = @UserID";
            SqlHelper.ExecuteNonQuery(sql,
                new MySqlParameter("@UserID", user.UserID),
                new MySqlParameter("@Password", user.Password),
                new MySqlParameter("@Nickname", user.Nickname)
                );
        }
    }
}
