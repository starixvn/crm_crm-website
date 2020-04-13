namespace Fanex.CRM.Criteria.MultipleLogs
{
    using Fanex.Data.Repository;

    public class VerifyRelatedCustomerCriteria : CriteriaBase
    {
        public string HeadlineIds { get; set; }
        public string Username { get; set; }

        public override string GetSettingKey() => "MultipleLogs_VerifyRelatedCustomer";

        public override bool IsValid() => true;
    }
}