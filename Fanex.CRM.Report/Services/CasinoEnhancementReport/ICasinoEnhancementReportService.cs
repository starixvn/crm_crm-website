// -----------------------------------------------------------------------
// <copyright file="ICasinoEnhancementReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.CasinoEnhancementReport
{
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;

    public interface ICasinoEnhancementReportService
    {
        CasinoReportEnhancementViewModel GetReportData(CasinoEnhancementReportCriteria criteria);
    }
}
