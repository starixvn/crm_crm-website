// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update Customer Information Criteria.
    /// </summary>
    public class UpdateCustomerInfoCriteria : CriteriaBase
    {
        public int CustomerId { get; set; }

        public int SiteSettingId { get; set; }

        public int CountryId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Languages { get; set; }

        public string PhoneInfo1CountryCode { get; set; }

        public string PhoneInfo1AreaCode { get; set; }

        public string PhoneInfo1 { get; set; }

        public string PhoneInfo2CountryCode { get; set; }

        public string PhoneInfo2AreaCode { get; set; }

        public string PhoneInfo2 { get; set; }

        public string PhoneInfo3CountryCode { get; set; }

        public string PhoneInfo3AreaCode { get; set; }

        public string PhoneInfo3 { get; set; }

        public string MobilePhoneCountryCode { get; set; }

        public string MobilePhoneAreaCode { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string Skype { get; set; }

        public List<string> PersonalInfoContact { get; set; }

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

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("site", SiteSettingId);
            param.Add("countryid", CountryId);
            param.Add("firstname", StringExtension.GetValueNotNull(FirstName));
            param.Add("lastname", StringExtension.GetValueNotNull(LastName));
            param.Add("language", StringExtension.GetValueNotNull(Languages));

            param.Add("phone", StringExtension.GetValueNotNull(PhoneInfo1));
            param.Add("phonecountrycode", StringExtension.GetValueNotNull(PhoneInfo1CountryCode));
            param.Add("phoneareacode", StringExtension.GetValueNotNull(PhoneInfo1AreaCode));

            param.Add("phone2", StringExtension.GetValueNotNull(PhoneInfo2));
            param.Add("phone2countrycode", StringExtension.GetValueNotNull(PhoneInfo2CountryCode));
            param.Add("phone2areacode", StringExtension.GetValueNotNull(PhoneInfo2AreaCode));

            param.Add("phone3", StringExtension.GetValueNotNull(PhoneInfo3));
            param.Add("phone3countrycode", StringExtension.GetValueNotNull(PhoneInfo3CountryCode));
            param.Add("phone3areacode", StringExtension.GetValueNotNull(PhoneInfo3AreaCode));

            param.Add("mobilephone", StringExtension.GetValueNotNull(MobilePhone));
            param.Add("mobilephonecountrycode", StringExtension.GetValueNotNull(MobilePhoneCountryCode));
            param.Add("mobilephoneareacode", StringExtension.GetValueNotNull(MobilePhoneAreaCode));

            param.Add("email", StringExtension.GetValueNotNull(Email));
            param.Add("msn", StringExtension.GetValueNotNull(MSN));
            param.Add("skype", StringExtension.GetValueNotNull(Skype));
            for (var i = 0; i < PersonalInfoContact.Count; i++)
            {
                param.Add("pic" + (i + 1).ToString(), PersonalInfoContact[i]);
            }

            param.Add("custid", CustomerId);
            param.Add("userid", UserId);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
