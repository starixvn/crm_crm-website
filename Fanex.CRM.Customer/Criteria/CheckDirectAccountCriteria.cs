namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.Data.Repository;

    public class CheckDirectAccountCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int CustomerId { get; set; }

        public override string GetSettingKey() => "Customer_CheckDirectAccount";

        public override bool IsValid() => CustomerId > 0;
    }
}