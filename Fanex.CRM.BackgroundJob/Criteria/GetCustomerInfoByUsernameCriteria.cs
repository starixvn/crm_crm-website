namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetCustomerInfoByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }
        public bool IsGuest { get; set; }
        public byte UserRole { get; set; } = 2;

        public override string GetSettingKey() => "Acc_CRM_CustomerSearch";

        public override bool IsValid() => true;
    }
}