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
    using Fanex.Common;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
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
        public IList<ActivityLog> GetActivityLogs(GetActionLogCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader == null)
                    {
                        return null;
                    }

                    IList<ActivityLog> activityLogs = new List<ActivityLog>();
                    while (reader.Read())
                    {
                        activityLogs.Add(new ActivityLog
                        {
                            // No need to convert ClientDatetime, we'll do it in Model.
                            LogDate = Converter.ToDateTime(reader["LogDate"]),
                            Username = Converter.ToString2(reader["Username"]),
                            OldValues = Converter.ToString2(reader["OldValues"]),
                            NewValues = Converter.ToString2(reader["NewValues"]),
                            ActionName = Converter.ToString2(reader["ActionName"])
                        });
                    }

                    return activityLogs;
                }
            }
        }

        #endregion

        #region Setting Log

        /// <summary>
        /// Gets the setting logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting log list.</returns>
        public IList<SettingLog> GetSettingLogs(GetActionLogCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader == null)
                    {
                        return null;
                    }

                    IList<SettingLog> settingLogs = new List<SettingLog>();
                    while (reader.Read())
                    {
                        settingLogs.Add(new SettingLog
                        {
                            // No need to convert ClientDatetime, we'll do it in Model.
                            LogDate = Converter.ToDateTime(reader["LogDate"]),
                            Username = Converter.ToString2(reader["Username"]),
                            OldValues = Converter.ToString2(reader["OldValues"]),
                            NewValues = Converter.ToString2(reader["NewValues"]),
                            ActionName = Converter.ToString2(reader["ActionName"]),
                            Entity = Converter.ToString2(reader["Entity"]),
                            Id = Converter.ToInt(reader["ID"])
                        });
                    }

                    return settingLogs;
                }
            }
        }

        #endregion
    }
}
