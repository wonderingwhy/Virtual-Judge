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
    public static class ContestService
    {
        public static uint Insert(Contest contest)
        {
            string sql = @"insert into contest (Title, StartTime, EndTime, Declaration, Type, UserID, Password, ProsNum)
                            values (@Title, @StartTime, @EndTime, @Declaration, @Type, @UserID, @Password, @ProsNum);select @@identity";
            object obj = SqlHelper.ExecuteScalar(sql,
                new MySqlParameter("@Title", contest.Title),
                new MySqlParameter("@Declaration", contest.Declaration),
                new MySqlParameter("@StartTime", contest.StartTime),
                new MySqlParameter("@EndTime", contest.EndTime),
                new MySqlParameter("@Type", contest.Type),
                new MySqlParameter("@UserID", contest.UserID),
                new MySqlParameter("@Password", contest.Password),
                new MySqlParameter("@ProsNum", contest.ProsNum)
                );
            return Convert.ToUInt32(obj);
        }
        public static List<Contest> SelectAll()
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            order by ContestID Desc";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                contests.Add(contest);
            }
            return contests;
        }
        public static List<Contest> SelectPart(uint p, uint npp)
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            order by ContestID Desc Limit @Base, @Number";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@Base", (p - 1) * npp), new MySqlParameter("@Number", npp));
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                contests.Add(contest);
            }
            return contests;
        }
        public static List<Contest> SelectPartByUID(uint p, uint npp, uint uid)
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            where contest.UserID = @UserID
                            order by ContestID Desc Limit @Base, @Number";
            DataTable dt = SqlHelper.ExecuteDataTable(sql,
                new MySqlParameter("@UserID", uid),
                new MySqlParameter("@Base", (p - 1) * npp),
                new MySqlParameter("@Number", npp));
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                contests.Add(contest);
            }
            return contests;
        }
        public static List<Contest> SelectByUID(uint UID)
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            where contest.UserID = @UserID
                            order by ContestID Desc";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@UserID", UID));
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                contests.Add(contest);
            }
            return contests;
        }
        public static uint CountAll()
        {
            string sql = @"select count(*) from contest";
            object obj = SqlHelper.ExecuteScalar(sql);
            return Convert.ToUInt32(obj);
        }
        public static uint CountByUID(uint uid)
        {
            string sql = @"select count(*) from contest where UserID = @UserID";
            object obj = SqlHelper.ExecuteScalar(sql, new MySqlParameter("@UserID", uid));
            return Convert.ToUInt32(obj);
        }
        public static Contest SelectByID(uint ID)
        {
            string sql = @"select * from contest 
                            join user
                            on contest.UserID = user.UserID
                            where ContestID = @ContestID ";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestID", ID));
            Contest contest = new Contest();
            if (dt.Rows.Count != 1)
            {
                contest.ContestID = 0;
                return contest;
            }
            contest.ContestID = ID;
            contest.UserID = Convert.ToUInt32(dt.Rows[0]["UserID"]);
            contest.Title = (string)dt.Rows[0]["Title"];
            contest.StartTime = (DateTime)dt.Rows[0]["StartTime"];
            contest.EndTime = (DateTime)dt.Rows[0]["EndTime"];
            contest.Password = (string)dt.Rows[0]["Password"];
            contest.Declaration = (string)dt.Rows[0]["Declaration"];
            contest.Type = (string)dt.Rows[0]["Type"];
            contest.Nickname = (string)dt.Rows[0]["Nickname"];
            contest.ProsNum = (uint)dt.Rows[0]["ProsNum"];
            DateTime datetime = DateTime.Now;
            if (datetime < contest.StartTime)
            {
                contest.Status = "Pending";
            }
            else if (datetime < contest.EndTime)
            {
                contest.Status = "Running";
            }
            else
            {
                contest.Status = "Ended";
            }
            return contest;
        }
        public static void Update(Contest contest)
        {
            string sql = @"update contest set Title = @Title, StartTime = @StartTime, EndTime = @EndTime, 
                            Declaration = @Declaration, Type = @Type, Password = @Password, 
                            ProsNum = @ProsNum where ContestID = @ContestID";
            SqlHelper.ExecuteNonQuery(sql,
                new MySqlParameter("@Title", contest.Title),
                new MySqlParameter("@Declaration", contest.Declaration),
                new MySqlParameter("@StartTime", contest.StartTime),
                new MySqlParameter("@EndTime", contest.EndTime),
                new MySqlParameter("@Type", contest.Type),
                new MySqlParameter("@Password", contest.Password),
                new MySqlParameter("@ProsNum", contest.ProsNum),
                new MySqlParameter("@ContestID", contest.ContestID)
                );
        }
        public static List<Contest> SelectByPara(uint cid, string title, string uname, string status, string type)
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            where ContestID = @ContestID and Title like @Title 
                            and Username like @Username and Type like @Type
                            order by ContestID Desc";
            DataTable dt = SqlHelper.ExecuteDataTable(sql,
                new MySqlParameter("@ContestID", cid),
                new MySqlParameter("@Title", "%" + title + "%"),
                new MySqlParameter("@Username", "%" + uname + "%"),
                new MySqlParameter("@Type", "%" + type + "%")
                );
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                if (contest.Status == status)
                {
                    contests.Add(contest);
                }
            }
            return contests;
        }
        public static List<Contest> SelectByPara(string title, string uname, string status, string type)
        {
            string sql = @"select * from contest
                            join user
                            on contest.UserID = user.UserID
                            where Title like @Title 
                            and Username like @Username and contest.Type like @Type
                            order by ContestID Desc";
            DataTable dt = SqlHelper.ExecuteDataTable(sql,
                new MySqlParameter("@Title", "%" + title + "%"),
                new MySqlParameter("@Username", "%" + uname + "%"),
                new MySqlParameter("@Type", "%" + type + "%")
                );
            List<Contest> contests = new List<Contest>();
            foreach (DataRow dr in dt.Rows)
            {
                Contest contest = new Contest();
                contest.ContestID = (uint)dr["ContestID"];
                contest.Title = (string)dr["Title"];
                contest.Type = (string)dr["Type"];
                contest.Username = (string)dr["Username"];
                contest.Nickname = (string)dr["Nickname"];
                contest.Password = (string)dr["Password"];
                contest.StartTime = (DateTime)dr["StartTime"];
                contest.EndTime = (DateTime)dr["EndTime"];
                DateTime datetime = DateTime.Now;
                if (datetime < contest.StartTime)
                {
                    contest.Status = "Pending";
                }
                else if (datetime < contest.EndTime)
                {
                    contest.Status = "Running";
                }
                else
                {
                    contest.Status = "Ended";
                }
                if (status == "" || contest.Status == status)
                {
                    contests.Add(contest);
                }
            }
            return contests;
        }
        public static List<Contest> SelectPartByPara(uint cid, string title, string uname, string status, string type, uint p, uint npp)
        {
            List<Contest> contests = SelectByPara(cid, title, uname, status, type);
            uint bas = (p - 1) * npp;
            return contests.GetRange((int)bas, (int)Math.Min(npp, contests.Count - bas));
        }
        public static List<Contest> SelectPartByPara(string title, string uname, string status, string type, uint p, uint npp)
        {
            List<Contest> contests = SelectByPara(title, uname, status, type);
            uint bas = (p - 1) * npp;
            return contests.GetRange((int)bas, (int)Math.Min(npp, contests.Count - bas));
        }
        public static uint CountByPara(uint cid, string title, string uname, string status, string type)
        {
            return (uint)SelectByPara(cid, title, uname, status, type).Count;
        }
        public static uint CountByPara(string title, string uname, string status, string type)
        {
            return (uint)SelectByPara(title, uname, status, type).Count;
        }
    }
}
