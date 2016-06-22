// --------------------------------------------------------------------------
// <copyright file="IDashboardRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface IDashboardRepository
    {
        IEnumerable<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria);

        CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria);

        IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria);

        Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria);
    }
}
