// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Infrastructure.Common
{
    using System.Collections.Generic;
    using System.Web;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSession
    {
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
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentLogInUser) as User;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentLogInUser, value);
            }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentUserRoleAccess) as Dictionary<string, int>;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentUserRoleAccess, value);
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
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentUserMenu) as IList<MenuItem>;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentUserMenu, value);
            }
        }
        
        public static string CurrentHttpMethod
        {
            get
            {
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentHttpMethod) as string;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentHttpMethod, value);
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
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentActionName) as string;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentActionName, value);
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
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.CurrentWorkingOnMenu) as MenuItem;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.CurrentWorkingOnMenu, value);
            }
        }

        public static string RedirectUrl 
        {
            get
            {
                return SessionExtension.GetSession(HttpContext.Current.Session, Constants.RedirectUrl) as string;
            }

            set
            {
                SessionExtension.AddSession(HttpContext.Current.Session, Constants.RedirectUrl, value);
            }
        }
    }
}
