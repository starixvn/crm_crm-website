namespace Fanex.CRM.Report.Repositories.RedemptionStatisticsReport
{
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using System.Collections.Generic;

    public interface IRedemptionStatisticsRepository
    {
        IEnumerable<RedemptionStatisticsEntity> GetRedemptionSummary(GetRedemptionStatisticsSummaryCriteria criteria);

        IEnumerable<RedemptionStatisticsDetailEntity> GetRedemptionDetail(GetRedemptionStatisticsDetailCriteria criteria);
    }
}
