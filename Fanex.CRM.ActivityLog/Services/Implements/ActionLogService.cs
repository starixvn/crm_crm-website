// --------------------------------------------------------------------------
// <copyright file="ActionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Implements;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.CRM.ActionLog.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Service.Implements;

    /// <summary>
    /// ActionLogService class.
    /// </summary>
    public class ActionLogService : IActionLogService
    {
        private const string ActivityLogKey = "Fanex.CRM.ActionLog.ActivityLog";
        private const string SettingLogKey = "Fanex.CRM.ActionLog.SettingLog";
        
        private readonly IActionLogRepository _actionLogRepository;
        private readonly IDictionary<string, string> _settingHash;
        
        public ActionLogService()
            : this(null)
        {
        }

        public ActionLogService(IActionLogRepository actionLogRepository)
        {
            _actionLogRepository = actionLogRepository ?? new ActionLogRepository();
            _settingHash = CacheManager.GetAllSetting();
        }

        #region Activity Log

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The activity log list with paging.</returns>
        public PagedList<ActivityLog> GetActivityLogs(GetActivityLogCriteria criteria)
        {
            try
            {
                var activityLogs = CacheService.Get<IList<ActivityLog>>(ActivityLogKey);
                if (criteria.PageIndex == 0 || activityLogs == null)
                {
                    // Exclude rows whose OldValues = NewValues = NULL/Empty.
                    activityLogs = _actionLogRepository.GetActivityLogs(criteria)
                        .Where(l => !string.Equals(l.OldValues, l.NewValues, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (activityLogs != null && activityLogs.Count > 0)
                    {
                        CacheService.Set(ActivityLogKey, activityLogs);
                    }
                }

                PagedList<ActivityLog> paginationActivityLogs = new PagedList<ActivityLog>(activityLogs, criteria.PageIndex, criteria.ItemsPerPage);
                foreach (var log in paginationActivityLogs)
                {
                    FilterOldNewResults(log);
                }

                return paginationActivityLogs;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        #endregion

        #region Setting Log

        /// <summary>
        /// Gets the setting logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting log list with paging.</returns>
        public PagedList<SettingLog> GetSettingLogs(GetSettingLogCriteria criteria)
        {
            try
            {
                var settingLogs = CacheService.Get<IList<SettingLog>>(SettingLogKey);
                if (criteria.PageIndex == 0 || settingLogs == null)
                {
                    // Exclude rows whose OldValues = NewValues = NULL/Empty.
                    settingLogs = _actionLogRepository.GetSettingLogs(criteria)
                        .Where(l => !string.Equals(l.OldValues, l.NewValues, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (settingLogs != null && settingLogs.Count > 0)
                    {
                        CacheService.Set(SettingLogKey, settingLogs);
                    }
                }
                
                PagedList<SettingLog> paginationSettingLogs = new PagedList<SettingLog>(settingLogs, criteria.PageIndex, criteria.ItemsPerPage);
                foreach (var log in paginationSettingLogs)
                {
                    FilterOldNewResults(log);
                }

                return paginationSettingLogs;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Filters the old and new results.
        /// </summary>
        /// <param name="log">The log parameter.</param>
        private void FilterOldNewResults(ActionLog log)
        {
            string oldValue = log.OldValues, newValue = log.NewValues;
            if (string.Equals(oldValue, newValue, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            IDictionary<string, string> oldResults = null, newResults = null;
            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                oldResults = ParseXMLString(oldValue);
            }

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                newResults = ParseXMLString(newValue);
            }

            int oldCount = oldResults == null ? 0 : oldResults.Count;
            int newCount = newResults == null ? 0 : newResults.Count;
            if (oldCount == 0)
            {
                foreach (string key in newResults.Keys)
                {
                    log.FieldNames.Add(key);
                    log.NewValuesList.Add(GetNameById(key, newResults[key]));
                }
            }
            else if (oldCount == newCount)
            {
                string oldString, newString;
                foreach (string key in newResults.Keys)
                {
                    oldString = oldResults[key];
                    newString = newResults[key];
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        log.FieldNames.Add(key);
                        log.OldValuesList.Add(GetNameById(key, oldString));
                        log.NewValuesList.Add(GetNameById(key, newString));
                    }
                }
            }
        }

        /// <summary>
        /// Parses the XML string.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The hash result.</returns>
        private IDictionary<string, string> ParseXMLString(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            IDictionary<string, string> results = new Dictionary<string, string>();
            foreach (XmlNode node in document.ChildNodes[0].ChildNodes)
            {
                if (!results.ContainsKey(node.Name))
                {
                    results.Add(node.Name, node.InnerText);
                }
                else
                {
                    results[node.Name] += "<br/>" + node.InnerText;
                }
            }

            return results;
        }

        private string GetNameById(string key, string value)
        {
            string keyLower = key.ToLower();
            if (string.Equals(keyLower, "LanguageID", StringComparison.OrdinalIgnoreCase))
            {
                return GetMultipleSettingValue(keyLower, value);   
            }

            return GetSettingValue(keyLower + value, value);
        }

        private string GetSettingValue(string key, string defaultValue)
        {
            string settingValue;
            if (!_settingHash.TryGetValue(key, out settingValue))
            {
                return defaultValue;
            }

            return settingValue;
        }

        /// <summary>
        /// Gets the multiple setting value.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value from hash.
        /// </returns>
        private string GetMultipleSettingValue(string key, string value)
        {
            var valueList = value.Split(new char[] { ',' });
            
            int keyCount = valueList.Length;
            for (int i = 0; i < keyCount; i++)
            {
                valueList[i] = GetSettingValue(key + valueList[i], valueList[i]);
            }

            return string.Join(",", valueList);
        }
    }
}
