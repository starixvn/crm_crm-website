// -----------------------------------------------------------------------
// <copyright file="ICasinoReportEnhancementRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CasinoReportEnhancement
{
    using System;
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;

    public interface ICasinoReportEnhancementRepository
    {
        StatisticsInfoByDateEnhancementRepositoryResultModel GetReportData(CasinoEnhancementReportCriteria criteria);
    }
}
