using Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public static class RunIDService
    {
        public static uint Select()
        {
            string sql = "select * from runid";
            object obj = SqlHelper.ExecuteScalar(sql);
            return Convert.ToUInt32(obj);
        }
        public static void Update(uint RunID)
        {
            string sql = "update runid set RunID = @RunID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@RunID", RunID));
        }
    }
}
