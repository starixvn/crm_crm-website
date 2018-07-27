namespace Fanex.CRM.Report.Repositories.StatisticByDateReport
{
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using System.Collections.Generic;

    public interface IStatisticByDateRepository
    {
        StatisticsInfoByDateRepositoryResultModel GetMainReport(GetMainReportCriteria criteria);

        IList<StatisticsDetailbyDateModel> GetDetailReport(GetDetailReportCriteria criteria);
    }
}
