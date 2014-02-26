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
        public const string ShortDateFormat = "MMM dd, yyyy";

        public const string LongDateFormat = "MM/dd/yyyy HH:mm tt";

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

        public static string ToShortFormatedDate(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }

        public static string ToLongFormatedDate(this DateTime date)
        {
            return date.ToString(LongDateFormat);
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public static DateTime LastDayOfYear(this DateTime dateTime)
        {
            return dateTime.FirstDayOfYear().AddYears(1).AddDays(-1);
        }

        public static DateTime GetEndOfDate(this DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day).AddDays(1).AddSeconds(-1);
            return date;
        }

        public static DateTime GetStartOfDate(this DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            return date;
        }
    }
}