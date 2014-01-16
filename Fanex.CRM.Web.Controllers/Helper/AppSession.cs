// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers.Helper
{
    using System.Collections.Generic;
    using System.Web;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Extension;

    /// <summary>
    /// Application Session.
    /// </summary>
    public class AppSession
    {
        ////public static User CurrentUser
        ////{
        ////    get
        ////    {
        ////        return SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo") as User;
        ////    }

        ////    set
        ////    {
        ////        HttpContext.Current.Session["CRMUserInfo"] = value;
        ////    }
        ////}

        ////public static string CurrentCustomer
        ////{
        ////    get
        ////    {
        ////        return SessionExtension.GetSession(HttpContext.Current.Session, "CurrentCustomer") as string;
        ////    }

        ////    set
        ////    {
        ////        SessionExtension.AddSession(HttpContext.Current.Session, "CurrentCustomer", value);
        ////    }
        ////}

        ////public static Dictionary<string, int> PageAccess
        ////{
        ////    get
        ////    {
        ////        return (Dictionary<string, int>)HttpContext.Current.Session["PageAccess"];
        ////    }

        ////    set
        ////    {
        ////        HttpContext.Current.Session["PageAccess"] = value;
        ////    }
        ////}

        ////public static Dictionary<string, int> RoleAccess
        ////{
        ////    get
        ////    {
        ////        return (Dictionary<string, int>)HttpContext.Current.Session["RoleAccess"];
        ////    }

        ////    set
        ////    {
        ////        HttpContext.Current.Session["RoleAccess"] = value;
        ////    }
        ////}

        ////public static string UserSessionId
        ////{
        ////    get
        ////    {
        ////        return (string)HttpContext.Current.Session["usersessionid"];
        ////    }

        ////    set
        ////    {
        ////        HttpContext.Current.Session["usersessionid"] = value;
        ////    }
        ////}

        ////public static IEnumerable<Menu> UserMenu
        ////{
        ////    get
        ////    {
        ////        return HttpContext.Current.Session["UserMenu"] != null ?
        ////            (List<Menu>)HttpContext.Current.Session["UserMenu"] :
        ////            null;
        ////    }

        ////    set
        ////    {
        ////        HttpContext.Current.Session["UserMenu"] = value;
        ////    }
        ////}
    }
}