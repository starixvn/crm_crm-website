namespace Fanex.CRM.ActionLog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;

    public class ActionLogService : IActionLogService
    {
        private readonly IBaseService baseService;

        public ActionLogService() : this(null)
        {
        }

        public ActionLogService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public PagedList<ActionLog> GetActivityLogs(GetActivityLogCriteria criteria)
        {
            // Exclude rows whose OldValues = NewValues = NULL/Empty.
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            var activityLogs = baseService.GetData<ActionLog>(criteria)
                .Where(x => !string.Equals(x.OldValues, x.NewValues, StringComparison.OrdinalIgnoreCase));

            var paginationActivityLogs = new PagedList<ActionLog>(activityLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationActivityLogs)
            {
                FilterOldNewResults(log);
            }

            return paginationActivityLogs;
        }

        public PagedList<SettingLog> GetSettingLogs(GetSettingLogCriteria criteria)
        {
            // Exclude rows whose OldValues = NewValues = NULL/Empty.
            criteria.FromDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ToDate.GetEndOfDate());
            var settingLogs = baseService.GetData<SettingLog>(criteria)
                .Where(x => !string.Equals(x.OldValues, x.NewValues, StringComparison.OrdinalIgnoreCase));

            var paginationSettingLogs = new PagedList<SettingLog>(settingLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationSettingLogs)
            {
                FilterOldNewResults(log);
            }

            return paginationSettingLogs;
        }

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

        private static string GetSettingValue(string key, string value, ref string fieldName)
        {
            string keyUpper = key.ToUpperInvariant();

            // Convert bit value (1/0) to Yes/No.
            if (AppSetting.ActionLogBoolValue.TryGetValue(keyUpper, out string[] boolValues))
            {
                if (int.TryParse(value, out int index) && index < 2)
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

        private static string GetSettingValue(string key, string settingType)
        {
            if (string.Equals(key, "0", StringComparison.OrdinalIgnoreCase))
            {
                return "N/A";
            }

            if (!CacheManager.AllSettings.TryGetValue(key + settingType.ToUpperInvariant(), out string settingValue))
            {
                settingValue = key;
            }

            return settingValue;
        }

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