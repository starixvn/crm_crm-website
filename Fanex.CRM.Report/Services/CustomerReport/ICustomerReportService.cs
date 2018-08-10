namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;

    public interface ICustomerReportService
    {
        CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria);
    }
}
