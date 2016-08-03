// --------------------------------------------------------------------------
// <copyright file="IActionLogService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services
{
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// IActionLogService interface.
    /// </summary>
    public interface IActionLogService
    {
        PagedList<ActionLog> GetActivityLogs(GetActivityLogCriteria criteria);

        PagedList<SettingLog> GetSettingLogs(GetSettingLogCriteria criteria);
    }
}
