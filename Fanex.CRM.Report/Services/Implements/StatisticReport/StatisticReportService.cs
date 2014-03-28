// --------------------------------------------------------------------------
// <copyright file="StatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Statistic Report Service.
    /// </summary>
    public class StatisticReportService : IStatisticReportService
    {
        private ISettingService _serviceSetting;

        public StatisticReportService()
            : this(null)
        {
        }

        public StatisticReportService(ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all period type.
        /// </summary>
        /// <returns>All period type.</returns>
        public List<SettingInfo> GetAllPeriodType()
        {
            try
            {
                var allPeriodType = new List<SettingInfo>();
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });

                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });

                return allPeriodType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            try
            {
                var allMainCategory = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllMainCategory();
                var allSubSettingOption = new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                };

                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                       allSubSettingOption
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
                    rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                    allMainCategory.Add(rawMainCategory);
                }

                return allMainCategory;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }
    }
}
