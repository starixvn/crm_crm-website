// --------------------------------------------------------------------------
// <copyright file="DateTimeUtilize.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Core
{
    using System;
    using System.Collections.Generic;
    using DNAide.Core;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Date Time Utilize.
    /// </summary>
    public class DateTimeUtilize
    {
        private TimeSpan _timeSpan;

        public DateTimeUtilize()
        {
            this._timeSpan = new TimeSpan(AppSetting.TimeSpan, 0, 0);
        }

        public static DateTime ResetEndDay(DateTime date)
        {
            return date.AddDays(1).AddSeconds(-1);
        }

        #region Convert time client <->server
        public static string ChangeTimeZoneToClient(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
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

        #region Convert Time for report client <->server

        public static string ChangeTimeToServerForReportBegin(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportBegin(string time)
        {
            var server = CastAide.AsDateTime(time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return server.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportEnd(DateTime time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return time.Subtract(timeSpan._timeSpan).AddDays(1).AddSeconds(-1).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportEnd(string time)
        {
            var server = CastAide.AsDateTime(time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return server.Subtract(timeSpan._timeSpan).AddDays(1).AddSeconds(-1).ToString("MM/dd/yyyy hh:mm");
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

        public static DateTime GetCurrentClientTime()
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return DateTime.Now.Add(timeSpan._timeSpan);
        }

        public static DateTime ChangeClientFromTimeToServerTime(DateTime clientFromTime)
        {
            return DateTimeUtilize.ChangeTimeZoneToServerDateTime(clientFromTime.Date);
        }

        public static DateTime ChangeClientToTimeToServerTime(DateTime clientToTime)
        {
            return DateTimeUtilize.ChangeTimeZoneToServerDateTime(clientToTime.GetEndOfDate());
        }
    }
}
