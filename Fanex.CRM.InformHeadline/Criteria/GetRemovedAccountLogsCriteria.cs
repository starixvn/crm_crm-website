namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetRemovedAccountLogsCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() => TitleId > 0;
    }
}