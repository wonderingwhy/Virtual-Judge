using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace vjudge
{
    public delegate void StartJudgeDelegate();
    public class ParaPair
    {
        public StartJudgeDelegate dlg { set; get; }
        public Solution slt { set; get; }

    }

    public class Program
    {
        public static uint RunID;
        public static HttpHeader header;
        public static List<OJ> ojs;
        public static string reXSS(string str)
        {
            str = str.Replace("&lt;", "<");
            return str.Replace("&gt;", ">");
        }
        static void Main(string[] args)
        {
            header = new HttpHeader();
            MakeHttpHeader();

            try
            {
                SqlHelper.OpenConnection();
                Console.WriteLine("DataBase Connection Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("DataBase Connection seemed to be Failed");
            }
            try
            {
                ojs = OJService.SelectAll();
                foreach (OJ oj in ojs)
                {
                    oj.Statuses = OJService.SelectStatusByOJ(oj.OJID);
                    oj.Senders = SenderService.SelectByOJ(oj.OJID);
                    oj.CookieContainers = new List<CookieContainer>();
                    foreach (Sender sender in oj.Senders)
                    {
                        CookieContainer cookieContainer = HttpHelper.GetCooKie(oj.UrlLogin, oj.UrlLoginPart1 + sender.Username + oj.UrlLoginPart2 + sender.Password, header);
                        if (cookieContainer != null)
                        {
                            oj.CookieContainers.Add(cookieContainer);
                        }
                    }
                    oj.QSenders = new Queue<int>();
                    for (int i = 0; i < oj.Senders.Count; ++i)
                    {
                        oj.QSenders.Enqueue(i);
                    }
                    Console.WriteLine(oj.OJName + " Succeed to Get Cookie");
                }

                RunID = RunIDService.Select();

                Console.WriteLine("Judge Start");
                StartJudge();
            }
            catch (Exception e)
            {
                LogService.Insert(2, e);
                Console.WriteLine("Fail to Get Cookie");
            }
            Console.ReadKey();
        }
        public static uint solCount;
        public static void StartJudge()
        {
        loop2:
            try
            {
                RunIDService.Update(RunID);
                List<Solution> solutions = SolutionService.SelectByIsJudged(RunID);
                solCount = (uint)solutions.Count();
                if (solCount == 0)
                {
                    goto loop2;
                }
                foreach (Solution solution in solutions)
                {
                    ParaPair pp = new ParaPair();
                    pp.dlg = StartJudge;
                    pp.slt = solution;
                    ThreadPool.QueueUserWorkItem(Run, pp);
                }
            }
            catch (Exception e)
            {
                LogService.Insert(2, e);
            }
        }
        public static int Choose(int index)
        {
        loop:
            while (ojs[index].QSenders.Count() <= 0) ;
            lock (ojs[index].QSenders)
            {
                try
                {
                    if (ojs[index].QSenders.Count() > 0)
                    {
                        int sid = ojs[index].QSenders.First();
                        ojs[index].QSenders.Dequeue();
                        return sid;
                    }
                }
                catch (Exception e)
                {
                    LogService.Insert(2, e);
                }
                goto loop;
            }
        }
        public static void Run(object obj)
        {
            try
            {
                ParaPair pp = (ParaPair)obj;
                Solution solution = pp.slt;

                int index = (int)solution.OJID - 1;
                string code = (index == 2 ?
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(reXSS(solution.SourceCode))) :
                    reXSS(solution.SourceCode));
                string pid = (index == 3 ?
                    (Convert.ToUInt32(solution.OJProblemID) - 1000).ToString() :
                    solution.OJProblemID);

                string status = "Pending";
                string time = "", memory = "", compiler = "", runid = "";
                string pattern = ojs[index].PatternStatus;
                Regex regex = new Regex(pattern);
                bool ok = false;
                List<int> order = ojs[index].MatchOrder;

                int sid = Choose(index);

                if (index == 1)
                {
                    CookieContainer cookieContainer = HttpHelper.GetCooKie(ojs[index].UrlLogin, ojs[index].UrlLoginPart1 + ojs[index].Senders[0].Username + ojs[index].UrlLoginPart2 + ojs[index].Senders[0].Password, header);
                    HttpHelper.Submit(ojs[index].UrlSubmit,
                                    ojs[index].UrlSubmitPart1 + pid +
                                    ojs[index].UrlSubmitPart2 + solution.OJCompilerID +
                                    ojs[index].UrlSubmitPart3 + HttpUtility.UrlEncode(code) + ojs[index].UrlSubmitPart4,
                                    cookieContainer, header);
                }
                else
                {
                    HttpHelper.Submit(ojs[index].UrlSubmit,
                                    ojs[index].UrlSubmitPart1 + pid +
                                    ojs[index].UrlSubmitPart2 + solution.OJCompilerID +
                                    ojs[index].UrlSubmitPart3 + HttpUtility.UrlEncode(code) + ojs[index].UrlSubmitPart4,
                                    ojs[index].CookieContainers[sid], header);
                }

                while (ok == false)
                {
                    string html = HttpHelper.GetHtml(
                        ojs[index].UrlStatus + ojs[index].Senders[sid].Username,
                        new CookieContainer(), header);
                    Match m = regex.Match(html);
                    status = m.Groups[order[0]].ToString();

                    if (ojs[index].Statuses.Contains(status))
                    {
                        runid = m.Groups[order[1]].ToString();
                        time = m.Groups[order[2]].ToString();
                        memory = m.Groups[order[3]].ToString();
                        compiler = m.Groups[order[5]].ToString();
                        ok = true;
                    }
                }

                solution.Status = status;
                solution.CompilerName = compiler;
                solution.RunTime = (index == 3 ? time + "MS" : time);
                solution.RunTime = solution.RunTime.Replace(" ", "");
                solution.RunMemory = (index == 3 ? memory + "K" : memory);
                solution.RunMemory = solution.RunMemory.Replace(" ", "");
                solution.OJRunID = runid;
                solution.IsJudged = true;
                solution.IsJudged = true;
                solution.IsAccepted = false;
                if (status == "Accepted")
                {
                    solution.IsAccepted = true;
                    ContestProblemService.UpdateAccepted(solution.ContestProblemID);
                }
                SolutionService.Update(solution);

                Console.WriteLine(ojs[index].OJName + " " + solution.OJRunID + " " + status);
                /*
                lock (RunID as object)
                {
                    RunID = Math.Max(RunID, solution.SolutionID);
                }
                */
                lock (solCount as object)
                {
                    solCount--;
                    if (solCount == 0)
                    {
                        pp.dlg();
                    }
                }
                ojs[index].QSenders.Enqueue(sid);
            }
            catch (Exception e)
            {
                LogService.Insert(2, e);
            }
        }
        
        static void MakeHttpHeader()
        {
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;
        }
    }
}
