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
    public static class SenderService
    {
        public static List<Sender> SelectByOJ(UInt32 OJID)
        {
            List<Sender> senders = new List<Sender>();
            string sql = @"select * from sender where OJID = @OJID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@OJID", OJID));
            foreach (DataRow dr in dt.Rows)
            {
                Sender sender = new Sender();
                sender.SenderID = (uint)dr["SenderID"];
                sender.OJID = (uint)dr["OJID"];
                sender.Username = (string)dr["Username"];
                sender.Password = (string)dr["Password"];
                senders.Add(sender);
            }
            return senders;
        }
    }
}
