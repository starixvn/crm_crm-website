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

        public DateTime StartDate { get; set; }

        public string StartDateString { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public TimeInfo Duration { get; set; }

        public string Username { get; set; }

        public SettingInfo SiteInfo { get; set; }

        public SettingInfo CommunicationTool { get; set; }

        public string Role { get; set; }

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

                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC1Name, Phone = PIC1 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC2Name, Phone = PIC2 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC3Name, Phone = PIC3 });
                customer.PersonalInfoContacts.Add(new PersonalInfoContact { Name = PIC4Name, Phone = PIC4 });

                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = PhoneAreaCode, CountryCode = PhoneCountryCode, PhoneNumber = Phone });
                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = Phone2AreaCode, CountryCode = Phone2CountryCode, PhoneNumber = Phone2 });
                customer.PhoneInfo.Add(new PhoneInfo { AreaCode = Phone3AreaCode, CountryCode = Phone3CountryCode, PhoneNumber = Phone3 });
                return FieldHelper.GenerateContactInfo(customer);
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
