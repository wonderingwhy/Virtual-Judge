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
    public static class ProblemService
    {
        public static void Insert(Problem problem)
        {
            string sql = @"insert into problem 
                            (OJID, OJProblemID, Title, Description, Input, Output, 
                            SampleInput, SampleOutput, Hint, Source, TimeLimit, MemoryLimit)
                            values
                            (@OJID, @OJProblemID, @Title, @Description, @Input, @Output, 
                            @SampleInput, @SampleOutput, @Hint, @Source, @TimeLimit, @MemoryLimit)";
            SqlHelper.ExecuteNonQuery(sql,
                new MySqlParameter("@OJID", problem.OJID),
                new MySqlParameter("@OJProblemID", problem.OJProblemID),
                new MySqlParameter("@Title", problem.Title),
                new MySqlParameter("@Description", problem.Description),
                new MySqlParameter("@Input", problem.Input),
                new MySqlParameter("@Output", problem.Output),
                new MySqlParameter("@SampleInput", problem.SampleInput),
                new MySqlParameter("@SampleOutput", problem.SampleOutput),
                new MySqlParameter("@Hint", problem.Hint),
                new MySqlParameter("@Source", problem.Source),
                new MySqlParameter("@TimeLimit", problem.TimeLimit),
                new MySqlParameter("@MemoryLimit", problem.MemoryLimit)
                );
        }
        public static List<Problem> SelectAll()
        {
            string sql = @"select * from problem 
                            join oj
                            on problem.OJID = oj.OJID
                            order by ProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            List<Problem> problems = new List<Problem>();
            foreach (DataRow dr in dt.Rows)
            {
                Problem problem = new Problem();
                problem.ProblemID = (uint)dr["ProblemID"];
                problem.OJName = (string)dr["OJName"];
                problem.OJProblemID = (string)dr["OJProblemID"];
                problem.Title = (string)dr["Title"];
                problem.Submit = (uint)dr["Submit"];
                problem.Accepted = (uint)dr["Accepted"];
                problems.Add(problem);
            }
            return problems;
        }
        public static Problem SelectByOJProblemID(UInt32 OJID, String OJProblemID)
        {
            string sql = @"select * from problem
                            join oj
                            on problem.OJID = oj.OJID
                            where problem.OJID = @OJID and OJProblemID = @OJProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql,
                new MySqlParameter("@OJID", OJID),
                new MySqlParameter("@OJProblemID", OJProblemID)
                );
            Problem problem = new Problem();
            if (dt.Rows.Count != 1)
            {
                problem.ProblemID = 0;
            }
            else
            {
                problem.ProblemID = (uint)dt.Rows[0]["ProblemID"];
                problem.OJName = (string)dt.Rows[0]["OJName"];
                problem.OJProblemID = (string)dt.Rows[0]["OJProblemID"];
                problem.Title = (string)dt.Rows[0]["Title"];
            }
            return problem;
        }
        public static List<Problem> SelectByOJID(UInt32 OJID)
        {
            string sql = @"select * from problem where OJID = @OJID order by OJProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@OJID", OJID));
            List<Problem> problems = new List<Problem>();
            foreach (DataRow dr in dt.Rows)
            {
                Problem problem = new Problem();
                problem.ProblemID = (uint)dr["ProblemID"];
                problem.OJProblemID = (string)dr["OJProblemID"];
                problem.Title = (string)dr["Title"];
                problems.Add(problem);
            }
            return problems;
        }
        public static Problem SelectByProblemID(UInt32 ProblemID)
        {
            string sql = @"select * from problem where ProblemID = @ProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ProblemID", ProblemID));
            Problem problem = new Problem();
            if (dt.Rows.Count != 1)
            {
                problem.ProblemID = 0;
            }
            else
            {
                problem.ProblemID = (uint)dt.Rows[0]["ProblemID"];
                problem.Description = (string)dt.Rows[0]["Description"];
                problem.Title = (string)dt.Rows[0]["Title"];
                problem.Input = (string)dt.Rows[0]["Input"];
                problem.Output = (string)dt.Rows[0]["Output"];
                problem.SampleInput = (string)dt.Rows[0]["SampleInput"];
                problem.SampleOutput = (string)dt.Rows[0]["SampleOutput"];
                problem.Hint = (string)dt.Rows[0]["Hint"];
                problem.Source = (string)dt.Rows[0]["Source"];
                problem.TimeLimit = (string)dt.Rows[0]["TimeLimit"];
                problem.MemoryLimit = (string)dt.Rows[0]["MemoryLimit"];
                problem.Submit = (uint)dt.Rows[0]["Submit"];
                problem.Accepted = (uint)dt.Rows[0]["Accepted"];
            }
            return problem;
        }
        public static void UpdateSubmit(UInt32 ProblemID)
        {
            string sql = @"update problem set Submit = Submit + 1 where ProblemID = @ProblemID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ProblemID", ProblemID));
        }
        public static void UpdateAccepted(UInt32 ProblemID)
        {
            string sql = @"update contestproblem set Accepted = Accepted + 1 where ProblemID = @ProblemID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ProblemID", ProblemID));
        }
    }
}
