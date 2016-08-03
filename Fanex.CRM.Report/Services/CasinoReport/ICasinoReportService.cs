// --------------------------------------------------------------------------
// <copyright file="ICasinoReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.CasinoReport
{
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;

    public interface ICasinoReportService
    {
        // Casino Daily Statistic.
        CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria);

        void ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria);

        CasinoDetail GetCasinoDailyDetail(GetCasinoDetailCriteria criteria);

        void ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria);

        // Casino Weekly Statistic.
        CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria);

        void ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria);

        CasinoDetail GetCasinoWeeklyDetail(GetCasinoDetailCriteria criteria);
    }
}
