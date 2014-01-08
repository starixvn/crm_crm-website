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