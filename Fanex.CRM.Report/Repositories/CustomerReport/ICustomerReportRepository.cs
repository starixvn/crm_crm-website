namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using System.Collections.Generic;

    public interface ICustomerReportRepository
    {
        IEnumerable<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria);
    }
}
