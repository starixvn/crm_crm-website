namespace Fanex.CRM.Customer.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class UpdateCustomerSuggestionCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "userid")]
        public int UserId { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_UpdateCustomerSuggestion";
        }

        public override bool IsValid() => !string.IsNullOrEmpty(Username);
    }
}