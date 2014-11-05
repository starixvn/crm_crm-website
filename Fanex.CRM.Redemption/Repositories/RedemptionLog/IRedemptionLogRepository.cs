// --------------------------------------------------------------------------
// <copyright file="IRedemptionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Repositories.RedemptionLog
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;

    public interface IRedemptionLogRepository
    {
        List<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);

        Result InsertLog(InsertLogCriteria criteria);

        LogInfo GetLogById(GetLogByIdCriteria criteria);
    }
}
