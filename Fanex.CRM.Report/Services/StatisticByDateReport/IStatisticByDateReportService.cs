// --------------------------------------------------------------------------
// <copyright file="IStatisticByDateReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.StatisticByDateReport
{
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface IStatisticByDateReportService
    {
        StatisticsReportByDateModel MainReport(GetMainReportCriteria criteria);

        void ExportMainReport(GetMainReportCriteria criteria);

        StatisticsDetailReportByDateModel DetailReport(GetDetailReportCriteria criteria);

        void ExportDetailReport(GetDetailReportCriteria criteria);
    }
}
