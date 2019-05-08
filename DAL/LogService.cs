using Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class LogService
    {
        public static void Insert(int ID, Exception e)
        {
            string sql = @"insert into log (ModuleID, Time, Message) values (@ModuleID, @Time, @Message)";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ModuleID",ID), new MySqlParameter("@Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")), new MySqlParameter("@Message", e.ToString()));
        }
    }
}
