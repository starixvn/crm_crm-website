namespace CRM.Criteria.Setting
{
    using Fanex.Data.Repository;

    public class GetSettingCriteria : CriteriaBase
    {
        public string SettingName { get; set; }
        public byte IsActive { get; set; }

        public override string GetSettingKey() => "Acc_CRM_SettingSel";

        public override bool IsValid() => true;
    }
}