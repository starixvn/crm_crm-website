// --------------------------------------------------------------------------
// <copyright file="BaseCustomerInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using Fanex.CRM.Common.Resources;
    using Fanex.CRM.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base Customer Information.
    /// </summary>
    [Serializable]
    public class BaseCustomer : BaseModel
    {
        public BaseCustomer()
        {
            Title = new Setting();
            SiteSetting = new Site();
            Country = new BaseModel();
            Province = new BaseModel();
            Languages = new List<BaseModel>();
            PhoneInfo = new List<Phone>();
            PersonalInfoContacts = new List<PersonalInfoContact>();
            BettingCode = new List<string>();
            UplineInfo = new List<BaseCustomer>();
        }

        #region Customer Info

        public IEnumerable<BaseCustomer> UplineInfo { get; set; }

        public Setting Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string BirthDayString { get; set; }

        public Site SiteSetting { get; set; }

        public BaseModel Passport { get; set; }

        public bool HeadLine { get; set; }

        public bool IsHeadline { get; set; }

        public bool IsVIP { get; set; }

        public bool IsActive { get; set; }

        public bool IsClosed { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsIgnored { get; set; }

        public string Status
        {
            get
            {
                return StatusName();
            }
        }

        public BaseModel LicenseeType { get; set; }

        public string Checker { get; set; }

        public string IntroducedBy { get; set; }

        #endregion

        #region Communication

        public List<Phone> PhoneInfo { get; set; }

        public Fax FaxInfo { get; set; }

        public Phone MobilePhone { get; set; }

        public List<PersonalInfoContact> PersonalInfoContacts { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string WeChat { get; set; }

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string Yahoo { get; set; }

        public string QQ { get; set; }

        public string BBM { get; set; }

        #endregion

        public BaseModel Country { get; set; }

        public BaseModel Province { get; set; }

        public List<BaseModel> Languages { get; set; }

        public Bank BankInfo { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Remark { get; set; }

        public string Remark2 { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserInfo Creator { get; set; }

        public BaseModel Role { get; set; }

        public int OriginalDangerLevel { get; set; }

        public int ABTDangerLevel { get; set; }

        public int OtherSportsDangerLevel { get; set; }

        public List<string> BettingCode { get; set; }

        public Address AddressInfo { get; set; }

        public string CustStatus { get; set; }

        public bool Locked { get; set; }

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <returns>The name of the status.</returns>
        private string StatusName()
        {
            return IsActive ? Labels.Active :
                IsClosed ? Labels.Closed :
                IsSuspended ? Labels.Suspended :
                string.Empty;
        }
    }
}
