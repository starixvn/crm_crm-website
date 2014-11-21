// --------------------------------------------------------------------------
// <copyright file="CasinoDetailEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.CasinoReport
{
    using System;
    using System.Web;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Report.Resources;

    /// <summary>
    /// Casino Daily Detail Entity.
    /// </summary>
    [Serializable]
    public class CasinoDetailEntity
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public string MSN { get; set; }

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string WEChat { get; set; }

        public string Yahoo { get; set; }

        public string QQ { get; set; }

        public string Bbm { get; set; }

        public string Product { get; set; }

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

        public string CountryName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateString
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClientDateTime(CreateDate).ToLongFormatedDateTime();
            }
        }

        public string CreatorName { get; set; }

        public DateTime FeedbackDate { get; set; }

        public string FeedbackDateString
        {
            get
            {
                return FeedbackDateClient.ToShortFormatedDate();
            }
        }

        public string FeedbackTimeString
        {
            get
            {
                return FeedbackDateClient.ToString("hh:mm tt");
            }
        }

        public DateTime ResolutionDate { get; set; }

        public string ResolutionTimeString 
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClientDateTime(ResolutionDate).ToString("hh:mm tt");
            }
        }

        public string Communication { get; set; }

        public string Station { get; set; }

        public string MainCategory { get; set; }

        public string SubCategory { get; set; }

        public string SiteName { get; set; }

        public string Response { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string Role { get; set; }

        public string DescriptionWithoutSpecialHtmlTag
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                {
                    return null;    
                }

                return HtmlHelper.RemoveSpecialHtmlTag(Description);
            }
        }

        public string Solution { get; set; }

        public string SolutionWithoutSpecialHtmlTag
        {
            get
            {
                if (string.IsNullOrEmpty(Solution))
                {
                    return null;
                }

                return HtmlHelper.RemoveSpecialHtmlTag(Solution);
            }
        }

        public string Address
        {
            get 
            {
                return string.Format(
                    "MSN: {0}<br/> Skype: {1}<br/> Email: {2}<br/> Phone: {3}<br/> Mobile: {4}",
                    MSN, 
                    Skype, 
                    Email, 
                    Phone,
                    MobilePhone);
            }
        }

        public string Duration
        {
            get
            {
                if (ResolutionDate == DateTime.MinValue)
                {
                    return string.Empty;
                }

                var totalDays = ResolutionDate - FeedbackDate;
                return string.Format(
                    "{0}:{1}:{2}",
                    totalDays.Days.ToString("00"),
                    totalDays.Hours.ToString("00"),
                    totalDays.Minutes.ToString("00"));
            }
        }

        private DateTime FeedbackDateClient
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClientDateTime(FeedbackDate);
            }
        }
    }
}
