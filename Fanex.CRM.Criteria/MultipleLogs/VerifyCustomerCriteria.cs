namespace Fanex.CRM.Criteria.MultipleLogs
{
    using Fanex.Data.Repository;

    public class VerifyCustomerCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey() => "MultipleLogs_VerifyCustomer";

        public override bool IsValid() => true;
    }
}