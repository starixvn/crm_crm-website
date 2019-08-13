namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using Fanex.CRM.Common.Models;
    using Fanex.Data.Repository;

    public class GetMainSettingForAddRelationCriteria : CriteriaBase
    {
        public Setting MainSetting { get; set; }

        public override bool IsValid() => true;
    }
}