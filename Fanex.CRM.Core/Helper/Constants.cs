// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Constants class.
    /// </summary>
    public static class Constants
    {
        public const string CurrentLogInUser = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentLogInUser";
        public const string CurrentUserMenu = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentUserMenu";
        public const string CurrentUserRoleAccess = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentUserRoleAccess";
        public const string CurrentWorkingOnMenu = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentWorkingOnMenu";
        public const string CurrentActionName = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentActionName";
        public const string CurrentHttpMethod = "Fanex.CRM.Infrastructure.Helper.Contants.AppSession.CurrentHttpMethod";

        public static readonly DateTime ProductionDate = new DateTime(2008, 1, 11);

        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Systems class.
        /// </summary>
        public static class Systems
        {
            public const string CRM = "CRM";
        }
    }
}