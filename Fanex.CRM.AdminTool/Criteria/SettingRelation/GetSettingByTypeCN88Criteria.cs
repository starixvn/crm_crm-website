using Fanex.Data.Repository;

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    public class GetSettingByTypeCN88Criteria : CriteriaBase
    {
        public string SettingName { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingSelBySettingName";
        }

        public override bool IsValid() => true;
    }
}