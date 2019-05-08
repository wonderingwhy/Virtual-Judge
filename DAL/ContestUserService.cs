using Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ContestUserService
    {
        public static void Insert(uint cid, uint uid)
        {
            string sql = @"insert into contestuser (ContestID, UserID) values (@ContestID, @UserID)";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ContestID", cid), new MySqlParameter("@UserID", uid));
        }
        public static bool Select(uint cid, uint uid)
        {
            string sql = @"select * from contestuser where ContestID = @ContestID and UserID = @UserID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestID", cid), new MySqlParameter("@UserID", uid));
            return dt.Rows.Count > 0;
        }
    }
}
