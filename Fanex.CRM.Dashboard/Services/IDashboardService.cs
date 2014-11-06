// --------------------------------------------------------------------------
// <copyright file="IDashboardService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface IDashboardService
    {
        List<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria);

        CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        List<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria);

        List<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria);

        Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria);
    }
}
