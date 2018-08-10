namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Entities.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using System.Collections.Generic;

    public interface ILogStatisticsRepository
    {
        IEnumerable<LogStatisticsEntity> GetReportData(LogStatisticsCriteria criteria);

        IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria);
    }
}
