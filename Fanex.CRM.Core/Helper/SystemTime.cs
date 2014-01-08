// --------------------------------------------------------------------------
// <copyright file="SystemTime.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helper
{
    using System;

    /// <summary>
    /// System Time.
    /// </summary>
    public static class SystemTime
    {
        static SystemTime()
        {
            Now = () => DateTime.UtcNow;
        }

        public static Func<DateTime> Now { get; set; }
    }
}