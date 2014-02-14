// --------------------------------------------------------------------------
// <copyright file="IActionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// IActionLogService interface.
    /// </summary>
    public interface IActionLogService
    {
        IList<ActivityLog> GetActivityLogs(ICriteria criteria);

        IList<SettingLog> GetSettingLogs(ICriteria criteria);
    }
}
