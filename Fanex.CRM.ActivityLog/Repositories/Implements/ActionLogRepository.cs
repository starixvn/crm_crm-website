// --------------------------------------------------------------------------
// <copyright file="ActionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ActionLogRepository class.
    /// </summary>
    public class ActionLogRepository : IActionLogRepository
    {
        #region IActionLogRepository Members

        public IList<ActivityLog> GetActivityLogs(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IList<SettingLog> GetSettingLogs(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
