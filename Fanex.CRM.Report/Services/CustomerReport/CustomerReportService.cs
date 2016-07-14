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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

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

        public CustomerReportService(
            ICustomerReportRepository customerReportRepository)
        {
            _customerReportRepository = customerReportRepository ?? new CustomerReportRepository();
        }

        public CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = new CustomerReportInfo();
            customerReport.CustomerList = _customerReportRepository.GetCustomerReport(criteria).ToList();
            customerReport.TotalCustomer = customerReport.CustomerList.Count;

            return new CustomerReportInfo
            {
                CustomerList = criteria.IsGetFullReport ?
                    customerReport.CustomerList :
                    customerReport.CustomerList.Skip(criteria.PageSize * criteria.PageIndex)
                       .Take(criteria.PageSize)
                       .ToList(),
                TotalCustomer = customerReport.TotalCustomer
            };
        }
    }
}