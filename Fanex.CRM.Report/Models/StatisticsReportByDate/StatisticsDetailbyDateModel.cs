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
    using System.Web;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Resources;

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
                return string.Format(
                    "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                    string.IsNullOrEmpty(Bbm) ? string.Empty : string.Format("{0}: {1}{2}", Labels.BBM, HttpContext.Current.Server.HtmlEncode(Bbm), "<br/>"),
                    string.IsNullOrEmpty(Email) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Email, HttpContext.Current.Server.HtmlEncode(Email), "<br/>"),
                    string.IsNullOrEmpty(Line) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Line, HttpContext.Current.Server.HtmlEncode(Line), "<br/>"),
                    string.IsNullOrEmpty(MSN) ? string.Empty : string.Format("{0}: {1}{2}", Labels.MSN, HttpContext.Current.Server.HtmlEncode(MSN), "<br/>"),
                    string.IsNullOrEmpty(MobilePhone) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Mobile, HttpContext.Current.Server.HtmlEncode(MobilePhone), "<br/>"),
                    string.IsNullOrEmpty(Phone) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Phone, HttpContext.Current.Server.HtmlEncode(Phone), "<br/>"),
                    string.IsNullOrEmpty(QQ) ? string.Empty : string.Format("{0}: {1}{2}", Labels.QQ, HttpContext.Current.Server.HtmlEncode(QQ), "<br/>"),
                    string.IsNullOrEmpty(Skype) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Skype, HttpContext.Current.Server.HtmlEncode(Skype), "<br/>"),
                    string.IsNullOrEmpty(WEChat) ? string.Empty : string.Format("{0}: {1}{2}", Labels.WeChat, HttpContext.Current.Server.HtmlEncode(WEChat), "<br/>"),
                    string.IsNullOrEmpty(WhatsApp) ? string.Empty : string.Format("{0}: {1}{2}", Labels.WhatsApp, HttpContext.Current.Server.HtmlEncode(WhatsApp), "<br/>"),
                    string.IsNullOrEmpty(Yahoo) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Yahoo, HttpContext.Current.Server.HtmlEncode(Yahoo), string.Empty));
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
