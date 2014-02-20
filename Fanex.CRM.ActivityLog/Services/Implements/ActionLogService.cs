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
        public PagedList<ActivityLog> GetActivityLogs(GetActivityLogCriteria criteria)
        {
            // Exclude rows whose OldValues = NewValues = NULL/Empty.
            var activityLogs = _actionLogRepository.GetActivityLogs(criteria)
                .Where(l => !string.IsNullOrWhiteSpace(l.OldValues) && !string.IsNullOrWhiteSpace(l.NewValues));
            
            PagedList<ActivityLog> paginationActivityLogs = new PagedList<ActivityLog>(activityLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationActivityLogs)
            {
                GetNewAndOldValues(log);
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
                .Where(l => !string.IsNullOrWhiteSpace(l.OldValues) && !string.IsNullOrWhiteSpace(l.NewValues));

            PagedList<SettingLog> paginationSettingLogs = new PagedList<SettingLog>(settingLogs, criteria.PageIndex, criteria.ItemsPerPage);
            foreach (var log in paginationSettingLogs)
            {
                GetNewAndOldValues(log);
            }

            return paginationSettingLogs;
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
            string oldString = null, newString = null;
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

            int oldCount = oldValuesList == null ? 0 : oldValuesList.Count;
            int newCount = newValuesList == null ? 0 : newValuesList.Count;
            if (oldCount == 0)
            {
                for (int i = 0; i < newCount; i++)
                {
                    newString = newValuesList[i].Value;
                    if (!string.IsNullOrWhiteSpace(newString))
                    {
                        log.FieldNames.Add(newValuesList[i].Name);
                        log.NewValuesList.Add(newString);
                    }
                }
            }
            else if (oldCount == newCount)
            {
                for (int i = 0; i < newCount; i++)
                {
                    oldString = oldValuesList[i].Value;
                    newString = newValuesList[i].Value;
                    if (!string.Equals(oldString, newString, StringComparison.OrdinalIgnoreCase))
                    {
                        log.FieldNames.Add(newValuesList[i].Name);
                        log.OldValuesList.Add(oldString);
                        log.NewValuesList.Add(newString);
                    }
                }
            }

            log.OldValues = log.NewValues = null;
        }
    }
}
