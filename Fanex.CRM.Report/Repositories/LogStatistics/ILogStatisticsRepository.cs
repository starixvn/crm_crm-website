namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface ILogStatisticsRepository
    {
        IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria);
    }
}