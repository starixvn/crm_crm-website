// --------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;
    using DNAide.Core;

    /// <summary>
    /// Session Extension.
    /// </summary>
    public static class SessionExtension
    {
        public static Dictionary<string, int> RoleAccess
        {
            get { return (Dictionary<string, int>)HttpContext.Current.Session["RoleAccess"]; }

            set { HttpContext.Current.Session["RoleAccess"] = value; }
        }

        public static int OnlineUserId
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["useronlineid"]); }

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

        public static void DeleteSession(HttpSessionState sessionState, string key)
        {
            sessionState[key] = null;
        }
    }
}
