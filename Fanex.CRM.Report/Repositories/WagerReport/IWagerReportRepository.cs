namespace Fanex.CRM.Report.Repositories.WagerReport
{
    using Fanex.CRM.Report.Entities.WagerReport;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface IWagerReportRepository
    {
        IEnumerable<WagerRawData> GetWagerReport(ICriteria criteria);
    }
}
