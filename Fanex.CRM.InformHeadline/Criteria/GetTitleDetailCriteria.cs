namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetTitleDetailCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformTitleDetail";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}