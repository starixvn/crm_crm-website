// --------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// Session Extension.
    /// </summary>
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
