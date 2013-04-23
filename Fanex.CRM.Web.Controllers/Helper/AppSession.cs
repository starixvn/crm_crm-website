namespace Fanex.CRM.Web.Controllers.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Extension;

    public class AppSession
    {
        public static User CurrentUser
        {
            get
            {
                return SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo") as User;
            }
            set { HttpContext.Current.Session["CRMUserInfo"] = value; }
        }

        public static string CurrentCustomer
        {
            get { return SessionExtension.GetSession(HttpContext.Current.Session, "CurrentCustomer") as string; }
            set { SessionExtension.AddSession(HttpContext.Current.Session, "CurrentCustomer", value); }
        }

        public static Dictionary<string, int> PageAccess
        {
            get { return (Dictionary<string, int>)HttpContext.Current.Session["PageAccess"]; }
            set { HttpContext.Current.Session["PageAccess"] = value; }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get { return (Dictionary<string, int>)HttpContext.Current.Session["RoleAccess"]; }
            set { HttpContext.Current.Session["RoleAccess"] = value; }
        }

        public static string UserSessionId
        {
            get { return (string)HttpContext.Current.Session["usersessionid"]; }
            set { HttpContext.Current.Session["usersessionid"] = value; }
        }

        public static IEnumerable<Menu> UserMenu
        {
            get
            {
                if (HttpContext.Current.Session["UserMenu"] != null)

                    return (List<Menu>)HttpContext.Current.Session["UserMenu"];
                else
                    return null;
            }
            set { HttpContext.Current.Session["UserMenu"] = value; }
        }

    }
}