// --------------------------------------------------------------------------
// <copyright file="ICommunicationLogRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICommunicationLogRepository
    {
        IEnumerable<BaseCommunicationLog> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);

        Result InsertLog(InsertLogCriteria criteria);

        BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria);

        CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria);
    }
}
