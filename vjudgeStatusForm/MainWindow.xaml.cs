using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vjudgeStatusForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void StartJudgeDelegate();
        private delegate void UpdateStatusDelegate(string status);
        public class ParaPair
        {
            public StartJudgeDelegate dlg { set; get; }
            public Solution slt { set; get; }

        }
        public static uint RunID;
        public static HttpHeader header = new HttpHeader();
        public static List<OJ> ojs;
        public static bool IsWork = false;
        static void MakeHttpHeader()
        {
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;
        }
        public bool GetCookie(OJ oj) {
            try
            {
                oj.Statuses = OJService.SelectStatusByOJ(oj.OJID);
                oj.Senders = SenderService.SelectByOJ(oj.OJID);
                oj.CookieContainers = new List<CookieContainer>();
                foreach (Sender sender in oj.Senders)
                {
                    CookieContainer cookieContainer = HttpHelper.GetCooKie(oj.UrlLogin, oj.UrlLoginPart1 + sender.Username + oj.UrlLoginPart2 + sender.Password +oj.UrlLoginPart3, header);
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
                return true;
            }
            catch(Exception e)
            {
                LogService.Insert(2, e);
                return false;
            }
            
        }
        private void UpdateStatus(object obj)
        {
            Dispatcher.Invoke((Action)delegate
            {
                List1.Items.Add(obj);
            });
        }
        public MainWindow()
        {
            InitializeComponent();
            MakeHttpHeader();
            stop.IsEnabled = false;
            try
            {
                SqlHelper.OpenConnection();
                MessageBox.Show("DataBase Connection Success");
            }
            catch (Exception e)
            {
                MessageBox.Show("DataBase Connection seemed to be Failed");
                start.IsEnabled = false;
            }
        }

        public static uint solCount;
        public void StartJudge()
        {
        loop2:            
            if (IsWork == false)
            {
                return;
            }
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
                    if (ojs[(int)solution.OJID - 1].IsWork)
                    {
                        ThreadPool.QueueUserWorkItem(Run, pp);
                    }
                    //Run(solution);
                }
            }
            catch (Exception e)
            {
                LogService.Insert(2, e);
            }
        }
        public void Run(object obj)
        {
            try
            {
                ParaPair pp = (ParaPair)obj;
                Solution solution = pp.slt;

                int index = (int)solution.OJID - 1;
                string code = (index == 2 ?
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(solution.SourceCode)) :
                    solution.SourceCode);
                string pid = (index == 3 ?
                    (Convert.ToUInt32(solution.OJProblemID) - 1000).ToString() :
                    solution.OJProblemID);

                string status = "Pending";
                string time = "", memory = "", compiler = "", runid = "";
                string pattern = ojs[index].PatternStatus;
                Regex regex = new Regex(pattern);
                
                List<int> order = ojs[index].MatchOrder;

                int sid = Choose(index);

                HttpHelper.Submit(ojs[index].UrlSubmit,
                ojs[index].UrlSubmitPart1 + pid +
                ojs[index].UrlSubmitPart2 + solution.OJCompilerID +
                ojs[index].UrlSubmitPart3 + HttpUtility.UrlEncode(code) + ojs[index].UrlSubmitPart4,
                ojs[index].CookieContainers[sid], header);
     loop3:           
                bool ok = false;
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
                solution.IsAccepted = (status == "Accepted");

                if (SolutionService.SelectByOJRunID((int)solution.OJID, solution.OJRunID))
                {
                    UpdateStatus(ojs[index].OJName + " " + solution.OJRunID + " is Existing");
                    goto loop3;
                }
                else
                {
                    SolutionService.Update(solution);
                    UpdateStatus(ojs[index].OJName + " " + solution.OJRunID + " " + status);
                }

                lock (RunID as object)
                {
                    RunID = Math.Max(RunID, solution.SolutionID);
                }

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
        public int Choose(int index)
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
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            start.IsEnabled = true;
            stop.IsEnabled = false;
            IsWork = false;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            start.IsEnabled = false;
            stop.IsEnabled = true;
            IsWork = true;
            try
            {
                ojs = OJService.SelectAll();
                foreach (OJ oj in ojs)
                {
                    if ((oj.IsWork = GetCookie(oj)) == false)
                    {
                        UpdateStatus(oj.OJName + "Connection seemed to be Failed");
                    }
                }
                RunID = RunIDService.Select();
            }
            catch (Exception ee)
            {
                LogService.Insert(2, ee);
                start.IsEnabled = false;
            }
            StartJudge();
        }
    }
}
