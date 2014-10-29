// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Add Guest Feedback Criteria.
    /// </summary>
    public class AddGuestFeedbackCriteria : CriteriaBase
    {
        public CustomerInfo CustomerInfo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public SettingInfo CommunicationTool { get; set; }

        public SettingInfo Product { get; set; }

        public SettingInfo Language { get; set; }

        public SettingInfo Response { get; set; }

        public SettingInfo MainCategory { get; set; }

        public SettingInfo BetType { get; set; }

        public SettingInfo Status { get; set; }

        public SettingInfo Extension { get; set; }

        public SettingInfo SubCategory { get; set; }

        public CountryInfo Country { get; set; }

        public SettingInfo Role { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool HasRelated { get; set; }

        public string RelatedCustomerId { get; set; }

        public bool IsReviewed { get; set; }

        public string Review { get; set; }

        public string ReviewerId { get; set; }

        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string IP
        {
            get
            {
                return Helper.GetClientIP();
            }
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("userid", CustomerInfo.Id);
            param.Add("username", CustomerInfo.Name);

            param.Add("firstname", CustomerInfo.FirstName);
            param.Add("lastname", CustomerInfo.LastName);
            param.Add("titleid", CustomerInfo.Title.Id);
            param.Add("birthday", CustomerInfo.BirthDayString);
            param.Add("phone", CustomerInfo.PhoneInfo[0].PhoneNumber);
            param.Add("phonecountrycode", CustomerInfo.PhoneInfo[0].CountryCode);
            param.Add("phoneareacode", CustomerInfo.PhoneInfo[0].AreaCode);

            param.Add("phone2", CustomerInfo.PhoneInfo[1].PhoneNumber);
            param.Add("phone2countrycode", CustomerInfo.PhoneInfo[1].CountryCode);
            param.Add("phone2areacode", CustomerInfo.PhoneInfo[1].AreaCode);
            param.Add("phone3", CustomerInfo.PhoneInfo[2].PhoneNumber);
            param.Add("phone3countrycode", CustomerInfo.PhoneInfo[2].CountryCode);
            param.Add("phone3areacode", CustomerInfo.PhoneInfo[2].AreaCode);
            param.Add("mobilephone", CustomerInfo.MobilePhone.PhoneNumber);
            param.Add("mobilephonecountrycode", CustomerInfo.MobilePhone.CountryCode);
            param.Add("mobilephoneareacode", CustomerInfo.MobilePhone.AreaCode);
            param.Add("fax", CustomerInfo.FaxInfo.FaxNumber);
            param.Add("faxcountrycode", CustomerInfo.FaxInfo.CountryCode);
            param.Add("faxareacode", CustomerInfo.FaxInfo.AreaCode);

            param.Add("passport", CustomerInfo.Passport.Name);
            param.Add("email", CustomerInfo.Email);
            param.Add("skype", CustomerInfo.Skype);
            param.Add("msn", CustomerInfo.MSN);
            param.Add("bettingcode", CustomerInfo.BettingCode[0]);
            param.Add("refcode2", CustomerInfo.BettingCode[1]);
            param.Add("pic1", CustomerInfo.PersonalInfoContact[0]);
            param.Add("pic2", CustomerInfo.PersonalInfoContact[1]);
            param.Add("pic3", CustomerInfo.PersonalInfoContact[2]);
            param.Add("pic4", CustomerInfo.PersonalInfoContact[3]);
            param.Add("siteid", 0);
            param.Add("isvip", CustomerInfo.IsVIP);

            param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(StartTime));
            param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(EndTime));
            param.Add("communicationid", CommunicationTool.Id);
            param.Add("stationid", Extension.Id);
            param.Add("maincategoryid", MainCategory.Id);
            param.Add("subcategoryid", SubCategory.Id);
            param.Add("productid", Product.Id);
            param.Add("bettypeid", BetType.Id);
            param.Add("countryid", Country.Id);
            param.Add("languageid", Language.Id);
            param.Add("responseid", Response.Id);
            param.Add("statusid", Status.Id);

            param.Add("relatedcustid", RelatedCustomerId);
            param.Add("subject", Subject);
            param.Add("description", Description);
            param.Add("solution", Solution);
            param.Add("isreviewed", IsReviewed);
            if (IsReviewed)
            {
                param.Add("review", Review);
                param.Add("reviewerid", ReviewerId);
            }

            param.Add("creatorid", CreatorId);
            param.Add("ip", IP);
            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_GuestFeedbackIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
