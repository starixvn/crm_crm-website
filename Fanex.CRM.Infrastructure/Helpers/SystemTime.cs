// --------------------------------------------------------------------------
// <copyright file="SystemTime.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helpers
{
    using System;

    /// <summary>
    /// System Time.
    /// </summary>
    public static class SystemTime
    {
        public static Func<DateTime> Now
        {
            get
            {
                return () => DateTime.UtcNow;
            }
        }
    }
}