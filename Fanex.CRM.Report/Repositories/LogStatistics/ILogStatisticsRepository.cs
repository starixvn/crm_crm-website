// -----------------------------------------------------------------------
// <copyright file="ILogStatisticRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Entities.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface ILogStatisticsRepository
    {
        IEnumerable<LogStatisticsEntity> GetReportData(LogStatisticsCriteria criteria);

        IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria);
    }
}
