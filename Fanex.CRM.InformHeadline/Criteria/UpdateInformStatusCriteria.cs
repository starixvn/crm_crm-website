namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.CRM.Common.Criteria;

    public class UpdateInformStatusCriteria : UpdateCriteria
    {
        public int TitleId { get; set; }

        public bool StatusType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformTitleStatusUpd";
        }

        public override bool IsValid() => true;
    }
}