namespace Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting
{
    using Fanex.Data.Repository;

    public class GetCommunicationToolSettingByIdCriteria : CriteriaBase
    {
        public int DefaultCommId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_crmDefaultCommToolSel";

        public override bool IsValid() => true;
    }
}