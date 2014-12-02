// --------------------------------------------------------------------------
// <copyright file="DateTimeUtilize.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using System;
    using DNAide.Core;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Extensions;

    /// <summary>
    /// Date Time Utilize.
    /// </summary>
    public class DateTimeUtilize
    {
        private static string[] monthName =
                                    {
                                        "January",
                                        "February", 
                                        "March", 
                                        "April", 
                                        "May", 
                                        "June",
                                        "July", 
                                        "August", 
                                        "September", 
                                        "October", 
                                        "November", 
                                        "December"
                                    };

        private TimeSpan _timeSpan;

        public DateTimeUtilize()
        {
            this._timeSpan = new TimeSpan(AppSetting.TimeSpan, 0, 0);
        }

        public static DateTime ResetEndDay(DateTime date)
        {
            return date.AddDays(1).AddSeconds(-1);
        }

        public static string GetMonthName(int month)
        {
            return DateTimeUtilize.monthName[month - 1];
        }

        #region Convert time client <->server
        public static string ChangeFullTimeZoneToClient(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("hh:mm:ss tt MMM dd, yyyy");
        }

        public static string ChangeFullTimeZoneToClientForActionLog(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("MMM dd, yyyy hh:mm:ss tt");
        }

        public static string ChangeTimeZoneToClient(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("MMM dd, yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClient(string time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var client = CastAide.AsDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClientDate(string time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var client = CastAide.AsDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy");
        }

        public static string ChangeTimeZoneToClientTime(string time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var client = CastAide.AsDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("hh:mm tt");
        }

        public static string ChangeTimeZoneToServer(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToServer(string time)
        {
            var client = CastAide.AsDateTime(time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();

            return client.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }
        #endregion

        #region Convert Time for report client <-> server

        public static string ChangeTimeToServerForReportBegin(DateTime time)
        {
            // Get begin time.
            DateTime beginTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            return FormatTimeForReport(beginTime);
        }

        public static string ChangeTimeToServerForReportBegin(string time)
        {
            var server = CastAide.AsDateTime(time);
            return ChangeTimeToServerForReportBegin(server);
        }

        public static string ChangeTimeToServerForReportEnd(DateTime time)
        {
            // Get end time.
            DateTime endTime = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
            return FormatTimeForReport(endTime);
        }

        public static string ChangeTimeToServerForReportEnd(string time)
        {
            var server = CastAide.AsDateTime(time);
            return ChangeTimeToServerForReportEnd(server);
        }

        #endregion

        public static DateTime ChangeTimeZoneToClientDateTime(DateTime dateTime)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return dateTime.Add(timeSpan._timeSpan);
        }

        public static DateTime ChangeTimeZoneToServerDateTime(DateTime dateTime)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return dateTime.Subtract(timeSpan._timeSpan);
        }

        public static DateTime CurrentClientTime()
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return DateTime.Now.Add(timeSpan._timeSpan);
        }

        public static DateTime ChangeClientFromTimeToServerTime(DateTime? clientFromTime)
        {
            DateTime fromTime = clientFromTime ?? DateTime.Now;
            return ChangeClientFromTimeToServerTime(fromTime);
        }

        public static DateTime ChangeClientToTimeToServerTime(DateTime? clientToTime)
        {
            DateTime toTime = clientToTime ?? DateTime.Now;
            return ChangeClientToTimeToServerTime(toTime);
        }

        public static DateTime ChangeClientFromTimeToServerTime(DateTime clientFromTime)
        {
            return DateTimeUtilize.ChangeTimeZoneToServerDateTime(clientFromTime.Date);
        }

        public static DateTime ChangeClientToTimeToServerTime(DateTime clientToTime)
        {
            return DateTimeUtilize.ChangeTimeZoneToServerDateTime(clientToTime.GetEndOfDate());
        }

        #region Convert Time for report client <-> server (cont...)

        private static string FormatTimeForReport(DateTime time)
        {
            TimeSpan timeSpan = new DateTimeUtilize()._timeSpan;
            return time.Subtract(timeSpan).ToString("MM/dd/yyyy HH:mm:ss");
        }

        #endregion
    }
}
