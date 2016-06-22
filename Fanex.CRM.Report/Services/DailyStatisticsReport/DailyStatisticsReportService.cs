// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DailyStatisticsReport
{
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.StatisticByDateReport;

    /// <summary>
    /// Daily Statistics Report Service.
    /// </summary>
    public class DailyStatisticsReportService : StatisticByDateReportService
    {
        public override string GetMainReportTitle()
        {
            return Labels.DailyStatisticsReport_Title;
        }

        public override string GetMainReportSheetName()
        {
            return Labels.Daily_Statistics;
        }

        public override string GetDetailReportTitle()
        {
            return Labels.DailyStatisticsReport_DetailTitle;
        }
    }
}
