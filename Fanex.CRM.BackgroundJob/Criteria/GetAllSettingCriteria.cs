namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.Data.Repository;

    public class GetAllSettingCriteria : CriteriaBase
    {
        public short UserRole { get; } = Constants.UserRole;

        public override string GetSettingKey() => "Acc_CRM_SettingSel";

        public override bool IsValid() => true;
    }
}