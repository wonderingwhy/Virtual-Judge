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
    public static class SolutionService
    {
        public static void Insert(Solution solution)
        {
            string sql = @"insert into solution (ContestProblemID, UserID, SubmitTime, CompilerID, SourceCode, CodeLength, IsJudged) 
                            values (@ContestProblemID, @UserID, @SubmitTime, @CompilerID, @SourceCode, @CodeLength, @IsJudged)";
            SqlHelper.ExecuteNonQuery(sql,
                    new MySqlParameter("@ContestProblemID", solution.ContestProblemID),
                    new MySqlParameter("@UserID", solution.UserID),
                    new MySqlParameter("@SubmitTime", solution.SubmitTime.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                    new MySqlParameter("@CompilerID", solution.CompilerID),
                    new MySqlParameter("@SourceCode", solution.SourceCode),
                    new MySqlParameter("@CodeLength", solution.SourceCode.Length + "B"),
                    new MySqlParameter("@IsJudged", solution.IsJudged)
                    );
        }
        public static void Update(Solution solution)
        {
            string sql = @"update solution set 
                            RunTime = @RunTime, RunMemory = @RunMemory, Status = @Status, 
                            CodeLength = @CodeLength, OJRunID = @OJRunID, IsJudged = @IsJudged,
                            IsAccepted = @IsAccepted
                            where SolutionID = @SolutionID";
            SqlHelper.ExecuteNonQuery(sql,
                new MySqlParameter("@RunTime", solution.RunTime),
                new MySqlParameter("@RunMemory", solution.RunMemory),
                new MySqlParameter("@Status", solution.Status),
                new MySqlParameter("@CodeLength", solution.CodeLength),
                new MySqlParameter("@OJRunID", solution.OJRunID),
                new MySqlParameter("@IsJudged", solution.IsJudged),
                new MySqlParameter("@IsAccepted", solution.IsAccepted),
                new MySqlParameter("@SolutionID", solution.SolutionID)

                );
        }
        public static Solution SelectBySolutionID(uint ID)
        {
            string sql = @"select * from solution 
                            where SolutionID = @SolutionID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@SolutionID", ID));
            Solution solution = new Solution();
            if (dt.Rows.Count != 1)
            {
                solution.SolutionID = 0;
            }
            else
            {
                solution.SolutionID = (uint)dt.Rows[0]["SolutionID"];
                solution.SourceCode = (string)dt.Rows[0]["SourceCode"];
                solution.ContestProblemID = (uint)dt.Rows[0]["ContestProblemID"];
            }
            return solution;
        }
        public static List<Solution> SelectByIsJudged(uint RunID)
        {
            string sql = @"select * from solution 
                            join contestproblem 
                            on solution.ContestProblemID = contestproblem.ContestProblemID
                            join problem
                            on contestproblem.ProblemID = problem.ProblemID
                            join compiler
                            on solution.CompilerID = compiler.CompilerID
                            where IsJudged = 0 and SolutionID > @SolutionID
                            order by SolutionID Limit 10";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@SolutionID", RunID));
            List<Solution> solutions = new List<Solution>();
            foreach (DataRow dr in dt.Rows)
            {
                Solution solution = new Solution();
                solution.SolutionID = (uint)dr["SolutionID"];
                solution.OJID = (uint)dr["OJID"];
                solution.OJProblemID = (string)dr["OJProblemID"];
                solution.OJCompilerID = (uint)dr["OJCompilerID"];
                solution.SourceCode = (string)dr["SourceCode"];
                solution.CodeLength = (string)dr["CodeLength"];
                solution.ContestProblemID = (uint)dr["ContestProblemID"];
                solutions.Add(solution);
            }
            return solutions;
        }
        public static List<Solution> SelectByUserContestProblem(uint UserID, uint ContestProblemID)
        {
            string sql = @"select * from solution 
                            join contestproblem 
                            on solution.ContestProblemID = contestproblem.ContestProblemID
                            join compiler
                            on solution.CompilerID = compiler.CompilerID
                            join user
                            on solution.UserID = user.UserID
                            where solution.UserID = @UserID and solution.ContestProblemID = @ContestProblemID and IsJudged = 1";
            DataTable dt = SqlHelper.ExecuteDataTable(sql,
                new MySqlParameter("@UserID", UserID),
                new MySqlParameter("@ContestProblemID", ContestProblemID)
                );
            List<Solution> solutions = new List<Solution>();
            foreach (DataRow dr in dt.Rows)
            {
                Solution solution = new Solution();
                solution.SolutionID = (uint)dr["SolutionID"];
                solution.ContestID = (uint)dr["ContestID"];
                solution.ContestProblemID = (uint)dr["ContestProblemID"];
                solution.Title = (string)dr["Title"];
                solution.UserID = (uint)dr["UserID"];
                solution.Nickname = (string)dr["Nickname"];
                solution.Nickname = (string)dr["Nickname"];
                solution.RunTime = (dr["RunTime"] == DBNull.Value ? "..." : (string)dr["RunTime"]);
                solution.RunMemory = (dr["RunMemory"] == DBNull.Value ? "..." : (string)dr["RunMemory"]);
                solution.SubmitTime = (DateTime)dr["SubmitTime"];
                solution.Status = (dr["Status"] == DBNull.Value ? "Pending..." : (string)dr["Status"]);
                solution.CompilerName = (string)dr["Name"];
                solution.CodeLength = (dr["CodeLength"] == DBNull.Value ? "..." : (string)dr["CodeLength"]);
                solution.SourceCode = (string)dr["SourceCode"];
                solution.IsJudged = (bool)dr["IsJudged"];
                solution.IsAccepted = (bool)dr["IsAccepted"];
                solutions.Add(solution);
            }
            return solutions;
        }
        public static List<Solution> SelectByContestIDDesc(uint ContestID)
        {
            string sql = @"select * from solution 
                            join contestproblem 
                            on solution.ContestProblemID = contestproblem.ContestProblemID
                            join compiler
                            on solution.CompilerID = compiler.CompilerID
                            join user
                            on solution.UserID = user.UserID
                            where ContestID = @ContestID
                            order by SolutionID Desc
                            ";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestID", ContestID));
            List<Solution> solutions = new List<Solution>();
            foreach (DataRow dr in dt.Rows)
            {
                Solution solution = new Solution();
                solution.SolutionID = (uint)dr["SolutionID"];
                solution.ContestID = (uint)dr["ContestID"];
                solution.ContestProblemID = (uint)dr["ContestProblemID"];
                solution.Title = (string)dr["Title"];
                solution.UserID = (uint)dr["UserID"];
                solution.Username = (string)dr["Username"];
                solution.Nickname = (string)dr["Nickname"];
                solution.RunTime = (dr["RunTime"] == DBNull.Value ? "..." : (string)dr["RunTime"]);
                solution.RunMemory = (dr["RunMemory"] == DBNull.Value ? "..." : (string)dr["RunMemory"]);
                solution.SubmitTime = (DateTime)dr["SubmitTime"];
                solution.Status = (dr["Status"] == DBNull.Value ? "Pending..." : (string)dr["Status"]);
                solution.CompilerName = (string)dr["Name"];
                solution.CodeLength = (dr["CodeLength"] == DBNull.Value ? "..." : (string)dr["CodeLength"]);
                solution.SourceCode = (string)dr["SourceCode"];
                solution.OrderID = (char)((uint)dr["OrderID"] + 'A');
                solution.IsJudged = (bool)dr["IsJudged"];
                solution.IsAccepted = (bool)dr["IsAccepted"];
                solutions.Add(solution);
            }
            return solutions;
        }
        public static List<Solution> SelectByContestIDAsc(uint ContestID)
        {
            string sql = @"select * from solution 
                            join contestproblem 
                            on solution.ContestProblemID = contestproblem.ContestProblemID
                            join compiler
                            on solution.CompilerID = compiler.CompilerID
                            join user
                            on solution.UserID = user.UserID
                            where ContestID = @ContestID
                            order by SolutionID Asc
                            ";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestID", ContestID));
            List<Solution> solutions = new List<Solution>();
            foreach (DataRow dr in dt.Rows)
            {
                Solution solution = new Solution();
                solution.SolutionID = (uint)dr["SolutionID"];
                solution.ContestID = (uint)dr["ContestID"];
                solution.ContestProblemID = (uint)dr["ContestProblemID"];
                solution.Title = (string)dr["Title"];
                solution.UserID = (uint)dr["UserID"];
                solution.Username = (string)dr["Username"];
                solution.Nickname = (string)dr["Nickname"];
                solution.RunTime = (dr["RunTime"] == DBNull.Value ? "..." : (string)dr["RunTime"]);
                solution.RunMemory = (dr["RunMemory"] == DBNull.Value ? "..." : (string)dr["RunMemory"]);
                solution.SubmitTime = (DateTime)dr["SubmitTime"];
                solution.Status = (dr["Status"] == DBNull.Value ? "Pending..." : (string)dr["Status"]);
                solution.CompilerName = (string)dr["Name"];
                solution.CodeLength = (dr["CodeLength"] == DBNull.Value ? "..." : (string)dr["CodeLength"]);
                solution.SourceCode = (string)dr["SourceCode"];
                solution.OrderID = (char)((uint)dr["OrderID"] + 'A');
                solution.IsJudged = (bool)dr["IsJudged"];
                solution.IsAccepted = (bool)dr["IsAccepted"];
                solutions.Add(solution);
            }
            return solutions;
        }
        public static List<Solution> SelectAllDesc()
        {
            string sql = @"select * from solution 
                            join contestproblem 
                            on solution.ContestProblemID = contestproblem.ContestProblemID
                            join compiler
                            on solution.CompilerID = compiler.CompilerID
                            join user
                            on solution.UserID = user.UserID
                            order by SolutionID Desc 
                            ";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            List<Solution> solutions = new List<Solution>();
            foreach (DataRow dr in dt.Rows)
            {
                Solution solution = new Solution();
                solution.SolutionID = (uint)dr["SolutionID"];
                solution.ContestID = (uint)dr["ContestID"];
                solution.ContestProblemID = (uint)dr["ContestProblemID"];
                solution.Title = (string)dr["Title"];
                solution.Nickname = (string)dr["Nickname"];
                solution.RunTime = (dr["RunTime"] == DBNull.Value ? "..." : (string)dr["RunTime"]);
                solution.RunMemory = (dr["RunMemory"] == DBNull.Value ? "..." : (string)dr["RunMemory"]);
                solution.SubmitTime = (DateTime)dr["SubmitTime"];
                solution.Status = (dr["Status"] == DBNull.Value ? "Pending..." : (string)dr["Status"]);
                solution.CompilerName = (string)dr["Name"];
                solution.CodeLength = (dr["CodeLength"] == DBNull.Value ? "..." : (string)dr["CodeLength"]);
                solution.SourceCode = (string)dr["SourceCode"];
                solution.OrderID = (char)((uint)dr["OrderID"] + 'A');
                /*
                solution.ProblemID = (uint)dr["ProblemID"];
                solution.OJID = (uint)dr["OJID"];
                solution.OJProblemID = (string)dr["OJProblemID"];
                solution.OJRunID = (string)dr["OJRunID"];
                solution.UserID = (uint)dr["UserID"];
                solution.CompilerID = (uint)dr["CompilerID"];
                 * */
                solutions.Add(solution);
            }
            return solutions;
        }
        public static bool SelectByOJRunID(int ojid, string runid)
        {
            string sql = @"select count (*) from solution where OJID = @OJID and OJRunID = @OJRunID";
            object obj = SqlHelper.ExecuteScalar(sql, new MySqlParameter("@OJID", ojid), new MySqlParameter("@OJRunID", runid));
            return Convert.ToInt32(obj) != 0;
        }
    }
}
