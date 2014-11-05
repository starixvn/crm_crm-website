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

        public string Product { get; set; }

        public string Contact
        {
            get
            {
                return string.Format(
                    "{0}: {1}<br/>{2}: {3}<br/>{4}: {5}<br/>{6}: {7}<br/>{8}: {9}",
                    Labels.MSN,
                    string.IsNullOrEmpty(MSN) ? "-" : MSN,
                    Labels.Skype,
                    string.IsNullOrEmpty(Skype) ? "-" : Skype,
                    Labels.Email,
                    string.IsNullOrEmpty(Email) ? "-" : Email,
                    Labels.Phone,
                    string.IsNullOrEmpty(Phone) ? "-" : Phone,
                    Labels.Mobile,
                    string.IsNullOrEmpty(MobilePhone) ? "-" : MobilePhone);
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
