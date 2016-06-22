// --------------------------------------------------------------------------
// <copyright file="IRedemptionLogService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Services.RedemptionLog
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;

    public interface IRedemptionLogService
    {
        CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        LogInfo GetLogById(GetLogByIdCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);

        Result InsertLog(InsertLogCriteria criteria);

        dynamic GetAllSettingsInsert();

        dynamic GetAllSettingsEdit();
    }
}
