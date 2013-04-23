
namespace Fanex.CRM.Infrastructure.Core
{
    using System;
    using Fanex.CRM.Infrastructure.Common;
    using DNAide.Core;

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
        public static string ChangeTimeZoneToClient(DateTime Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Time.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClient(string Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var Client = CastAide.AsDateTime(Time);
            return Client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClientDate(string Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var Client = CastAide.AsDateTime(Time);
            return Client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy");
        }

        public static string ChangeTimeZoneToClientTime(string Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            var Client = CastAide.AsDateTime(Time);
            return Client.Add(timeSpan._timeSpan).ToString("hh:mm tt");
        }


        public static string ChangeTimeZoneToServer(DateTime Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Time.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToServer(string Time)
        {
            var Client = CastAide.AsDateTime(Time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();

            return Client.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }
        #endregion

        #region Convert Time for report client <->server

        public static string ChangeTimeToServerForReportBegin(DateTime Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Time.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportBegin(string Time)
        {
            var Server = CastAide.AsDateTime(Time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Server.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportEnd(DateTime Time)
        {
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Time.Subtract(timeSpan._timeSpan).AddDays(1).AddSeconds(-1).ToString("MM/dd/yyyy hh:mm");
        }

        public static string ChangeTimeToServerForReportEnd(string Time)
        {
            var Server = CastAide.AsDateTime(Time);
            DateTimeUtilize timeSpan = new DateTimeUtilize();
            return Server.Subtract(timeSpan._timeSpan).AddDays(1).AddSeconds(-1).ToString("MM/dd/yyyy hh:mm");
        }

        #endregion

    }
}
