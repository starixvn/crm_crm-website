// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DailyStatisticsReport
{
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.StatisticByDateReport;

    /// <summary>
    /// Daily Statistics Report Service.
    /// </summary>
    public class DailyStatisticsReportService : StatisticByDateReportService
    {
        public override string MainReportSessionKey
        {
            get
            {
                return "Fanex.CRM.Report.DailyStatisticsReport.MainReportSessionKey_" +
                    (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);
            }
        }

        public override string RawDetailReportSessionKey
        {
            get
            {
                return "Fanex.CRM.Report.DailyStatisticsReport.RawDetailReportSessionKey_" +
                    (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);
            }
        }

        public override string MainReportTitle()
        {
            return Labels.DailyStatisticsReport_Title;
        }

        public override string MainReportSheetName()
        {
            return Labels.Daily_Statistics;
        }

        public override string DetailReportTitle()
        {
            return Labels.DailyStatisticsReport_DetailTitle;
        }
    }
}
