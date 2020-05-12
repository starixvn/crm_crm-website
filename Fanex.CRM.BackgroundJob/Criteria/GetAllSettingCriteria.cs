namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllSettingCriteria : CriteriaBase
    {
        public int UserRole { get; set; }

        public override string GetSettingKey() => "Acc_CRM_SettingSel";

        public override bool IsValid() => true;
    }
}