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
                    GetNewAndOldValues(log);
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
                    GetNewAndOldValues(log);
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
        /// Parses the XML string.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>Name value list.</returns>
        private IList<NameValue> ParseXMLString(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            IList<NameValue> results = new List<NameValue>();
            foreach (XmlNode node in document.ChildNodes[0].ChildNodes)
            {
                results.Add(new NameValue { Name = node.Name, Value = node.InnerText });
            }

            return results;
        }

        /// <summary>
        /// Gets the new and old values.
        /// </summary>
        /// <param name="log">The log object.</param>
        private void GetNewAndOldValues(ActionLog log)
        {
            IList<NameValue> oldValuesList = null, newValuesList = null;
            string oldValues = log.OldValues,
                newValues = log.NewValues;
            
            if (!string.IsNullOrWhiteSpace(oldValues))
            {
                oldValuesList = ParseXMLString(oldValues);
            }

            if (!string.IsNullOrWhiteSpace(newValues))
            {
                newValuesList = ParseXMLString(newValues);
            }

            if (newValues.IndexOf("Acc_CRM_CustomerRemarkUpd", StringComparison.OrdinalIgnoreCase) >= 0 ||
                oldValues.IndexOf("Acc_CRM_CustomerRemarkUpd", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                GetNewAndOldValues4RemarkLog(log, oldValuesList, newValuesList);
            }
            else
            {
                GetNewAndOldValues(log, oldValuesList, newValuesList);
            }
            
            log.OldValues = log.NewValues = null;
        }

        private void GetNewAndOldValues(ActionLog log, IList<NameValue> oldValues, IList<NameValue> newValues)
        {
            string oldString = null, newString = null;
            int oldCount = oldValues == null ? 0 : oldValues.Count;
            int newCount = newValues == null ? 0 : newValues.Count;
            if (oldCount == 0)
            {
                for (int i = 0; i < newCount; i++)
                {
                    newString = newValues[i].Value;
                    if (!string.IsNullOrWhiteSpace(newString))
                    {
                        log.FieldNames.Add(newValues[i].Name);
                        log.NewValuesList.Add(newString);
                    }
                }
            }
            else if (oldCount == newCount)
            {
                for (int i = 0; i < newCount; i++)
                {
                    oldString = oldValues[i].Value;
                    newString = newValues[i].Value;
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        log.FieldNames.Add(newValues[i].Name);
                        log.OldValuesList.Add(oldString);
                        log.NewValuesList.Add(newString);
                    }
                }
            }
        }

        #region Remark Log

        /// <summary>
        /// Gets the new and old values4 remark log.
        /// </summary>
        /// <param name="log">The log parameter.</param>
        /// <param name="oldValues">The old values.</param>
        /// <param name="newValues">The new values.</param>
        private void GetNewAndOldValues4RemarkLog(ActionLog log, IList<NameValue> oldValues, IList<NameValue> newValues)
        {
            IList<string> oldRemarks = null, oldSpecialRemarks = null;
            IList<string> newRemarks = null, newSpecialRemarks = null;
            if (oldValues != null)
            {
                GetRemarkLogList(oldValues, out oldRemarks, out oldSpecialRemarks);
            }

            if (newValues != null)
            {
                GetRemarkLogList(newValues, out newRemarks, out newSpecialRemarks);
            }

            CompareNewAndOldRemarks(log, oldRemarks, newRemarks, false);
            CompareNewAndOldRemarks(log, oldSpecialRemarks, newSpecialRemarks, true);
        }

        /// <summary>
        /// Gets the remark log list.
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="specialRemarks">The special remarks.</param>
        private void GetRemarkLogList(IList<NameValue> sources, out IList<string> remarks, out IList<string> specialRemarks)
        {
            remarks = new List<string>();
            specialRemarks = new List<string>();
            foreach (NameValue item in sources)
            {
                if (string.Equals(item.Name, "remark", StringComparison.OrdinalIgnoreCase))
                {
                    remarks.Add(item.Value);
                }
                else
                {
                    specialRemarks.Add(item.Value);
                }
            }
        }

        /// <summary>
        /// Compares the new and old remarks.
        /// </summary>
        /// <param name="log">The log parameter.</param>
        /// <param name="oldList">The old list.</param>
        /// <param name="newList">The new list.</param>
        /// <param name="isSpecialRemark">If set to <c>true</c> [is special remark].</param>
        private void CompareNewAndOldRemarks(ActionLog log, IList<string> oldList, IList<string> newList, bool isSpecialRemark = true)
        {
            string oldString, newString;
            int oldCount = oldList == null ? 0 : oldList.Count;
            int newCount = newList == null ? 0 : newList.Count;
            if (oldCount == 0)
            {
                for (int i = 0; i < newCount; i++)
                {
                    newString = newList[i];
                    if (!string.IsNullOrWhiteSpace(newString))
                    {
                        log.FieldNames.Add(isSpecialRemark ? "SpecialRemark" : "Remark");
                        log.NewValuesList.Add(newString);
                    }
                }
            }
            else
            {
                //// balance between oldList and newList
                if (newCount > oldCount)
                {
                    for (int i = oldCount; i < newCount; i++)
                    {
                        oldList.Add(string.Empty);
                    }
                }
                else if (oldCount > newCount)
                {
                    for (int i = newCount; i < oldCount; i++)
                    {
                        newList.Add(string.Empty);
                    }
                }

                newCount = newList.Count;
                for (int i = 0; i < newCount; i++)
                {
                    oldString = oldList[i];
                    newString = newList[i];
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        log.FieldNames.Add(isSpecialRemark ? "SpecialRemark" : "Remark");
                        log.OldValuesList.Add(oldString);
                        log.NewValuesList.Add(newString);
                    }
                }
            }
        }

        #endregion
    }
}
