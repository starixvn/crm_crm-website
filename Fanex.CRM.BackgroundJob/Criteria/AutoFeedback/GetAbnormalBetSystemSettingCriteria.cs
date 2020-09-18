namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetAbnormalBetSystemSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_SystemSettingSel";

        public override bool IsValid() => true;
    }
}