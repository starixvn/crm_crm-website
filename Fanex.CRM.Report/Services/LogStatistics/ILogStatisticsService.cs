namespace Fanex.CRM.Report.Services.LogStatistics
{
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Models.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface ILogStatisticsService
    {
        LogStatisticsViewModel GetReportData(LogStatisticsCriteria criteria);

        StatisticsDetailReportByDateModel GetReportDetail(LogStatisticsReportDetailCriteria criteria);
    }
}
