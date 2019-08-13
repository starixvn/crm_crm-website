namespace Fanex.CRM.Dashboard.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateCustomerVerifyFlagCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public bool IsVerifyEmail { get; set; }

        public bool IsVerifyPassport { get; set; }

        public bool IsVerifyBirthday { get; set; }

        public bool IsVerifySecretCode { get; set; }

        public bool IsVerifyBetCredit { get; set; }

        public bool IsVerifyGivenCredit { get; set; }

        public int UserId => AppSession.CurrentUser.Id;

        public string IP => IPHelper.ClientIP;

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_UpdateVerifyFlag";
        }

        public override bool IsValid() => true;
    }
}