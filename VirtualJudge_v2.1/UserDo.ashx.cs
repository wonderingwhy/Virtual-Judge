using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VirtualJudge_v2._1
{
    /// <summary>
    /// UserDo 的摘要说明
    /// </summary>
    public class UserDo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"];
            try
            {
                #region
                if (action == "login")
                {

                    string uname = context.Request["uid"].ToString();
                    string pword = context.Request["psd"].ToString();
                    User u = UserService.SelectByUsername(uname);
                    if (u.UserID == 0 || pword != u.Password)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();
                        CookieService.Insert(new CookieItem(guid, u.UserID));
                        context.Response.SetCookie(new HttpCookie("GUID", guid.ToString()));
                        context.Response.Write("ok");
                    }

                }
                #endregion

                #region
                else if (action == "auto")
                {

                    HttpCookie cookie = context.Request.Cookies["GUID"];
                    if (cookie == null)
                    {
                        context.Response.Write("no");
                        return;
                    }
                    uint uid = CookieService.SelectByGUID(cookie.Value);
                    if (uid == 0)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        string username = UserService.SelectUsernameByID(uid);
                        if (username == null || username == "")
                        {
                            context.Response.Write("no");
                        }
                        else
                        {
                            context.Response.Write(username);
                        }
                    }

                }
                #endregion

                #region
                else if (action == "logout")
                {
                    HttpCookie cookie = context.Request.Cookies["GUID"];
                    CookieService.DeleteByGUID(cookie.Value);
                    context.Response.Write("ok");
                }
                #endregion

                #region
                else if (action == "reg")
                {

                    string uname = context.Request["uid"].ToString();
                    string pword = context.Request["psd"].ToString();
                    string nname = context.Request["nnm"].ToString();
                    User user = new User(uname, pword, nname);
                    if (user.CheckValid() == false)
                    {
                        context.Response.Write("no");
                    }
                    uint uid = UserService.SelectIDByUsername(uname);
                    if (uid != 0)
                    {
                        context.Response.Write("exist");
                    }
                    else
                    {
                        UserService.Insert(user);
                        context.Response.Write("ok");
                    }

                }
                #endregion

                #region
                else if (action == "getinfo")
                {

                    HttpCookie cookie = context.Request.Cookies["GUID"];
                    uint uid = CookieService.SelectByGUID(cookie.Value);
                    if (uid == 0)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        User user = UserService.SelectByUserID(uid);
                        user.Password = "";
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string json = jss.Serialize(user);
                        context.Response.Write(json);
                    }

                }
                #endregion

                #region
                else if (action == "modify")
                {

                    HttpCookie cookie = context.Request.Cookies["GUID"];
                    uint uid = CookieService.SelectByGUID(cookie.Value);
                    if (uid == 0)
                    {
                        context.Response.Write("no");
                        return;
                    }
                    User u = UserService.SelectByUserID(uid);
                    string opword = context.Request["npsd"].ToString();
                    string npword = context.Request["npsd"].ToString();
                    string nname = context.Request["nnm"].ToString();
                    if (opword != u.Password)
                    {
                        context.Response.Write("wrong");
                        return;
                    }
                    u.Password = npword;
                    u.Nickname = nname;
                    if (u.CheckValid() == false)
                    {
                        context.Response.Write("no");
                    }
                    else
                    {
                        UserService.Update(u);
                        context.Response.Write("ok");
                    }
                }
                #endregion

                #region
                else
                {
                    context.Response.Redirect("index.ashx");
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