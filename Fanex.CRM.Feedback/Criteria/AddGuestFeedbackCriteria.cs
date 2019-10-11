namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Common.Criteria;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Models;

    public class AddGuestFeedbackCriteria : BasedCriteria
    {
        public CustomerInfo CustomerInfo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CommunicationIds { get; set; }

        public IEnumerable<ProductFilter> Products { get; set; }

        public string ProductsContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Products);
            }
        }

        public Setting Language { get; set; }

        public Setting Response { get; set; }

        public IEnumerable<CategoryFilter> Categories { get; set; }

        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        public Setting Status { get; set; }

        public string ExtensionIds { get; set; }

        public Country Country { get; set; }

        public Province Province { get; set; }

        public Setting Role { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool HasRelated { get; set; }

        public string RelatedCustomerId { get; set; }

        public bool IsReviewed { get; set; }

        public string Review { get; set; }

        public string ReviewerId { get; set; }

        public int CreatorId => AppSession.CurrentUser.Id;

        public int UserId => AppSession.CurrentUser.Id;

        public string AutoField { get; set; }

        public string IP => IPHelper.ClientIP;

        public int NoInquiries { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override Hashtable GetSpParams()
        {
            var isCN88 = UserRole == Common.Enumerations.UserRole.CN88Staff;
            var siteId = isCN88 ? SiteConstants.CN88 : 0;
            var roleId = isCN88 ? RoleIdConstants.CN88Guest : RoleIdConstants.CRMGuest;

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
            param.Add("siteid", siteId);
            param.Add("roleid", roleId);
            param.Add("isvip", CustomerInfo.IsVIP);
            param.Add("wechat", CustomerInfo.WeChat);
            param.Add("whatsapp", CustomerInfo.WhatsApp);
            param.Add("line", CustomerInfo.Line);
            param.Add("yahoo", CustomerInfo.Yahoo);
            param.Add("qq", CustomerInfo.QQ);
            param.Add("bbm", CustomerInfo.BBM);

            param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(StartTime));
            param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(EndTime));
            param.Add("countryid", Country.Id);
            param.Add("provinceid", Province.Id);
            param.Add("languageid", Language.Id);
            param.Add("responseid", Response.Id);
            param.Add("statusid", Status.Id);
            param.Add("licenseetype", CustomerInfo.LicenseeType.Id);
            param.Add("autofield", AutoField);

            param.Add("communicationid", CommunicationIds);
            param.Add("stationid", ExtensionIds);
            param.Add("category", CategoriesContent);
            param.Add("product", ProductsContent);

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
            param.Add("NoInquiries", NoInquiries);

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

        public override bool IsValid() => true;
    }
}