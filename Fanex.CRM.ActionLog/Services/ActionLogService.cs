// --------------------------------------------------------------------------
// <copyright file="ActionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// ActionLogService class.
    /// </summary>
    public class ActionLogService : IActionLogService
    {
        private readonly IActionLogRepository _actionLogRepository;

        public ActionLogService()
            : this(null)
        {
        }

        public ActionLogService(IActionLogRepository actionLogRepository)
        {
            _actionLogRepository = actionLogRepository ?? new ActionLogRepository();
        }

        #region Activity Log

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The activity log list with paging.</returns>
        public PagedList<ActionLog> GetActivityLogs(GetActivityLogCriteria criteria)
        {
            // Exclude rows whose OldValues = NewValues = NULL/Empty.
            var activityLogs = _actionLogRepository.GetActivityLogs(criteria)
                .Where(x => !string.Equals(x.OldValues, x.NewValues, StringComparison.OrdinalIgnoreCase));

            var paginationActivityLogs = new PagedList<ActionLog>(activityLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationActivityLogs)
            {
                FilterOldNewResults(log);
            }

            return paginationActivityLogs;
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
            // Exclude rows whose OldValues = NewValues = NULL/Empty.
            var settingLogs = _actionLogRepository.GetSettingLogs(criteria)
                .Where(x => !string.Equals(x.OldValues, x.NewValues, StringComparison.OrdinalIgnoreCase));

            var paginationSettingLogs = new PagedList<SettingLog>(settingLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationSettingLogs)
            {
                FilterOldNewResults(log);
            }

            return paginationSettingLogs;
        }

        #endregion

        /// <summary>
        /// Filters the old and new results.
        /// </summary>
        /// <param name="log">The log parameter.</param>
        private static void FilterOldNewResults(ActionLog log)
        {
            string oldValue = log.OldValues, newValue = log.NewValues;
            if (string.Equals(oldValue, newValue, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var oldResults = ParseXMLString(oldValue);
            var newResults = ParseXMLString(newValue);

            if (oldResults.Count == 0)
            {
                foreach (string key in newResults.Keys)
                {
                    var fieldName = key;
                    log.NewValuesList.Add(GetSettingValue(key, newResults[key], ref fieldName));
                    log.FieldNames.Add(fieldName);
                }
            }
            else if (oldResults.Count == newResults.Count)
            {
                foreach (string key in newResults.Keys)
                {
                    var oldString = oldResults.ContainsKey(key) ? oldResults[key] : string.Empty;
                    var newString = newResults[key];
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        var fieldName = key;
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
        private static IDictionary<string, string> ParseXMLString(string xml)
        {
            var results = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(xml))
            {
                return results;
            }

            var document = new XmlDocument();
            document.LoadXml(xml);

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
        private static string GetSettingValue(string key, string value, ref string fieldName)
        {
            string keyUpper = key.ToUpperInvariant();

            // Convert bit value (1/0) to Yes/No.
            string[] boolValues;
            if (AppSetting.ActionLogBoolValue.TryGetValue(keyUpper, out boolValues))
            {
                int index;
                if (int.TryParse(value, out index) && index < 2)
                {
                    return boolValues[index];
                }
            }

            // Value isn't in Setting hash.
            if (!AppSetting.ActionLogKey.ContainsKey(keyUpper))
            {
                return value;
            }

            // LanguageID can be stored multiple values.
            if (string.Equals(keyUpper, "LanguageID", StringComparison.OrdinalIgnoreCase))
            {
                fieldName = "Language";
                return GetMultipleSettingValue(value, fieldName);
            }

            fieldName = key.Replace("ID", string.Empty);
            return GetSettingValue(value, fieldName);
        }

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <param name="key">The key parameter.</param>
        /// <param name="settingType">Type of the setting.</param>
        /// <returns>The value of key.</returns>
        private static string GetSettingValue(string key, string settingType)
        {
            if (string.Equals(key, "0", StringComparison.OrdinalIgnoreCase))
            {
                return "N/A";
            }

            string settingValue;
            if (!CacheManager.AllSettings.TryGetValue(key + settingType.ToUpperInvariant(), out settingValue))
            {
                settingValue = key;
            }

            return settingValue;
        }

        /// <summary>
        /// Gets the multiple setting value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="settingType">Type of the setting.</param>
        /// <returns>
        /// The value from hash.
        /// </returns>
        private static string GetMultipleSettingValue(string value, string settingType)
        {
            var valueList = value.Split(',');

            int keyCount = valueList.Length;
            for (int i = 0; i < keyCount; i++)
            {
                valueList[i] = GetSettingValue(valueList[i], settingType);
            }

            return string.Join(", ", valueList);
        }
    }
}
