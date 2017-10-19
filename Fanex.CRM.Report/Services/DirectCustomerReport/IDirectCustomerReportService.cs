namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Criteria.DirectCustomerReport;
    using Models.DirectCustomerReport;

    public interface IDirectCustomerReportService
    {
        DirectCustomerReportInfo GetReport(DirectCustomerReportCriteria criteria);
    }
}
