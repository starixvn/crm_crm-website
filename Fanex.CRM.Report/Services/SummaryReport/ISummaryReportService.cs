// --------------------------------------------------------------------------
// <copyright file="ISummaryReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.SummaryReport
{
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;

    /// <summary>
    /// ISummaryReportService interface.
    /// </summary>
    public interface ISummaryReportService
    {
        dynamic AllSettings();

        SummaryReportModel GetSummaryReport(GetSummaryReportCriteria criteria);

        void ExportSummaryReportExcel(GetSummaryReportCriteria criteria);
    }
}