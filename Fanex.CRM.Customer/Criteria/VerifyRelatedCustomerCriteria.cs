namespace Fanex.CRM.Customer.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class VerifyRelatedCustomerCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "CustomerVerify";
        }

        public override bool IsValid() => true;
    }
}