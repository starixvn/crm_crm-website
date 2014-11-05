// --------------------------------------------------------------------------
// <copyright file="IActionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;

    /// <summary>
    /// IActionLogRepository interface.
    /// </summary>
    public interface IActionLogRepository
    {
        IList<ActivityLog> GetActivityLogs(GetActionLogCriteria criteria);

        IList<SettingLog> GetSettingLogs(GetActionLogCriteria criteria);
    }
}
