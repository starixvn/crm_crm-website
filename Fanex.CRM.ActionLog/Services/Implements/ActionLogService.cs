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
    using System.Web;
    using System.Xml;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Implements;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.CRM.ActionLog.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// ActionLogService class.
    /// </summary>
    public class ActionLogService : IActionLogService
    {
        private const string ActivityLogKey = "Fanex.CRM.ActionLog.ActivityLog";
        private const string SettingLogKey = "Fanex.CRM.ActionLog.SettingLog";

        private readonly IActionLogRepository _actionLogRepository;
        private readonly IDictionary<string, string> _settingHash;
        private readonly IDictionary<string, bool> _actionLogKeyHash;
        private readonly IDictionary<string, string[]> _actionLogBoolValueHash;

        public ActionLogService()
            : this(null)
        {
        }

        public ActionLogService(IActionLogRepository actionLogRepository)
        {
            _actionLogRepository = actionLogRepository ?? new ActionLogRepository();
            _settingHash = CacheManager.GetAllSetting();
            _actionLogKeyHash = WebConfigApplicationSettings.ActionLogKey;
            _actionLogBoolValueHash = WebConfigApplicationSettings.ActionLogBoolValue;
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
                var activityLogs = (IList<ActivityLog>)SessionExtension.GetSession(HttpContext.Current.Session, ActivityLogKey);
                if (criteria.PageIndex == 0 || activityLogs == null)
                {
                    // Exclude rows whose OldValues = NewValues = NULL/Empty.
                    activityLogs = _actionLogRepository.GetActivityLogs(criteria)
                        .Where(l => !string.Equals(l.OldValues, l.NewValues, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (activityLogs != null && activityLogs.Count > 0)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, ActivityLogKey, activityLogs);
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
                var settingLogs = (IList<SettingLog>)SessionExtension.GetSession(HttpContext.Current.Session, SettingLogKey);
                if (criteria.PageIndex == 0 || settingLogs == null)
                {
                    // Exclude rows whose OldValues = NewValues = NULL/Empty.
                    settingLogs = _actionLogRepository.GetSettingLogs(criteria)
                        .Where(l => !string.Equals(l.OldValues, l.NewValues, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (settingLogs != null && settingLogs.Count > 0)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, SettingLogKey, settingLogs);
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

            string fieldName;
            int oldCount = oldResults == null ? 0 : oldResults.Count;
            int newCount = newResults == null ? 0 : newResults.Count;
            if (oldCount == 0)
            {
                foreach (string key in newResults.Keys)
                {
                    fieldName = key;
                    log.NewValuesList.Add(GetSettingValue(key, newResults[key], ref fieldName));
                    log.FieldNames.Add(fieldName);
                }
            }
            else if (oldCount == newCount)
            {
                string oldString, newString;
                foreach (string key in newResults.Keys)
                {
                    oldString = oldResults.ContainsKey(key) ? oldResults[key] : string.Empty;
                    newString = newResults[key];
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        fieldName = key;
                        log.OldValuesList.Add(GetSettingValue(key, oldString, ref fieldName));
                        log.NewValuesList.Add(GetSettingValue(key, newString, ref fieldName));
                        log.FieldNames.Add(fieldName);
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

        /// <summary>
        /// Gets the name by id.
        /// </summary>
        /// <param name="key">The key parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// The value of key.
        /// </returns>
        private string GetSettingValue(string key, string value, ref string fieldName)
        {
            string keyLower = key.ToLowerInvariant();

            // Convert bit value (1/0) to Yes/No.
            string[] boolValues;
            if (_actionLogBoolValueHash.TryGetValue(keyLower, out boolValues))
            {
                int index;
                if (int.TryParse(value, out index) && index < 2)
                {
                    return boolValues[index];
                }
            }

            // Value isn't in Setting hash.
            if (!_actionLogKeyHash.ContainsKey(keyLower))
            {
                return value;
            }

            // LanguageID can be stored multiple values.
            if (string.Equals(keyLower, "LanguageID", StringComparison.OrdinalIgnoreCase))
            {
                fieldName = "Language";
                return GetMultipleSettingValue(value);
            }

            fieldName = key.Replace("ID", string.Empty);
            return GetSettingValue(value);
        }

        private string GetSettingValue(string key)
        {
            string settingValue;
            if (!_settingHash.TryGetValue(key, out settingValue))
            {
                settingValue = key;
            }

            return settingValue;
        }

        /// <summary>
        /// Gets the multiple setting value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value from hash.
        /// </returns>
        private string GetMultipleSettingValue(string value)
        {
            var valueList = value.Split(new char[] { ',' });

            int keyCount = valueList.Length;
            for (int i = 0; i < keyCount; i++)
            {
                valueList[i] = GetSettingValue(valueList[i]);
            }

            return string.Join(", ", valueList);
        }
    }
}
