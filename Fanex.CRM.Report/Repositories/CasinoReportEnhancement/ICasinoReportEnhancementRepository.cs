// -----------------------------------------------------------------------
// <copyright file="ICasinoReportEnhancementRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CasinoReportEnhancement
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;

    public interface ICasinoReportEnhancementRepository
    {
        IEnumerable<StatisticByDateEnhancementEntity> GetReportData(CasinoEnhancementReportCriteria criteria);
    }
}
