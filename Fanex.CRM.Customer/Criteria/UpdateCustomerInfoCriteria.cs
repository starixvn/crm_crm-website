namespace Fanex.CRM.Customer.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Common.Criteria;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateCustomerInfoCriteria : BasedCriteria
    {
        public int CustomerId { get; set; }

        public int SiteSettingId { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

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

        public List<PersonalInfoContact> PersonalInfoContacts { get; set; }

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
                return IPHelper.ClientIP;
            }
        }

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("site", SiteSettingId);
            param.Add("countryid", CountryId);
            param.Add("provinceid", ProvinceId);
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
            for (var i = 0; i < PersonalInfoContacts.Count; i++)
            {
                param.Add("pic" + (i + 1).ToString() + "name", PersonalInfoContacts[i].Name);
                param.Add("pic" + (i + 1).ToString(), PersonalInfoContacts[i].Phone);
            }

            param.Add("custid", CustomerId);
            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_Update";
        }

        public override bool IsValid() => true;
    }
}