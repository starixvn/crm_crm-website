namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllSettingRelationCriteria : CriteriaBase
    {
        public int UserRole { get; set; }

        public override string GetSettingKey() => "Acc_CRM_SettingRelationSel";

        public override bool IsValid() => true;
    }
}