// --------------------------------------------------------------------------
// <copyright file="DateTimeExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Date Time Extension.
    /// </summary>
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        [DebuggerStepThrough]
        public static bool IsInRange(this DateTime target, DateTime fromDate, DateTime toDate)
        {
            return (target >= fromDate) && (target <= toDate);
        }
    }
}