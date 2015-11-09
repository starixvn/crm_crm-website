// --------------------------------------------------------------------------
// <copyright file="ICommunicationLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    public interface ICommunicationLogService
    {
        IEnumerable<Setting> GetAllStatus();

        CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);

        Result InsertLog(InsertLogCriteria criteria);

        CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria);
    }
}
