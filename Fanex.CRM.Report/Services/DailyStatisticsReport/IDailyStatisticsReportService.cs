// --------------------------------------------------------------------------
// <copyright file="IDailyStatisticsReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DailyStatisticsReport
{
    using Fanex.CRM.Report.Criteria.DailyStatisticsReport;
    using Fanex.CRM.Report.Models.DailyStatisticsReport;

    public interface IDailyStatisticsReportService
    {
        DailyStatisticsReportInfo MainReport(GetMainReportCriteria criteria);

        void ExportMainReport(GetMainReportCriteria criteria);

        DailyStatisticsDetailReportInfo DetailReport(GetDetailReportCriteria criteria);

        void ExportDetailReport(GetDetailReportCriteria criteria);
    }
}
