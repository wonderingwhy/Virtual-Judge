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
    public static class OJService
    {
        public static List<OJ> SelectAll()
        {
            string sql = "select * from oj";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            List<OJ> ojs = new List<OJ>();
            foreach (DataRow dr in dt.Rows)
            {
                OJ oj = new OJ();
                oj.OJID = (uint)dr["OJID"];
                oj.OJName = (string)dr["OJName"];
                oj.PatternProblemID = (string)dr["PatternProblemID"];
                oj.PatternTitle = dr["PatternTitle"].ToString();
                oj.PatternTimeMemory = dr["PatternTimeMemory"].ToString();
                oj.PatternProblem = dr["PatternProblem"].ToString();
                oj.PatternStatus = dr["PatternStatus"].ToString();
                oj.PatternA = dr["PatternA"].ToString();
                oj.PatternImg = dr["PatternImg"].ToString();
                oj.UrlLogin = dr["UrlLogin"].ToString();
                oj.UrlLoginPart1 = dr["UrlLoginPart1"].ToString();
                oj.UrlLoginPart2 = dr["UrlLoginPart2"].ToString();
                oj.UrlLoginPart3 = dr["UrlLoginPart3"].ToString();
                oj.UrlSubmit = dr["UrlSubmit"].ToString();
                oj.UrlSubmitPart1 = dr["UrlSubmitPart1"].ToString();
                oj.UrlSubmitPart2 = dr["UrlSubmitPart2"].ToString();
                oj.UrlSubmitPart3 = dr["UrlSubmitPart3"].ToString();
                oj.UrlSubmitPart4 = dr["UrlSubmitPart4"].ToString();
                oj.UrlStatus = dr["UrlStatus"].ToString();
                int order = Convert.ToInt32(dr["MatchOrder"]);
                oj.MatchOrder = new List<int>();
                for (int i = 0; i < 6; ++i, order /= 10)
                {
                    oj.MatchOrder.Add(order % 10);
                }
                ojs.Add(oj);
            }
            return ojs;
        }
        public static HashSet<String> SelectStatusByOJ(uint ID)
        {
            string sql = "select * from status where OJID = @OJID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@OJID", ID));
            HashSet<String> set = new HashSet<String>();
            foreach (DataRow dr in dt.Rows)
            {
                set.Add(dr["Status"].ToString());
            }
            return set;
        }
        public static OJ SelectByID(uint ID)
        {
            string sql = "select * from oj where OJID = @OJID";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, new MySqlParameter("@OJID", ID));
            OJ oj = new OJ();
            if (dt.Rows.Count != 1)
            {
                oj.OJID = 0;
            }
            else
            {
                oj.OJID = Convert.ToUInt32(dt.Rows[0]["OJID"]);
                oj.OJName = dt.Rows[0]["OJName"].ToString();
                oj.PatternProblemID = (string)dt.Rows[0]["PatternProblemID"];
                oj.PatternTitle = dt.Rows[0]["PatternTitle"].ToString();
                oj.PatternTimeMemory = dt.Rows[0]["PatternTimeMemory"].ToString();
                oj.PatternProblem = dt.Rows[0]["PatternProblem"].ToString();
                oj.PatternHint = dt.Rows[0]["PatternHint"].ToString();
                oj.PatternStatus = dt.Rows[0]["PatternStatus"].ToString();
                oj.PatternA = dt.Rows[0]["PatternA"].ToString();
                oj.PatternImg = dt.Rows[0]["PatternImg"].ToString();
                oj.Url = dt.Rows[0]["Url"].ToString();
                oj.UrlPid = dt.Rows[0]["UrlPid"].ToString();
                oj.UrlLogin = dt.Rows[0]["UrlLogin"].ToString();
                oj.UrlLoginPart1 = dt.Rows[0]["UrlLoginPart1"].ToString();
                oj.UrlLoginPart2 = dt.Rows[0]["UrlLoginPart2"].ToString();
                oj.UrlLoginPart3 = dt.Rows[0]["UrlLoginPart3"].ToString();
                oj.UrlSubmit = dt.Rows[0]["UrlSubmit"].ToString();
                oj.UrlSubmitPart1 = dt.Rows[0]["UrlSubmitPart1"].ToString();
                oj.UrlSubmitPart2 = dt.Rows[0]["UrlSubmitPart2"].ToString();
                oj.UrlSubmitPart3 = dt.Rows[0]["UrlSubmitPart3"].ToString();
                oj.UrlSubmitPart4 = dt.Rows[0]["UrlSubmitPart4"].ToString();
                oj.UrlStatus = dt.Rows[0]["UrlStatus"].ToString();
            }
            return oj;
        }
    }
}
