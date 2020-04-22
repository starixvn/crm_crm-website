namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllSettingCriteria : CriteriaBase
    {
        public int UserRole { get; set; } = 2;

        public override string GetSettingKey() => "Acc_CRM_SettingSel";

        public override bool IsValid() => true;
    }
}