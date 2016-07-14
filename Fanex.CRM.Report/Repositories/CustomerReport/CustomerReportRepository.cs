// --------------------------------------------------------------------------
// <copyright file="CustomerReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.Data;

    /// <summary>
    /// Customer Report Repository.
    /// </summary>
    public class CustomerReportRepository : ICustomerReportRepository
    {
        public IEnumerable<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<CustomerInfo>(criteria.GetSpParams());
            }
        }
    }
}