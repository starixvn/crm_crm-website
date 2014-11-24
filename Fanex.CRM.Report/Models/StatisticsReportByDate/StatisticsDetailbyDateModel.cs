// --------------------------------------------------------------------------
// <copyright file="StatisticsDetailbyDateModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using System;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Helpers;

    /// <summary>
    /// Statistics Detail By Date Information.
    /// </summary>
    [Serializable]
    public class StatisticsDetailbyDateModel
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

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string WEChat { get; set; }

        public string Yahoo { get; set; }

        public string QQ { get; set; }

        public string Bbm { get; set; }

        public string Role { get; set; }

        public string Contact
        {
            get
            {
                return FieldHelper.GenerateContactInfo(Bbm, Email, Line, MSN, MobilePhone, Phone, QQ, Skype, WEChat, WhatsApp, Yahoo);
            }
        }

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
