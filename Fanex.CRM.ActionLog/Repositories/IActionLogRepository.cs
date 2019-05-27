// --------------------------------------------------------------------------
// <copyright file="IActionLogRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Repositories
{
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using System.Collections.Generic;

    /// <summary>
    /// IActionLogRepository interface.
    /// </summary>
    public interface IActionLogRepository
    {
        IEnumerable<ActionLog> GetActivityLogs(GetActionLogCriteria criteria);

        IEnumerable<SettingLog> GetSettingLogs(GetSettingLogCriteria criteria);
    }
}
