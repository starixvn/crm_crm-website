namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetRemovedAccountLogsCriteria : CriteriaBase
    {
        public int TitleId { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_InformHeadlineLogSel";

        public override bool IsValid() => TitleId > 0;
    }
}