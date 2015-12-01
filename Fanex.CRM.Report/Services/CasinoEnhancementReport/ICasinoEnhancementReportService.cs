// -----------------------------------------------------------------------
// <copyright file="ICasinoEnhancementReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.CasinoEnhancementReport
{
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface ICasinoEnhancementReportService
    {
        StatisticsReportByDateModel GetReportData(CasinoEnhancementReportCriteria criteria);
    }
}
