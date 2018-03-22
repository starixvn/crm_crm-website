// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Add Guest Feedback Criteria.
    /// </summary>
    public class AddGuestFeedbackCriteria : CriteriaBase
    {
        public CustomerInfo CustomerInfo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Setting CommunicationTool { get; set; }

        public Setting Product { get; set; }

        public Setting SubProduct { get; set; }

        public Setting Language { get; set; }

        public Setting Response { get; set; }

        public Setting MainCategory { get; set; }

        public Setting BetType { get; set; }

        public Setting Status { get; set; }

        public Setting Extension { get; set; }

        public Setting SubCategory { get; set; }

        public Country Country { get; set; }

        public Setting Role { get; set; }

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

        public string AutoField { get; set; }

        public string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("userid", UserId);
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
            param.Add("pic1name", CustomerInfo.PersonalInfoContacts[0].Name);
            param.Add("pic1", CustomerInfo.PersonalInfoContacts[0].Phone);
            param.Add("pic2name", CustomerInfo.PersonalInfoContacts[1].Name);
            param.Add("pic2", CustomerInfo.PersonalInfoContacts[1].Phone);
            param.Add("pic3name", CustomerInfo.PersonalInfoContacts[2].Name);
            param.Add("pic3", CustomerInfo.PersonalInfoContacts[2].Phone);
            param.Add("pic4name", CustomerInfo.PersonalInfoContacts[3].Name);
            param.Add("pic4", CustomerInfo.PersonalInfoContacts[3].Phone);
            param.Add("siteid", 0);
            param.Add("isvip", CustomerInfo.IsVIP);
            param.Add("wechat", CustomerInfo.WeChat);
            param.Add("whatsapp", CustomerInfo.WhatsApp);
            param.Add("line", CustomerInfo.Line);
            param.Add("yahoo", CustomerInfo.Yahoo);
            param.Add("qq", CustomerInfo.QQ);
            param.Add("bbm", CustomerInfo.BBM);

            param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(StartTime));
            param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(EndTime));
            param.Add("communicationid", CommunicationTool.Id);
            param.Add("stationid", Extension.Id);
            param.Add("maincategoryid", MainCategory.Id);
            param.Add("subcategoryid", SubCategory.Id);
            param.Add("productid", Product.Id);
            param.Add("subproductid", SubProduct.Id);
            param.Add("bettypeid", BetType.Id);
            param.Add("countryid", Country.Id);
            param.Add("languageid", Language.Id);
            param.Add("responseid", Response.Id);
            param.Add("statusid", Status.Id);
            param.Add("licenseetype", CustomerInfo.LicenseeType.Id);
            param.Add("autofield", AutoField);

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
