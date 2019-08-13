namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllSettingRelationCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingRelationSel";
        }

        public override bool IsValid() => true;
    }
}