namespace Fanex.CRM.Report.Repositories.StatisticReport
{
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface IStatisticReportRepository
    {
        IList<StatisticReportEntity> GetStatisticReport(ICriteria criteria);
    }
}
