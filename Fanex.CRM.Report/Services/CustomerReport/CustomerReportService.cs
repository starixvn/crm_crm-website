// --------------------------------------------------------------------------
// <copyright file="CustomerReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System.Linq;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;

    /// <summary>
    /// Customer Report Service.
    /// </summary>
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