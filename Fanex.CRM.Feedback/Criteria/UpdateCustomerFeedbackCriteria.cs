// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerFeedbackCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Update customer feedback criteria.
    /// </summary>
    public class UpdateCustomerFeedbackCriteria : CustomerFeedback, ICriteria
    {
        public bool OverrideCRMInfo { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public byte UpdateReviewMode { get; set; }

        public System.Collections.Generic.IEnumerable<ProductFilter> Products { get; set; }

        public string ProductsContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Products);
            }
        }

        public System.Collections.Generic.IEnumerable<CaterogyFilter> Categories { get; set; }

        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        public string CommunicationIds { get; set; }

        public string ExtensionIds { get; set; }

        public Hashtable AddSpParams(Hashtable storeProcParamName)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public string GetSettingKey()
        {
            return "FeedbackInfoUpd";
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The input parameters hash.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public Hashtable GetSpParams()
        {
            int userId = AppSession.CurrentUser.Id;
            Hashtable inputParams = new Hashtable
            {
                { "feedbackid", Id },
                { "userid", userId },
                { "custid", CustomerInfo.Id },
                { "titleid", CustomerInfo.Title.Id },
                { "firstname", CustomerInfo.FirstName },
                { "lastname", CustomerInfo.LastName },
                { "phone", CustomerInfo.PhoneInfo[0].PhoneNumber },
                { "phonecountrycode", CustomerInfo.PhoneInfo[0].CountryCode },
                { "phoneareacode", CustomerInfo.PhoneInfo[0].AreaCode },
                { "phone2", CustomerInfo.PhoneInfo[1].PhoneNumber },
                { "phone2countrycode", CustomerInfo.PhoneInfo[1].CountryCode },
                { "phone2areacode", CustomerInfo.PhoneInfo[1].AreaCode },
                { "phone3", CustomerInfo.PhoneInfo[2].PhoneNumber },
                { "phone3countrycode", CustomerInfo.PhoneInfo[2].CountryCode },
                { "phone3areacode", CustomerInfo.PhoneInfo[2].AreaCode },
                { "fax", CustomerInfo.FaxInfo.FaxNumber },
                { "faxcountrycode", CustomerInfo.FaxInfo.CountryCode },
                { "faxareacode", CustomerInfo.FaxInfo.AreaCode },
                { "mobilephone", CustomerInfo.MobilePhone.PhoneNumber },
                { "mobilephonecountrycode", CustomerInfo.MobilePhone.CountryCode },
                { "mobilephoneareacode", CustomerInfo.MobilePhone.AreaCode },
                { "skype", CustomerInfo.Skype },
                { "email", CustomerInfo.Email },
                { "msn", CustomerInfo.MSN },
                { "passport", CustomerInfo.Passport.Name },
                { "siteid", CustomerInfo.SiteSetting.Id },
                { "bettingcode", CustomerInfo.BettingCode[0] },
                { "refcode2", CustomerInfo.BettingCode[1] },
                { "pic1name", CustomerInfo.PersonalInfoContacts[0].Name },
                { "pic1", CustomerInfo.PersonalInfoContacts[0].Phone },
                { "pic2name", CustomerInfo.PersonalInfoContacts[1].Name },
                { "pic2", CustomerInfo.PersonalInfoContacts[1].Phone },
                { "pic3name", CustomerInfo.PersonalInfoContacts[2].Name },
                { "pic3", CustomerInfo.PersonalInfoContacts[2].Phone },
                { "pic4name", CustomerInfo.PersonalInfoContacts[3].Name },
                { "pic4", CustomerInfo.PersonalInfoContacts[3].Phone },
                { "licenseetype", CustomerInfo.LicenseeType.Id },
                { "isvip", CustomerInfo.IsVIP },
                { "isoverwrite", OverrideCRMInfo },
                { "wechat", CustomerInfo.WeChat },
                { "whatsapp", CustomerInfo.WhatsApp },
                { "line", CustomerInfo.Line },
                { "yahoo", CustomerInfo.Yahoo },
                { "qq", CustomerInfo.QQ },
                { "bbm", CustomerInfo.BBM },

                { "communicationid", CommunicationIds },
                { "stationid", ExtensionIds },
                { "category", CategoriesContent },
                { "product", ProductsContent },

                { "countryid", Country.Id },
                { "languageid", Language.Id },
                { "statusid", Status.Id },
                { "roleid", Role.Id },
                { "responseid", Response.Id },
                { "subject", Subject },
                { "description", Description },
                { "solution", Solution },
                { "isreviewed", IsReviewed },
                { "autofield", AutoField },
                { "ip", IPHelper.ClientIP },
                { "errcode", ErrorCode },
                { "errmsg", ErrorMessage }
        };

            DateTime birthday, feedbackTime, resolutionTime;
            var sqlMinDate = new DateTime(1900, 1, 1);
            if (DateTime.TryParse(CustomerInfo.BirthDayString, out birthday) && birthday >= sqlMinDate)
            {
                inputParams.Add("birthday", birthday);
            }

            if (DateTime.TryParse(FeedbackTimeString, out feedbackTime))
            {
                inputParams.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(feedbackTime));
            }

            if (DateTime.TryParse(ResolutionTimeString, out resolutionTime))
            {
                inputParams.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(resolutionTime));
            }

            // RelatedCustomer parameter:
            // null: don't update anything.
            // string.Empty: update RelatedCustomer to empty.
            inputParams.Add("relatedcustid", GetRelatedCustomerId());

            if (IsReviewed)
            {
                inputParams.Add("review", Review);
                inputParams.Add("reviewerid", userId);
            }
            else
            {
                inputParams.Add("review", DBNull.Value);
            }

            // UpdateReviewedMode (tinyint) parameter:
            // 0: Do nothing.
            // 1: Update review.
            // 2: Remove review.
            inputParams.Add("updatereviewedmode", UpdateReviewMode);
            return inputParams;
        }

        private string GetRelatedCustomerId()
        {
            if (HasRelated && RelatedCustomers != null && RelatedCustomers.Count() > 0)
            {
                return string.Join(",", RelatedCustomers.Select(c => c.Id));
            }

            return string.Empty;
        }
    }
}
