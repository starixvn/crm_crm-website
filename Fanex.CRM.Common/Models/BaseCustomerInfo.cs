// --------------------------------------------------------------------------
// <copyright file="BaseCustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Resources;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Base Customer Information.
    /// </summary>
    [Serializable]
    public class BaseCustomerInfo : BaseModel
    {
        public BaseCustomerInfo()
        {
            Title = new SettingInfo();
            SiteSetting = new BaseModel();
            Country = new BaseModel();
            Languages = new List<BaseModel>();
            PhoneInfo = new List<PhoneInfo>();
            PersonalInfoContact = new List<string>();
            BettingCode = new List<string>();
            UplineInfo = new List<BaseCustomerInfo>();
        }

        public SettingInfo Title { get; set; }

        public BaseModel SiteSetting { get; set; }

        public BaseModel Country { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public List<BaseModel> Languages { get; set; }

        public List<PhoneInfo> PhoneInfo { get; set; }

        public PhoneInfo MobilePhone { get; set; }

        public FaxInfo FaxInfo { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string Skype { get; set; }

        public List<string> PersonalInfoContact { get; set; }

        public string BirthDayString { get; set; }

        public BankInfo BankInfo { get; set; }

        public bool IsVIP { get; set; }

        public bool IsHeadline { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserInfo Creator { get; set; }

        public BaseModel Role { get; set; }

        public int OriginalDangerLevel { get; set; }

        public int ABTDangerLevel { get; set; }

        public int OtherSportsDangerLevel { get; set; }

        public List<string> BettingCode { get; set; }

        public string Remark2 { get; set; }

        public string Status
        {
            get
            {
                return GetStatusName();
            }
        }

        public bool IsActive { get; set; }

        public bool IsClosed { get; set; }

        public bool IsSuspended { get; set; }

        public List<BaseCustomerInfo> UplineInfo { get; set; }

        public BaseModel Passport { get; set; }

        public AddressInfo AddressInfo { get; set; }

        public bool HeadLine { get; set; }

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <returns>The name of the status.</returns>
        public string GetStatusName()
        {
            return IsActive ? Labels.Active :
                IsClosed ? Labels.Closed :
                IsSuspended ? Labels.Suspended :
                string.Empty;
        }
    }
}
