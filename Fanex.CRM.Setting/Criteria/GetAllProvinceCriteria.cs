using Fanex.CRM.Common.Criteria;

namespace Fanex.CRM.Setting.Criteria
{
    public class GetAllProvinceCriteria : LanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_ProvinceSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
