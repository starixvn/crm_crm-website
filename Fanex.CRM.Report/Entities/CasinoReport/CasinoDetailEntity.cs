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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Report.Helpers;

    /// <summary>
    /// Casino Daily Detail Entity.
    /// </summary>
    [Serializable]
    public class CasinoDetailEntity
    {
        #region Communication

        public string PhoneCountryCode { get; set; }

        public string PhoneAreaCode { get; set; }

        public string Phone { get; set; }

        public string Phone2CountryCode { get; set; }

        public string Phone2AreaCode { get; set; }

        public string Phone2 { get; set; }

        public string Phone3CountryCode { get; set; }

        public string Phone3AreaCode { get; set; }

        public string Phone3 { get; set; }

        public string PIC1Name { get; set; }

        public string PIC1 { get; set; }

        public string PIC2Name { get; set; }

        public string PIC2 { get; set; }

        public string PIC3Name { get; set; }

        public string PIC3 { get; set; }

        public string PIC4Name { get; set; }

        public string PIC4 { get; set; }

        public string FaxCountryCode { get; set; }

        public string FaxAreaCode { get; set; }

        public string Fax { get; set; }

        public string MobilePhoneCountryCode { get; set; }

        public string MobilePhoneAreaCode { get; set; }

        public string MobilePhone { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string WEChat { get; set; }

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string Yahoo { get; set; }

        public string QQ { get; set; }

        public string Bbm { get; set; }

        #endregion

        public string UserName { get; set; }

        public string Product { get; set; }

        public string Contact
        {
            get
            {
                BaseCustomerInfo customer = new BaseCustomerInfo
                {
                    FaxInfo = new FaxInfo { AreaCode = FaxAreaCode, CountryCode = FaxCountryCode, FaxNumber = Fax },
                    MobilePhone = new PhoneInfo { AreaCode = MobilePhoneAreaCode, CountryCode = MobilePhoneCountryCode, PhoneNumber = MobilePhone },
                    Skype = Skype,
                    Email = Email,
                    MSN = MSN,
                    WeChat = WEChat,
                    WhatsApp = WhatsApp,
                    Line = Line,
                    Yahoo = Yahoo,
                    QQ = QQ,
                    BBM = Bbm
                };

                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = PhoneAreaCode, CountryCode = PhoneCountryCode, PhoneNumber = Phone });
                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = Phone2AreaCode, CountryCode = Phone2CountryCode, PhoneNumber = Phone2 });
                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = Phone3AreaCode, CountryCode = Phone3CountryCode, PhoneNumber = Phone3 });

                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC1Name, Phone = PIC1 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC2Name, Phone = PIC2 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC3Name, Phone = PIC3 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC4Name, Phone = PIC4 });
                return FieldHelper.GenerateContactInfo(customer);
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
