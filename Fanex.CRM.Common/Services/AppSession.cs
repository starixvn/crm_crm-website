// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Common.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using NAP.Core.Models;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AppSession
    {
        private const string CurrentLogInUserKey = "CurrentLogInUser";
        private const string CurrentUserMenuKey = "CurrentUserMenu";

        public static void UpdateLanguage(string language)
        {
            var user = CurrentUser;
            if (user != null)
            {
                user.Language = language;
                CurrentUser = user;
            }
        }

        public static SearchedCustomer CurrentCustomer
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, "SearchedCustomer") as SearchedCustomer;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, "SearchedCustomer", value);
            }
        }

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public static CRMUser CurrentUser
        {
            get
            {
                return SessionExtension.GetSession(CurrentSession, CurrentLogInUserKey) as CRMUser;
            }

            set
            {
                SessionExtension.AddSession(CurrentSession, CurrentLogInUserKey, value);
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

        private static HttpSessionState CurrentSession
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }
    }
}