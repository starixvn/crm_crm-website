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
    using System.Collections.Generic;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ActionLogRepository class.
    /// </summary>
    public class ActionLogRepository : IActionLogRepository
    {
        #region Activity Log

        public IEnumerable<ActivityLog> GetActivityLogs(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<ActivityLog>(criteria.GetSpParams());
            }
        }

        #endregion

        #region Setting Log

        public IEnumerable<SettingLog> GetSettingLogs(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<SettingLog>(criteria.GetSpParams());
            }
        }

        #endregion
    }
}
