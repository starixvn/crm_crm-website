// --------------------------------------------------------------------------
// <copyright file="DynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class DynamicReportService : IDynamicReportService
    {
        public List<SettingInfo> GetAllPeriodType()
        {
            var allPeriodType = new List<SettingInfo>();
            try
            {
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Weekly,
                    Name = Labels.Weekly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Yearly,
                    Name = Labels.Yearly
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allPeriodType;
        }

        public List<SettingInfo> GetAllReportType()
        {
            var allReportType = new List<SettingInfo>();
            try
            {
                allReportType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Yearly,
                    Name = Labels.Yearly
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allReportType;
        }
    }
}
