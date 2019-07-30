namespace Fanex.CRM.Common.Helpers
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Services;

    public class DateTimeUtilize
    {
        private static readonly string[] MonthName =
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
            _timeSpan = new TimeSpan(AppSetting.TimeSpan, 0, 0);
        }

        public static DateTime ResetEndDay(DateTime date)
        {
            return date.AddDays(1).AddSeconds(-1);
        }

        public static string GetMonthName(int month)
        {
            return MonthName[month - 1];
        }

        public static string GetDateTimeString(DateTime time)
        {
            return time.ToString("MM/dd/yyyy hh:mm tt");
        }

        public static int GetTimeSpan()
        {
            return AppSetting.TimeSpan;
        }

        #region Convert time client <->server

        public static string ChangeFullTimeZoneToClient(DateTime time)
        {
            var timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("hh:mm:ss tt MMM dd, yyyy");
        }

        public static string ChangeFullTimeZoneToClientForActionLog(DateTime time)
        {
            var timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("MMM dd, yyyy hh:mm:ss tt");
        }

        public static string ChangeTimeZoneToClient(DateTime time)
        {
            var timeSpan = new DateTimeUtilize();
            return time.Add(timeSpan._timeSpan).ToString("MMM dd, yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClient(string time)
        {
            var timeSpan = new DateTimeUtilize();
            var client = Convert.ToDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToClientDate(string time)
        {
            var timeSpan = new DateTimeUtilize();
            var client = Convert.ToDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("MM/dd/yyyy");
        }

        public static string ChangeTimeZoneToClientTime(string time)
        {
            var timeSpan = new DateTimeUtilize();
            var client = Convert.ToDateTime(time);
            return client.Add(timeSpan._timeSpan).ToString("hh:mm tt");
        }

        public static string ChangeTimeZoneToServer(DateTime time)
        {
            var timeSpan = new DateTimeUtilize();
            return time.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string ChangeTimeZoneToServer(string time)
        {
            var client = Convert.ToDateTime(time);
            var timeSpan = new DateTimeUtilize();

            return client.Subtract(timeSpan._timeSpan).ToString("MM/dd/yyyy hh:mm tt");
        }

        #endregion Convert time client <->server

        #region Convert Time for report client <-> server

        public static string ChangeTimeToServerForReportBegin(DateTime time)
        {
            // Get begin time.
            var beginTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            return FormatTimeForReport(beginTime);
        }

        public static string ChangeTimeToServerForReportBegin(string time)
        {
            var server = Convert.ToDateTime(time);
            return ChangeTimeToServerForReportBegin(server);
        }

        public static string ChangeTimeToServerForReportEnd(DateTime time)
        {
            // Get end time.
            var endTime = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
            return FormatTimeForReport(endTime);
        }

        public static string ChangeTimeToServerForReportEnd(string time)
        {
            var server = Convert.ToDateTime(time);
            return ChangeTimeToServerForReportEnd(server);
        }

        #endregion Convert Time for report client <-> server

        public static DateTime ChangeTimeZoneToClientDateTime(DateTime dateTime)
        {
            var timeSpan = new DateTimeUtilize();
            return dateTime.Add(timeSpan._timeSpan);
        }

        public static DateTime ChangeTimeZoneToServerDateTime(DateTime dateTime)
        {
            var timeSpan = new DateTimeUtilize();
            return dateTime.Subtract(timeSpan._timeSpan);
        }

        public static DateTime CurrentClientTime()
        {
            var timeSpan = new DateTimeUtilize();
            return DateTime.Now.Add(timeSpan._timeSpan);
        }

        public static DateTime ChangeClientFromTimeToServerTime(DateTime? clientFromTime)
        {
            var fromTime = clientFromTime ?? DateTime.Now;
            return ChangeClientFromTimeToServerTime(fromTime);
        }

        public static DateTime ChangeClientToTimeToServerTime(DateTime? clientToTime)
        {
            var toTime = clientToTime ?? DateTime.Now;
            return ChangeClientToTimeToServerTime(toTime);
        }

        public static DateTime ChangeClientFromTimeToServerTime(DateTime clientFromTime)
        {
            return ChangeTimeZoneToServerDateTime(clientFromTime.Date);
        }

        public static DateTime ChangeClientToTimeToServerTime(DateTime clientToTime)
        {
            return ChangeTimeZoneToServerDateTime(clientToTime.GetEndOfDate());
        }

        #region Convert Time for report client <-> server (cont...)

        private static string FormatTimeForReport(DateTime time)
        {
            var timeSpan = new DateTimeUtilize()._timeSpan;
            return time.Subtract(timeSpan).ToString("MM/dd/yyyy HH:mm:ss");
        }

        #endregion Convert Time for report client <-> server (cont...)
    }
}