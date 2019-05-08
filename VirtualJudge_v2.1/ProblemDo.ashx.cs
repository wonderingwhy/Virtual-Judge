using DAL;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualJudge_v2._1
{
    /// <summary>
    /// ProblemDo 的摘要说明
    /// </summary>
    public class ProblemDo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/html";
                string action = context.Request["action"];

                if (action == "view")
                {
                    uint ID = Convert.ToUInt32(context.Request["pid"]);
                    Problem problem = ProblemService.SelectByProblemID(ID);
                    var Data = new { problem = problem };
                    string html = CommonHelper.RenderHtml("problemView.html", Data);
                    context.Response.Write(html);
                }
                else
                {
                    context.Response.Redirect("index.ashx", false);
                }
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