// --------------------------------------------------------------------------
// <copyright file="ICustomerReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;

    public interface ICustomerReportService
    {
        CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria);
    }
}
