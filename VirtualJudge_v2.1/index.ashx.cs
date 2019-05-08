using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualJudge_v2._1
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try 
            {
                SqlHelper.OpenConnection();
                string html = CommonHelper.RenderHtml("index.html");
                context.Response.Write(html);
            }
            catch(Exception e)
            {
                string html = CommonHelper.RenderHtml("error.html");
                context.Response.Write(html);
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