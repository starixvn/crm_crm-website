// --------------------------------------------------------------------------
// <copyright file="CustomerReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System.Collections.Generic;
    using Fanex.Common;
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
                var data = objectDB.Query<CustomerInfo>(criteria.GetSpParams());
                var outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return data;
            }
        }
    }
}