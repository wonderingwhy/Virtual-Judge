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
    public static class ContestProblemService
    {
        public static void Insert(List<ContestProblem> ContestProblems)
        {
            foreach (ContestProblem contestproblem in ContestProblems)
            {
                string sql = @"insert into contestproblem (ContestID, ProblemID, Title, Submit, Accepted, OrderID) 
                            values (@ContestID, @ProblemID, @Title, @Submit, @Accepted, @OrderID)";
                SqlHelper.ExecuteNonQuery(sql,
                    new MySqlParameter("@ContestID", contestproblem.ContestID),
                    new MySqlParameter("@ProblemID", contestproblem.ProblemID),
                    new MySqlParameter("@Title", contestproblem.Title),
                    new MySqlParameter("@Submit", contestproblem.Submit),
                    new MySqlParameter("@Accepted", contestproblem.Accepted),
                    new MySqlParameter("@OrderID", (uint)(contestproblem.OrderID - 'A'))
                    );
            }
        }

        public static Problem SelectByContestProblemID(UInt32 ContestProblemID)
        {
            string sql = @"select * from contestproblem 
                            join problem 
                            on contestproblem.ProblemID = problem.ProblemID 
                            where ContestProblemID = @ContestProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestProblemID", ContestProblemID));
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
        public static List<ContestProblem> SelectByContestID(UInt32 ContestID)
        {
            string sql = @"select ContestProblemID, oj.OJID, oj.OJName, contestproblem.ProblemID, OJProblemID, 
                        contestproblem.Title, Problem.Title as PreTitle, contestproblem.Accepted, 
                        contestproblem.Submit, Description, Input, Output, SampleInput, SampleOutput,
                        Hint, TimeLimit, MemoryLimit, OrderID, OJName from contestproblem 
                        join problem on contestproblem.ProblemID = problem.ProblemID 
                        join oj on oj.OJID = problem.OJID
                        where ContestID = @ContestID order by ContestProblemID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@ContestID", ContestID));
            List<ContestProblem> contestproblems = new List<ContestProblem>();
            foreach (DataRow dr in dt.Rows)
            {
                ContestProblem contestproblem = new ContestProblem();
                contestproblem.ContestProblemID = (uint)dr["ContestProblemID"];
                contestproblem.OJID = (uint)dr["OJID"];
                contestproblem.OJName = (string)dr["OJName"];
                contestproblem.ProblemID = (uint)dr["ProblemID"];
                contestproblem.OJProblemID = (string)dr["OJProblemID"];
                contestproblem.Title = (string)dr["Title"];
                contestproblem.PreTitle = (string)dr["PreTitle"];
                contestproblem.Accepted = (uint)dr["Accepted"];
                contestproblem.Submit = (uint)dr["Submit"];
                contestproblem.Description = (string)dr["Description"];
                contestproblem.Input = (string)dr["Input"];
                contestproblem.Output = (string)dr["Output"];
                contestproblem.SampleInput = (string)dr["SampleInput"];
                contestproblem.SampleOutput = (string)dr["SampleOutput"];
                contestproblem.Hint = (string)dr["Hint"];
                contestproblem.TimeLimit = (string)dr["TimeLimit"];
                contestproblem.MemoryLimit = (string)dr["MemoryLimit"];
                contestproblem.OrderID = (char)((uint)dr["OrderID"] + 'A');
                contestproblems.Add(contestproblem);
            }
            return contestproblems;
        }

        public static void UpdateSubmit(UInt32 ContestProblemID)
        {
            string sql = @"Update contestproblem set Submit = Submit + 1 where ContestProblemID = @ContestProblemID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ContestProblemID", ContestProblemID));
        }
        public static void UpdateAccepted(UInt32 ContestProblemID)
        {
            string sql = @"Update contestproblem set Accepted = Accepted + 1 where ContestProblemID = @ContestProblemID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ContestProblemID", ContestProblemID));
        }

        public static void DeleteByContestID(uint ID)
        {
            string sql = @"delete from contestproblem where ContestID = @ContestID";
            SqlHelper.ExecuteNonQuery(sql, new MySqlParameter("@ContestID", ID));
        }
    }
}
