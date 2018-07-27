namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Criteria.DirectCustomerReport;
    using Models.DirectCustomerReport;
    using System.Linq;

    public class DirectCustomerReportService : IDirectCustomerReportService
    {
        private readonly IDirectCustomerReportRepository _repository;

        public DirectCustomerReportService()
            : this(null)
        {
        }

        public DirectCustomerReportService(IDirectCustomerReportRepository repository)
        {
            _repository = repository ?? new DirectCustomerReportRepository();
        }

        public DirectCustomerReportInfo GetReport(DirectCustomerReportCriteria criteria)
        {
            return new DirectCustomerReportInfo
            {
                CustomerList = _repository.GetReport(criteria).ToList(),
                TotalCustomer = criteria.TotalRows
            };
        }
    }
}