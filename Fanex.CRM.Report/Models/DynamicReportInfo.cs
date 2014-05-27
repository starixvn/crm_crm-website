// --------------------------------------------------------------------------
// <copyright file="DynamicReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Dynamic Report Information.
    /// </summary>
    [Serializable]
    public class DynamicReportInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int ReportTypeId { get; set; }

        public int CustomerId { get; set; }

        public int RunningNumber { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public SettingInfo MainCategory { get; set; }

        public SettingInfo SubCategory { get; set; }

        public string StatusName { get; set; }

        public string Subject { get; set; }

        public TimeInfo Duration { get; set; }

        public string Handler { get; set; }

        public string Reviewer { get; set; }

        public string Username { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public SettingInfo Extension { get; set; }

        public CountryInfo Country { get; set; }

        public SettingInfo CommunicationTool { get; set; }

        public int LanguageId { get; set; }

        public SettingInfo Product { get; set; }

        public SettingInfo BetType { get; set; }

        public SettingInfo Status { get; set; }

        public SettingInfo SiteInfo { get; set; }

        public SettingInfo RoleInfo { get; set; }

        public SettingInfo Response { get; set; }

        public string RelatedCustomers { get; set; }

        public string RelatedCustomerIds { get; set; }

        public string Remark { get; set; }

        public string Solution { get; set; }

        public string Review { get; set; }

        public bool IsReviewed { get; set; }

        public List<DynamicReportInfo> ChildReports { get; set; }

        public int TotalChildReports { get; set; }

        public int StartChildIndex { get; set; }

        public DynamicReportInfo ChartInfo { get; set; }
    }
}
