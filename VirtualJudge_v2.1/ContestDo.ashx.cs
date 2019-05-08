using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace VirtualJudge_v2._1
{
    /// <summary>
    /// ContestDo 的摘要说明
    /// </summary>
    public class ContestDo : IHttpHandler
    {
        public bool IsValidChar(Char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            if (ch >= 'a' && ch <= 'z')
            {
                return true;
            }
            if (ch >= 'A' && ch <= 'Z')
            {
                return true;
            }
            return ch == '_';
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/html";
                string action = context.Request["action"];
                HttpCookie cookie = context.Request.Cookies["GUID"];
                if (cookie == null)
                {
                    context.Response.Write("<script>alert('Log in First');window.location.href='index.ashx'</script>");
                    return;
                }
                uint uid = CookieService.SelectByGUID(cookie.Value);
                if (uid == 0)
                {
                    context.Response.Write("<script>alert('Log in First');window.location.href='index.ashx'</script>");
                    return;
                }
                #region
                /*
                    * ajax
                    * */
                if (action == "check")
                {
                    context.Response.ContentType = "text/plain";
                    uint cid = (uint)Convert.ToUInt32(context.Request["cid"]);
                    if (ContestUserService.Select(cid, uid))
                    {
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("no");
                    }
                }
                #endregion

                #region
                /*
                    * ajax
                    * */
                else if (action == "checkcpsd")
                {
                    context.Response.ContentType = "text/plain";
                    uint cid = (uint)Convert.ToUInt32(context.Request["cid"]);
                    Contest contest = ContestService.SelectByID(cid);
                    if (contest.Type == "private" && ContestUserService.Select(cid, uid) == false)
                    {
                        string pp = (string)context.Request["psd"];
                        if (pp == contest.Password)
                        {
                            ContestUserService.Insert(cid, uid);
                            context.Response.Write("ok");
                        }
                        else
                        {
                            context.Response.Write("no");
                        }
                    }
                    else
                    {
                        context.Response.Write("ok");
                    }
                    return;
                }
                #endregion

                #region
                else if (action == "view")
                {

                    uint cid = (uint)Convert.ToUInt32(context.Request["cid"]);
                    Contest contest = ContestService.SelectByID(cid);
                    if (contest.Type == "private" && ContestUserService.Select(cid, uid) == false)
                    {
                        context.Response.Write("<script>alert('No Permission')</script>");
                        return;
                    }
                    List<ContestProblem> contestproblems;
                    if (contest.Status == "Pending")
                    {
                        contestproblems = new List<ContestProblem>();
                    }
                    else
                    {
                        contestproblems = ContestProblemService.SelectByContestID(cid);
                    }
                    List<Solution> solsAsc = SolutionService.SelectByContestIDAsc(cid);
                    List<Solution> solsDesc = new List<Solution>(solsAsc);
                    solsDesc.Reverse(0, solsAsc.Count);
                    /*
                     * OverView
                     * */
                    foreach (ContestProblem contestproblem in contestproblems)
                    {
                        List<Solution> msols = SolutionService.SelectByUserContestProblem(uid, contestproblem.ContestProblemID);
                        if (msols.Count == 0)
                        {
                            contestproblem.Status = "";
                            continue;
                        }
                        contestproblem.Status = "No";
                        foreach (Solution solution in msols)
                        {
                            if (solution.IsAccepted)
                            {
                                contestproblem.Status = "Yes";
                                break;
                            }
                        }
                    }
                    /*
                     * RankList
                     * */
                    List<User> us = new List<User>();
                    foreach (Solution sol in solsAsc)
                    {
                        sol.IsVisible = (sol.UserID == uid);
                        bool ok = false;
                        foreach (User u in us)
                        {
                            if (sol.UserID == u.UserID)
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (ok == false)
                        {
                            User user = new User();
                            user.UserID = sol.UserID;
                            user.Username = sol.Username;
                            user.Nickname = sol.Nickname;
                            for (int i = 0; i < contestproblems.Count; ++i)
                            {
                                user.Items.Add(new Item());
                            }
                            us.Add(user);
                        }
                    }
                    foreach (Solution sol in solsAsc)
                    {
                        foreach (User u in us)
                        {
                            if (sol.UserID == u.UserID)
                            {
                                uint id = (uint)sol.OrderID - 'A';
                                if (u.Items[(int)id].IsSolved == false)
                                {
                                    if (sol.IsAccepted)
                                    {
                                        u.Items[(int)id].IsSolved = true;
                                        u.Items[(int)id].Time = sol.SubmitTime - contest.StartTime;
                                    }
                                    else if (sol.IsJudged)
                                    {
                                        u.Items[(int)id].Penalty++;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    foreach (User u in us)
                    {
                        foreach (Item item in u.Items)
                        {
                            if (item.IsSolved == true)
                            {
                                u.ProsSolved++;
                                u.Timer += item.Time.Add(new TimeSpan(0, (int)(20 * item.Penalty), 0));
                            }
                        }
                    }

                    for (int i = 0; i < contestproblems.Count; ++i)
                    {
                        TimeSpan FB = new TimeSpan(1000, 0, 0);
                        foreach (User u in us)
                        {
                            if (u.Items[i].Time < FB && u.Items[i].Time != TimeSpan.Zero)
                            {
                                FB = u.Items[i].Time;
                            }
                        }
                        foreach (User u in us)
                        {
                            if (u.Items[i].Time == FB)
                            {
                                u.Items[i].IsFirst = true;
                            }
                        }
                    }
                    us.Sort();
                    for (int i = 0; i < us.Count; ++i)
                    {
                        us[i].Rank = (uint)i + 1;
                    }
                    var Data = new { problems = contestproblems, contest = contest, solutions = solsDesc, users = us, ctime = (DateTime.UtcNow - DateTime.Parse("1970-1-1")).TotalMilliseconds };
                    string html = CommonHelper.RenderHtml("contestView.html", Data);
                    context.Response.Write(html);
                }
                #endregion

                #region
                else if (action == "add")
                {
                    List<OJ> OJs = OJService.SelectAll();
                    Contest contest = new Contest();
                    var Data = new
                    {
                        OJs = OJs,
                        edit = false,
                        Title = "",
                        Time = "",
                        Length = new TimeSpan(5, 0, 0),
                        Dec = "",
                        Psd = ""
                    };
                    string html = CommonHelper.RenderHtml("contestAdd.html", Data);
                    context.Response.Write(html);
                }
                #endregion

                #region
                else if (action == "csearch")
                {

                    string mtitle = context.Request["mtitle"].ToString();
                    string mcreator = context.Request["mcreator"].ToString();
                    string mstatus = context.Request["mstatus"].ToString();
                    string mtype = context.Request["mtype"].ToString();
                    if (mtitle == "" && mcreator == "" && mstatus == "" && mtype == "")
                    {
                        context.Response.Redirect("ContestDo.ashx?action=list", false);
                    }
                    uint Page;
                    uint IsLast = 0;
                    uint npp = 20;
                    if (context.Request["Page"] == null)
                    {
                        Page = 1;
                    }
                    else
                    {
                        Page = Convert.ToUInt32(context.Request["Page"]);
                    }
                    uint num = ContestService.CountByPara(mtitle, mcreator, mstatus, mtype);
                    uint last = Math.Max(1, (num % npp == 0 ? num / npp : num / npp + 1));
                    if (Page == 0 || Page >= last)
                    {
                        Page = last;
                    }
                    if (Page == last)
                    {
                        IsLast = 1;
                    }
                    List<Contest> contests = ContestService.SelectPartByPara(mtitle, mcreator, mstatus, mtype, Page, npp);
                    var Data = new
                    {
                        contests = contests,
                        mlist = false,
                        Page = Page,
                        IsLast = IsLast,
                        mTitle = mtitle,
                        mCreator = mcreator,
                        mStatus = mstatus,
                        mType = mtype,
                        csearch = true
                    };
                    string html = CommonHelper.RenderHtml("contestList.html", Data);
                    context.Response.Write(html);
                }
                #endregion

                #region
                else if (action == "list")
                {
                    uint Page;
                    uint IsLast = 0;
                    uint npp = 20;
                    if (context.Request["Page"] == null)
                    {
                        Page = 1;
                    }
                    else
                    {
                        Page = Convert.ToUInt32(context.Request["Page"]);
                    }
                    uint num = ContestService.CountAll();
                    uint last = Math.Max(1, (num % npp == 0 ? num / npp : num / npp + 1));
                    if (Page == 0 || Page >= last)
                    {
                        Page = last;
                    }
                    if (Page == last)
                    {
                        IsLast = 1;
                    }
                    List<Contest> contests = ContestService.SelectPart(Page, npp);
                    var Data = new
                    {
                        contests = contests,
                        mlist = false,
                        Page = Page,
                        IsLast = IsLast,
                        mTitle = "",
                        mCreator = "",
                        mStatus = "",
                        mType = "",
                        csearch = false
                        
                    };
                    string html = CommonHelper.RenderHtml("contestList.html", Data);
                    context.Response.Write(html);
                }
                #endregion

                #region
                else if (action == "mlist")
                {
                    uint Page;
                    uint IsLast = 0;
                    uint npp = 20;
                    if (context.Request["Page"] == null)
                    {
                        Page = 1;
                    }
                    else
                    {
                        Page = Convert.ToUInt32(context.Request["Page"]);
                    }
                    uint num = ContestService.CountByUID(uid);
                    uint last = Math.Max(1, (num % npp == 0 ? num / npp : num / npp + 1));
                    if (Page == 0 || Page >= last)
                    {
                        Page = last;
                    }
                    if (Page == last)
                    {
                        IsLast = 1;
                    }

                    List<Contest> contests = ContestService.SelectPartByUID(Page, npp, uid);
                    var Data = new { contests = contests, mlist = true, Page = Page, IsLast = IsLast };
                    string html = CommonHelper.RenderHtml("contestList.html", Data);
                    context.Response.Write(html);
                }
                #endregion

                #region
                else if (action == "getptitle")
                {
                    /*
                     * ajax
                     * */
                    context.Response.ContentType = "text/plain";
                    uint oid = (uint)Convert.ToUInt64(context.Request["oid"]);
                    string pid = context.Request["pid"].ToString();
                    Problem problem = ProblemService.SelectByOJProblemID(oid, pid);
                    if (problem.ProblemID == 0)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string json = jss.Serialize(problem);
                        context.Response.Write(json);
                    }
                }
                #endregion

                #region
                else if (action == "addnew")
                {
                    string ctitle = (string)context.Request["ctitle"];
                    string stime = (string)context.Request["stime"];
                    string tlen = (string)context.Request["tlen"];
                    string psd = (string)context.Request["psd"];
                    string dcl = (string)context.Request["dcl"];
                    string pid = (string)context.Request["pid"];
                    string ptitle = (string)context.Request["ptitle"];
                    for (int i = 0; i < psd.Length; ++i)
                    {
                        if (IsValidChar(psd[i]) == false)
                        {
                            return;
                        }
                    }
                    if (ctitle.Length > 80 || psd.Length > 24 || dcl.Length > 400)
                    {
                        return;
                    }

                    string p_stime = @"(\d\d):(\d\d):(\d\d) (\d\d)/(\d\d)/(\d\d\d\d)";
                    Regex r_stime = new Regex(p_stime);
                    Match match = r_stime.Match(stime);
                    int hh = Convert.ToInt32(match.Groups[1].ToString());
                    int mm = Convert.ToInt32(match.Groups[2].ToString());
                    int ss = Convert.ToInt32(match.Groups[3].ToString());
                    int MM = Convert.ToInt32(match.Groups[4].ToString());
                    int dd = Convert.ToInt32(match.Groups[5].ToString());
                    int yyyy = Convert.ToInt32(match.Groups[6].ToString());
                    DateTime StartTime = new DateTime(yyyy, MM, dd, hh, mm, ss);
                    string[] strs = tlen.Split(':');
                    if (Convert.ToInt32(strs[0]) > 999 || Convert.ToInt32(strs[1]) > 23 ||
                        Convert.ToInt32(strs[2]) > 59 || Convert.ToInt32(strs[3]) > 59)
                    {
                        return;
                    }
                    if (Convert.ToInt32(strs[0]) <= 0 && Convert.ToInt32(strs[1]) <= 0 &&
                        Convert.ToInt32(strs[2]) <= 0 && Convert.ToInt32(strs[3]) <= 0)
                    {
                        return;
                    }


                    DateTime EndTime = StartTime.AddDays(Convert.ToInt32(strs[0])).AddHours(Convert.ToInt32(strs[1]))
                        .AddMinutes(Convert.ToInt32(strs[2])).AddSeconds(Convert.ToInt32(strs[3]));

                    string[] pros = pid.Split("`".ToCharArray());
                    string[] protitles = ptitle.Split("`".ToCharArray());
                    if (pros.Length > 15 || pros.Length <= 0 || protitles.Length > 15 ||
                        protitles.Length <= 0 || pros.Length != protitles.Length)
                    {
                        return;
                    }
                    foreach (string title in protitles)
                    {
                        if (title.Length <= 0 || title.Length > 80)
                        {
                            return;
                        }
                    }
                    Contest contest = new Contest();
                    contest.Title = ctitle;
                    contest.Type = psd.Length > 0 ? "private" : "public";
                    contest.Password = psd;
                    contest.UserID = uid;
                    contest.StartTime = StartTime;
                    contest.EndTime = EndTime;
                    contest.Declaration = dcl;
                    contest.ProsNum = (uint)pros.Length;

                    uint cid = ContestService.Insert(contest);
                    ContestUserService.Insert(cid, uid);
                    List<ContestProblem> conpros = new List<ContestProblem>();
                    for (int i = 0; i < pros.Length; ++i)
                    {
                        ContestProblem conpro = new ContestProblem();
                        conpro.ContestID = cid;
                        conpro.ProblemID = Convert.ToUInt32(pros[i]);
                        conpro.Title = protitles[i];
                        conpro.Accepted = 0;
                        conpro.Submit = 0;
                        conpro.OrderID = (char)(i + 'A');
                        conpros.Add(conpro);
                    }
                    ContestProblemService.Insert(conpros);
                    context.Response.Redirect("ContestDo.ashx?action=mlist", false);
                    return;
                }
                #endregion

                #region
                else if (action == "editold")
                {
                    string ctitle = (string)context.Request["ctitle"];
                    string stime = (string)context.Request["stime"];
                    string tlen = (string)context.Request["tlen"];
                    string psd = (string)context.Request["psd"];
                    string dcl = (string)context.Request["dcl"];
                    string pid = (string)context.Request["pid"];
                    string ptitle = (string)context.Request["ptitle"];
                    for (int i = 0; i < psd.Length; ++i)
                    {
                        if (IsValidChar(psd[i]) == false)
                        {
                            return;
                        }
                    }
                    if (ctitle.Length > 80 || psd.Length > 24 || dcl.Length > 400)
                    {
                        return;
                    }

                    string p_stime = @"(\d\d):(\d\d):(\d\d) (\d\d)/(\d\d)/(\d\d\d\d)";
                    Regex r_stime = new Regex(p_stime);
                    Match match = r_stime.Match(stime);
                    int hh = Convert.ToInt32(match.Groups[1].ToString());
                    int mm = Convert.ToInt32(match.Groups[2].ToString());
                    int ss = Convert.ToInt32(match.Groups[3].ToString());
                    int MM = Convert.ToInt32(match.Groups[4].ToString());
                    int dd = Convert.ToInt32(match.Groups[5].ToString());
                    int yyyy = Convert.ToInt32(match.Groups[6].ToString());
                    DateTime StartTime = new DateTime(yyyy, MM, dd, hh, mm, ss);
                    string[] strs = tlen.Split(':');
                    if (Convert.ToInt32(strs[0]) > 999 || Convert.ToInt32(strs[1]) > 23 ||
                        Convert.ToInt32(strs[2]) > 59 || Convert.ToInt32(strs[3]) > 59)
                    {
                        return;
                    }
                    if (Convert.ToInt32(strs[0]) <= 0 && Convert.ToInt32(strs[1]) <= 0 &&
                        Convert.ToInt32(strs[2]) <= 0 && Convert.ToInt32(strs[3]) <= 0)
                    {
                        return;
                    }


                    DateTime EndTime = StartTime.AddDays(Convert.ToInt32(strs[0])).AddHours(Convert.ToInt32(strs[1]))
                        .AddMinutes(Convert.ToInt32(strs[2])).AddSeconds(Convert.ToInt32(strs[3]));

                    string[] pros = pid.Split("`".ToCharArray());
                    string[] protitles = ptitle.Split("`".ToCharArray());
                    if (pros.Length > 15 || pros.Length <= 0 || protitles.Length > 15 ||
                        protitles.Length <= 0 || pros.Length != protitles.Length)
                    {
                        return;
                    }
                    foreach (string title in protitles)
                    {
                        if (title.Length <= 0 || title.Length > 80)
                        {
                            return;
                        }
                    }
                    Contest contest = new Contest();
                    contest.Title = ctitle;
                    contest.Type = psd.Length > 0 ? "private" : "public";
                    contest.Password = psd;
                    contest.UserID = uid;
                    contest.StartTime = StartTime;
                    contest.EndTime = EndTime;
                    contest.Declaration = dcl;
                    contest.ProsNum = (uint)pros.Length;

                    uint cid = Convert.ToUInt32(context.Request["cid"]);
                    contest.ContestID = cid;
                    ContestService.Update(contest);
                    ContestProblemService.DeleteByContestID(contest.ContestID);

                    List<ContestProblem> conpros = new List<ContestProblem>();
                    for (int i = 0; i < pros.Length; ++i)
                    {
                        ContestProblem conpro = new ContestProblem();
                        conpro.ContestID = cid;
                        conpro.ProblemID = Convert.ToUInt32(pros[i]);
                        conpro.Title = protitles[i];
                        conpro.Accepted = 0;
                        conpro.Submit = 0;
                        conpro.OrderID = (char)(i + 'A');
                        conpros.Add(conpro);
                    }
                    ContestProblemService.Insert(conpros);
                    context.Response.Redirect("ContestDo.ashx?action=mlist", false);
                }
                #endregion

                #region
                /*
                     * ajax
                     * */
                else if (action == "getcompilers")
                {
                    context.Response.ContentType = "text/plain";
                    uint oid = (uint)Convert.ToUInt64(context.Request["oid"]);
                    List<Compiler> compilers = CompilerService.SelectByOJ(oid);
                    string ret = "";
                    for (int i = 0; i < compilers.Count; ++i)
                    {
                        if (i > 0)
                        {
                            ret += "|";
                        }
                        ret += compilers[i].CompilerID + "&" + compilers[i].Name;
                    }
                    context.Response.Write(ret);
                }
                #endregion

                #region
                else if (action == "submit")
                {
                    uint conid = Convert.ToUInt32(context.Request["cid"]);
                    Contest contest = ContestService.SelectByID(conid);
                    if (contest.EndTime <= DateTime.Now || contest.StartTime >= DateTime.Now)
                    {
                        context.Response.Write("<script>alert('Contest is not Running');window.location.href='ContestDo.ashx?action=view&cid=" + conid + "'</script>");
                        return;
                    }
                    string pid = (string)context.Request["pselect"];
                    uint cid = Convert.ToUInt32(context.Request["cselect"]);
                    string code = (string)context.Request["code"];

                    if (code.Length < 50 || code.Length > 65536)
                    {
                        context.Response.Write("<script>alert('The Code is Too Short or Too Long');window.location.href='ContestDo.ashx?action=view&cid=" + conid + "#submit'</script>");
                        return;
                    }

                    string[] strs = pid.Split('&');
                    Solution solution = new Solution();
                    solution.ContestProblemID = Convert.ToUInt32(strs[0]);
                    solution.CompilerID = cid;
                    solution.SourceCode = code;
                    solution.IsJudged = false;
                    solution.SubmitTime = DateTime.Now;
                    solution.UserID = Convert.ToUInt32(uid);
                    SolutionService.Insert(solution);
                    ContestProblemService.UpdateSubmit(solution.ContestProblemID);
                    Problem problem = ContestProblemService.SelectByContestProblemID(solution.ContestProblemID);
                    ProblemService.UpdateSubmit(problem.ProblemID);

                    context.Response.Redirect("ContestDo.ashx?action=view&cid=" + conid + "#status", false);
                }
                #endregion

                #region
                /*
                     * ajax
                     * */
                else if (action == "code")
                {
                    context.Response.ContentType = "text/plain";
                    uint id = Convert.ToUInt32(context.Request["sid"]);
                    Solution solution = SolutionService.SelectBySolutionID(id);
                    if (solution.SolutionID == 0)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        context.Response.Write(solution.SourceCode);
                    }
                }
                #endregion

                #region
                else if (action == "edit")
                {
                    uint cid = (uint)Convert.ToUInt32(context.Request["cid"]);
                    Contest contest = ContestService.SelectByID(cid);
                    if (DateTime.Now >= contest.StartTime)
                    {
                        context.Response.Write("<script>alert('Contest is Running');window.location.href='ContestDo.ashx?action=mlist'</script>");
                        return;
                    }
                    if (contest.UserID != uid)
                    {
                        return;
                    }
                    else
                    {
                        List<ContestProblem> conpros = ContestProblemService.SelectByContestID(cid);
                        foreach (ContestProblem conpro in conpros)
                        {
                            conpro.Description = conpro.Input = conpro.Output =
                                conpro.SampleInput = conpro.SampleOutput = conpro.Hint = null;
                        }
                        List<OJ> OJs = OJService.SelectAll();
                        TimeSpan Length = contest.EndTime - contest.StartTime;
                        var Data = new
                        {
                            OJs = OJs,
                            edit = true,
                            pros = conpros,
                            ID = contest.ContestID,
                            Title = contest.Title,
                            Time = contest.StartTime.ToString("HH:mm:ss MM/dd/yyyy"),
                            Length = contest.EndTime - contest.StartTime,
                            Dec = contest.Declaration,
                            Psd = contest.Password
                        };
                        string html = CommonHelper.RenderHtml("contestAdd.html", Data);
                        context.Response.Write(html);
                    }
                }
                #endregion

                #region
                else
                {
                    context.Response.Redirect("index.ashx", false);
                }
                #endregion
            }
            catch (Exception e)
            {
                LogService.Insert(0, e);
                context.Response.Redirect("index.ashx", false);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}