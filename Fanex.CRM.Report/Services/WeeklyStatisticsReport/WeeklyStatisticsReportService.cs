// --------------------------------------------------------------------------
// <copyright file="WeeklyStatisticsReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.WeeklyStatisticsReport
{
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.StatisticByDateReport;

    /// <summary>
    /// Weekly Statistics Report Service.
    /// </summary>
    public class WeeklyStatisticsReportService : StatisticByDateReportService
    {
        public override string MainReportSessionKey
        {
            get
            {
                return "Fanex.CRM.Report.WeeklyStatisticsReport.MainReportSessionKey_" +
                    (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);
            }
        }

        public override string RawDetailReportSessionKey
        {
            get
            {
                return "Fanex.CRM.Report.WeeklyStatisticsReport.RawDetailReportSessionKey_" +
                    (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);
            }
        }

        public override string MainReportTitle()
        {
            return Labels.WeeklyStatisticsReport_Title;
        }

        public override string MainReportSheetName()
        {
            return Labels.Weekly_Statistics;
        }

        public override string DetailReportTitle()
        {
            return Labels.WeeklyStatisticsReport_DetailTitle;
        }
    }
}
