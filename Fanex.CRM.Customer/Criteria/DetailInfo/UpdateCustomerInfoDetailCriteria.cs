// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerInfoDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.DetailInfo
{
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Customer Information Detail Criteria.
    /// </summary>
    public class UpdateCustomerInfoDetailCriteria : CriteriaBase
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

        [SpParam(SqlParamName = "pic1")]
        public string PersonalInfoContact1 { get; set; }

        [SpParam(SqlParamName = "pic2")]
        public string PersonalInfoContact2 { get; set; }

        [SpParam(SqlParamName = "pic3")]
        public string PersonalInfoContact3 { get; set; }

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

        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return Helper.GetClientIP();
            }
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

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
