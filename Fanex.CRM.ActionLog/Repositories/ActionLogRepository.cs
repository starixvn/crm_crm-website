// --------------------------------------------------------------------------
// <copyright file="ActionLogRepository.cs" company="">
// Copyright (c) . All rights reserved.
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
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.Data;

    /// <summary>
    /// ActionLogRepository class.
    /// </summary>
    public class ActionLogRepository : IActionLogRepository
    {
        #region Activity Log

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The activity log list.</returns>
        public IEnumerable<ActionLog> GetActivityLogs(GetActionLogCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<ActionLog>(criteria.GetSpParams());
            }
        }

        #endregion

        #region Setting Log

        /// <summary>
        /// Gets the setting logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting log list.</returns>
        public IEnumerable<SettingLog> GetSettingLogs(GetActionLogCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<SettingLog>(criteria.GetSpParams());
            }
        }

        #endregion
    }
}
