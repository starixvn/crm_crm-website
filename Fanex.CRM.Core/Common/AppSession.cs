// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Infrastructure.Common
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSession
    {
        private const string CurrentLogInUserKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentLogInUser";
        private const string CurrentUserMenuKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentUserMenu";
        private const string CurrentUserRoleAccessKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentUserRoleAccess";
        private const string CurrentWorkingOnMenuKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentWorkingOnMenu";
        private const string CurrentActionNameKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentActionName";
        private const string CurrentHttpMethodKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentHttpMethod";
        private const string RedirectUrlKey = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.RedirectUrl";

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public static User CurrentUser
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentLogInUserKey) as User;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentLogInUserKey, value);
            }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentUserRoleAccessKey) as Dictionary<string, int>;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentUserRoleAccessKey, value);
            }
        }

        /// <summary>
        /// Gets or sets menu of current login user.
        /// </summary>
        /// <value>
        /// The current user menu.
        /// </value>
        public static IList<MenuItem> CurrentUserMenu
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentUserMenuKey) as IList<MenuItem>;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentUserMenuKey, value);
            }
        }
        
        public static string CurrentHttpMethod
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentHttpMethodKey) as string;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentHttpMethodKey, value);
            }
        }

        /// <summary>
        /// Gets or sets IsTrackedMenu: determine current working on menu is
        /// tracked for page tracking or not.
        /// </summary>
        /// <value>
        /// The name of the current action.
        /// </value>
        public static string CurrentActionName
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentActionNameKey) as string;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentActionNameKey, value);
            }
        }

        /// <summary>
        /// Gets or sets current working menu url.
        /// </summary>
        /// <value>
        /// The current working on menu.
        /// </value>
        public static MenuItem CurrentWorkingOnMenu
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentWorkingOnMenuKey) as MenuItem;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentWorkingOnMenuKey, value);
            }
        }

        public static string RedirectUrl 
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, RedirectUrlKey) as string;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, RedirectUrlKey, value);
            }
        }

        private static HttpSessionState CurrentSession
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }
    }
}
