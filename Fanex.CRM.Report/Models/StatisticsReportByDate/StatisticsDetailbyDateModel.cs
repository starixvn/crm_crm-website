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
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Helpers;

    /// <summary>
    /// Statistics Detail By Date Information.
    /// </summary>
    [Serializable]
    public class StatisticsDetailbyDateModel
    {
        public StatisticsDetailbyDateModel()
        {
            Phones = new List<PhoneInfo>();
            PICs = new List<PersonalInfoContact>();
        }

        #region Communication

        public List<PhoneInfo> Phones { get; set; }

        public List<PersonalInfoContact> PICs { get; set; }

        public FaxInfo Fax { get; set; }

        public PhoneInfo MobilePhone { get; set; }

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
                    PhoneInfo = Phones,
                    PersonalInfoContacts = PICs,
                    FaxInfo = Fax,
                    MobilePhone = MobilePhone,
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
