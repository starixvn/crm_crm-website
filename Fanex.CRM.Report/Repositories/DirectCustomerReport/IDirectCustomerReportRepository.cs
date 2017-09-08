namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Criteria.DirectCustomerReport;
    using Models.DirectCustomerReport;
    using System.Collections.Generic;

    public interface IDirectCustomerReportRepository
    {
        IEnumerable<DirectCustomerInfo> GetReport(DirectCustomerReportCriteria criteria);
    }
}
