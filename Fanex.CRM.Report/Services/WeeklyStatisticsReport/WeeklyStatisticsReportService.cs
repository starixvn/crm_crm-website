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
        public override string GetMainReportTitle()
        {
            return Labels.WeeklyStatisticsReport_Title;
        }

        public override string GetMainReportSheetName()
        {
            return Labels.Weekly_Statistics;
        }

        public override string GetDetailReportTitle()
        {
            return Labels.WeeklyStatisticsReport_DetailTitle;
        }
    }
}
