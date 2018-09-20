// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerInfoDetailCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.DetailInfo
{
    using Fanex.CRM.Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update Customer Information Detail Criteria.
    /// </summary>
    public class UpdateCustomerInfoDetailCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public string CustomerId { get; set; }

        [SpParam(SqlParamName = "firstname")]
        public string FirstName { get; set; }

        [SpParam(SqlParamName = "lastname")]
        public string LastName { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public string LanguageId { get; set; }

        [SpParam(SqlParamName = "phone")]
        public string Phone1PhoneNumber { get; set; }

        [SpParam(SqlParamName = "phonecountrycode")]
        public string Phone1CountryCode { get; set; }

        [SpParam(SqlParamName = "phoneareacode")]
        public string Phone1AreaCode { get; set; }

        [SpParam(SqlParamName = "phone2")]
        public string Phone2PhoneNumber { get; set; }

        [SpParam(SqlParamName = "phone2countrycode")]
        public string Phone2CountryCode { get; set; }

        [SpParam(SqlParamName = "phone2areacode")]
        public string Phone2AreaCode { get; set; }

        [SpParam(SqlParamName = "phone3")]
        public string Phone3PhoneNumber { get; set; }

        [SpParam(SqlParamName = "phone3countrycode")]
        public string Phone3CountryCode { get; set; }

        [SpParam(SqlParamName = "phone3areacode")]
        public string Phone3AreaCode { get; set; }

        [SpParam(SqlParamName = "passport")]
        public string PassportNumber { get; set; }

        [SpParam(SqlParamName = "mobilephone")]
        public string MobilePhonePhoneNumber { get; set; }

        [SpParam(SqlParamName = "mobilephonecountrycode")]
        public string MobilePhoneCountryCode { get; set; }

        [SpParam(SqlParamName = "mobilephoneareacode")]
        public string MobilePhoneAreaCode { get; set; }

        [SpParam(SqlParamName = "fax")]
        public string FaxNumber { get; set; }

        [SpParam(SqlParamName = "faxcountrycode")]
        public string FaxCountryCode { get; set; }

        [SpParam(SqlParamName = "faxareacode")]
        public string FaxAreaCode { get; set; }

        [SpParam(SqlParamName = "email")]
        public string Email { get; set; }

        [SpParam(SqlParamName = "skype")]
        public string Skype { get; set; }

        [SpParam(SqlParamName = "msn")]
        public string MSN { get; set; }

        [SpParam(SqlParamName = "wechat")]
        public string WeChat { get; set; }

        [SpParam(SqlParamName = "whatsapp")]
        public string WhatsApp { get; set; }

        [SpParam(SqlParamName = "line")]
        public string Line { get; set; }

        [SpParam(SqlParamName = "yahoo")]
        public string Yahoo { get; set; }

        [SpParam(SqlParamName = "qq")]
        public string QQ { get; set; }

        [SpParam(SqlParamName = "bbm")]
        public string BBM { get; set; }

        [SpParam(SqlParamName = "pic1name")]
        public string PersonalInfoContactName1 { get; set; }

        [SpParam(SqlParamName = "pic1")]
        public string PersonalInfoContact1 { get; set; }

        [SpParam(SqlParamName = "pic2name")]
        public string PersonalInfoContactName2 { get; set; }

        [SpParam(SqlParamName = "pic2")]
        public string PersonalInfoContact2 { get; set; }

        [SpParam(SqlParamName = "pic3name")]
        public string PersonalInfoContactName3 { get; set; }

        [SpParam(SqlParamName = "pic3")]
        public string PersonalInfoContact3 { get; set; }

        [SpParam(SqlParamName = "pic4name")]
        public string PersonalInfoContactName4 { get; set; }

        [SpParam(SqlParamName = "pic4")]
        public string PersonalInfoContact4 { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public string SiteId { get; set; }

        [SpParam(SqlParamName = "line1")]
        public string HomeAddressLine1 { get; set; }

        [SpParam(SqlParamName = "line2")]
        public string HomeAddressLine2 { get; set; }

        [SpParam(SqlParamName = "line3")]
        public string HomeAddressLine3 { get; set; }

        [SpParam(SqlParamName = "city")]
        public string HomeAddressCity { get; set; }

        [SpParam(SqlParamName = "countryid")]
        public string HomeAddressCountryId { get; set; }

        [SpParam(SqlParamName = "postcode")]
        public string HomeAddressPostCode { get; set; }

        [SpParam(SqlParamName = "state")]
        public string HomeAddressState { get; set; }

        [SpParam(SqlParamName = "bankline1")]
        public string BankAddressLine1 { get; set; }

        [SpParam(SqlParamName = "bankline2")]
        public string BankAddressLine2 { get; set; }

        [SpParam(SqlParamName = "bankline3")]
        public string BankAddressLine3 { get; set; }

        [SpParam(SqlParamName = "bankcity")]
        public string BankAddressCity { get; set; }

        [SpParam(SqlParamName = "bankcode")]
        public string BankAddressPostCode { get; set; }

        [SpParam(SqlParamName = "bankstate")]
        public string BankAddressState { get; set; }

        [SpParam(SqlParamName = "bankcountryid")]
        public string BankAddressCountryId { get; set; }

        [SpParam(SqlParamName = "wire_bankname")]
        public string BankName { get; set; }

        [SpParam(SqlParamName = "accountholdername")]
        public string BankHolderName { get; set; }

        [SpParam(SqlParamName = "wire_accountno")]
        public string BankAccountNumber { get; set; }

        [SpParam(SqlParamName = "wire_swiftcode")]
        public string BankSwiftCode { get; set; }

        [SpParam(SqlParamName = "birthday")]
        public string BirthDay { get; set; }

        [SpParam(SqlParamName = "licenseetype")]
        public string LicenseeTypeId { get; set; }

        [SpParam(SqlParamName = "isvip")]
        public string IsVIP { get; set; }

        [SpParam(SqlParamName = "ignored")]
        public string IsIgnored { get; set; }

        [SpParam(SqlParamName = "titleid")]
        public string TitleId { get; set; }

        [SpParam(SqlParamName = "bettingcode")]
        public string BettingCode { get; set; }

        [SpParam(SqlParamName = "refcode2")]
        public string RefCode2 { get; set; }

        public string IntroducedBy { get; set; }

        public bool IsUpdateInfoHeadline { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfoDetail_Update_New";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
