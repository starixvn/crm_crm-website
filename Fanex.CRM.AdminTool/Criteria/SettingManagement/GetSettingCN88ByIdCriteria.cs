namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Fanex.Data.Repository;

    public class GetSettingCN88ByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingSelBySettingID";
        }
    }
}