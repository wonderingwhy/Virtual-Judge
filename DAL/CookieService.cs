using Helpers;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CookieService
    {
        public static void Insert(CookieItem cookie)
        {
            string sql = @"insert into cookie (GUID, UserID) values (@GUID, @UserID)";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@GUID", cookie.GUID), new MySqlParameter("@UserID", cookie.UserID));
        }
        public static uint SelectByGUID(String GUID)
        {
            string sql = @"select UserID from cookie where GUID = @GUID";
            object obj = SqlHelper.ExecuteScalar(sql, new MySqlParameter("@GUID", GUID));
            return obj == DBNull.Value ? 0 : Convert.ToUInt32(obj);
        }
        public static void DeleteByGUID(String GUID)
        {
            string sql = @"delete from cookie where GUID = @GUID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@GUID", GUID));
        }
        public static void DeleteByUserID(uint UserID)
        {
            string sql = @"delete from cookie where UserID = @UserID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@UserID", UserID));
        }
    }
}
