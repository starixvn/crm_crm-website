using Fanex.CRM.Common.Criteria;

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    public class GetMainSettingCriteria : LanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingSel";
        }

        public override bool IsValid() => true;
    }
}