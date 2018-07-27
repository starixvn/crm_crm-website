namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using System.Linq;

    public class CustomerReportService : ICustomerReportService
    {
        private readonly ICustomerReportRepository _customerReportRepository;

        public CustomerReportService()
            : this(null)
        {
        }

        public CustomerReportService(ICustomerReportRepository customerReportRepository)
        {
            _customerReportRepository = customerReportRepository ?? new CustomerReportRepository();
        }

        public CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            return new CustomerReportInfo
            {
                CustomerList = _customerReportRepository.GetCustomerReport(criteria).ToList(),
                TotalCustomer = criteria.TotalRows
            };
        }
    }
}