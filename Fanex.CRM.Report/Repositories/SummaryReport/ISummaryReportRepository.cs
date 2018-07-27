namespace Fanex.CRM.Report.Repositories.SummaryReport
{
    using Fanex.CRM.Report.Entities.SummaryReport;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface ISummaryReportRepository
    {
        IEnumerable<SummaryRawData> GetSummaryReport(ICriteria criteria);
    }
}
