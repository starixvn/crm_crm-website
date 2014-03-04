// --------------------------------------------------------------------------
// <copyright file="ICustomerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;

    public interface ICustomerReportRepository
    {
        List<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria);
    }
}
