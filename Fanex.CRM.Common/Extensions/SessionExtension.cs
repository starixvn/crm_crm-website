namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Web;
    using System.Web.SessionState;

    public static class SessionExtension
    {
        public static int OnlineUserId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["useronlineid"]); }

            set { HttpContext.Current.Session["useronlineid"] = value; }
        }

        public static object GetSession(HttpSessionState sessionState, string key)
        {
            return sessionState[key];
        }

        public static void AddSession(HttpSessionState sessionState, string key, object value)
        {
            sessionState[key] = value;
        }
    }
}