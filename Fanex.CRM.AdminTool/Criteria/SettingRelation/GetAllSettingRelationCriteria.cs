namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
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