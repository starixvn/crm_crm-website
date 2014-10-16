// --------------------------------------------------------------------------
// <copyright file="IWeeklyStatisticsReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.WeeklyStatisticsReport
{
    using Fanex.CRM.Report.Criteria.WeeklyStatisticsReport;
    using Fanex.CRM.Report.Models.WeeklyStatisticsReport;

    public interface IWeeklyStatisticsReportService
    {
        WeeklyStatisticsReportInfo MainReport(GetMainReportCriteria criteria);

        void ExportMainReport(GetMainReportCriteria criteria);

        WeeklyStatisticsDetailReportInfo DetailReport(GetDetailReportCriteria criteria);

        void ExportDetailReport(GetDetailReportCriteria criteria);
    }
}
