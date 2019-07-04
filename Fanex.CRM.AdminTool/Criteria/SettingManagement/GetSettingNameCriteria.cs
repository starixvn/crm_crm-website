using Fanex.Data.Repository;

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    public class GetSettingNameCriteria : CriteriaBase
    {
        public string SettingName { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingNameSelByName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
