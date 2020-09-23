namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.Data.Repository;

    public class GetCustomerInfoByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }
        public bool IsGuest { get; set; }
        public short UserRole { get; } = Constants.UserRole;

        public override string GetSettingKey() => "Acc_CRM_CustomerSearch";

        public override bool IsValid() => true;
    }
}