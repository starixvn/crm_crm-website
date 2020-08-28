namespace CRM.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Pending")]
    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "MMM dd, yyyy";
        public const string ShortDateTimeFormat = "MMM dd, yyyy hh:mm tt";
        public const string LongDateTimeFormat = "MMM dd, yyyy hh:mm:ss tt";

        public static DateTime ToStartOfDay(this DateTime date) => new DateTime(date.Year, date.Month, date.Day);

        public static DateTime ToEndOfDay(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime date) => date.FirstDayOfWeek().AddDays(6);

        public static DateTime FirstDayOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static DateTime LastDayOfMonth(this DateTime date) => date.FirstDayOfMonth().AddMonths(1).AddDays(-1);

        public static string ToShortDateFormat(this DateTime date) => date.ToString(ShortDateFormat);

        public static string ToShortDateTimeFormat(this DateTime date) => date.ToString(ShortDateTimeFormat);

        public static string ToLongDateTimeFormat(this DateTime date) => date.ToString(LongDateTimeFormat);
    }
}