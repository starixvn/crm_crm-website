// --------------------------------------------------------------------------
// <copyright file="ISummaryReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Interfaces
{
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;

    /// <summary>
    /// ISummaryReportService interface.
    /// </summary>
    public interface ISummaryReportService
    {
        dynamic GetAllSettings();

        SummaryReportModel GetSummaryReport(GetSummaryReportCriteria criteria);

        void ExportSummaryReportExcel(GetSummaryReportCriteria criteria);
    }
}