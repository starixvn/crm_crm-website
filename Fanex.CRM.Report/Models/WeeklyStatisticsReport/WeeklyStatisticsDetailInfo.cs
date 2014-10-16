// --------------------------------------------------------------------------
// <copyright file="WeeklyStatisticsDetailInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.WeeklyStatisticsReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Weekly Statistics Detail Information.
    /// </summary>
    [Serializable]
    public class WeeklyStatisticsDetailInfo
    {
        public DateTime StartDate { get; set; }

        public string StartDateString { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public TimeInfo Duration { get; set; }

        public string Username { get; set; }

        public SettingInfo SiteInfo { get; set; }

        public SettingInfo CommunicationTool { get; set; }

        public string MSN { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public SettingInfo Extension { get; set; }

        public SettingInfo Product { get; set; }

        public CountryInfo Country { get; set; }

        public SettingInfo MainCategory { get; set; }

        public SettingInfo SubCategory { get; set; }

        public string Remark { get; set; }

        public string Solution { get; set; }

        public string StatusName { get; set; }

        public SettingInfo Response { get; set; }

        public string Handler { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreationTimeString { get; set; }

        public DateTime ResolutionTime { get; set; }

        public string ResolutionTimeString { get; set; }
    }
}
